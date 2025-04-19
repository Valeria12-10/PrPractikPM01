using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoffeeHouse;
using System.Windows;
using System.Linq;
using CoffeeHouse.Admin;
using CoffeeHouse.Manager;
using System.Collections.Generic;
using Moq;
using System.Data.Entity;
using System.Windows.Controls;

namespace CoffeeHouseTests
{
    [TestClass]
    public class MainWindowTests
    {
        private CoffeeHouseEntities _mockContext;
        private Mock<DbSet<Сотрудники>> _mockEmployeesSet;

        [TestInitialize]
        public void TestInitialize()
        {
            // Настройка моков для тестов
            var testEmployees = new[]
            {
                new Сотрудники
                {
                    IDСотрудника = 1,
                    ФИО = "Иванов Иван Иванович",
                    Логин = "admin",
                    ХешПароля = "admin123",
                    Email = "valeria1739poglazova@gmail.com",
                    ГрафикРаботы = "Пн-Пт 9:00-18:00",
                    Роль = 1
                },
                new Сотрудники
                {
                    IDСотрудника = 2,
                    ФИО = "Петрова Анна Сергеевны",
                    Логин = "manager",
                    ХешПароля = "manager123",
                    Email = "valeria1739poglazova@gmail.com",
                    ГрафикРаботы = "Пн-Сб 10:00-19:00",
                    Роль = 2
                }
            }.AsQueryable();

            _mockEmployeesSet = new Mock<DbSet<Сотрудники>>();
            _mockEmployeesSet.As<IQueryable<Сотрудники>>().Setup(m => m.Provider).Returns(testEmployees.Provider);
            _mockEmployeesSet.As<IQueryable<Сотрудники>>().Setup(m => m.Expression).Returns(testEmployees.Expression);
            _mockEmployeesSet.As<IQueryable<Сотрудники>>().Setup(m => m.ElementType).Returns(testEmployees.ElementType);
            _mockEmployeesSet.As<IQueryable<Сотрудники>>().Setup(m => m.GetEnumerator()).Returns(testEmployees.GetEnumerator());

            _mockContext = new Mock<CoffeeHouseEntities>().Object;
            _mockContext.Сотрудники = _mockEmployeesSet.Object;
        }

        [TestMethod]
        public void Next_Click_WithEmptyFields_ShowsErrorMessage()
        {
            // Arrange
            var window = new MainWindow();

            // Используем рефлексию для установки значений, так как свойства только для чтения
            var loginField = typeof(MainWindow).GetField("lg", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var passwordField = typeof(MainWindow).GetField("pr", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            loginField.SetValue(window, new TextBox { Text = "" });
            passwordField.SetValue(window, new PasswordBox { Password = "" });

            // Act
            window.Next_Click(null, null);

            // Assert
            Assert.AreEqual("Заполните все поля", MessageBoxHelper.LastMessage);
        }

        [TestMethod]
        public void Next_Click_WithInvalidCredentials_ShowsErrorMessage()
        {
            // Arrange
            var window = new MainWindow();

            var loginField = typeof(MainWindow).GetField("lg", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var passwordField = typeof(MainWindow).GetField("pr", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            loginField.SetValue(window, new TextBox { Text = "wrong" });
            passwordField.SetValue(window, new PasswordBox { Password = "wrong" });

            // Act
            window.Next_Click(null, null);

            // Assert
            Assert.AreEqual("Неверный логин или пароль.", MessageBoxHelper.LastMessage);
        }

        [TestMethod]
        public void Next_Click_WithAdminCredentials_OpensAdminWindow()
        {
            // Arrange
            var window = new MainWindow();

            var loginField = typeof(MainWindow).GetField("lg", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var passwordField = typeof(MainWindow).GetField("pr", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            loginField.SetValue(window, new TextBox { Text = "admin" });
            passwordField.SetValue(window, new PasswordBox { Password = "admin123" });

            // Act
            window.Next_Click(null, null);

            // Assert
            Assert.IsInstanceOfType(WindowHelper.LastOpenedWindow, typeof(AdminWindow));
        }

        // Остальные тесты аналогично...
    }

    public static class MessageBoxHelper
    {
        public static string LastMessage { get; set; }

        public static MessageBoxResult Show(string message)
        {
            LastMessage = message;
            return MessageBoxResult.OK;
        }
    }

    public static class WindowHelper
    {
        public static Window LastOpenedWindow { get; set; }

        public static void Show(Window window)
        {
            LastOpenedWindow = window;
        }
    }

}