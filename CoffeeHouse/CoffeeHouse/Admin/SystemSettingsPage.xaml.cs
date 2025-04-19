using System;
using System.Windows;
using System.Windows.Controls;
using CoffeeHouse.Properties;

namespace CoffeeHouse.Admin
{
    public partial class SystemSettingsPage : Page

    {

        public interface IBackupManager
        {
            string BackupFolder { get; }
            void CreateBackup(CoffeeHouseEntities context);
            void RestoreBackup(CoffeeHouseEntities context, string backupPath);

        }

        private readonly IBackupManager _backupManager;

        public SystemSettingsPage(IBackupManager backupManager = null)
        {
            InitializeComponent();
            _backupManager = backupManager ?? new DefaultBackupManager();
            LoadSettings();
        }

        private class DefaultBackupManager : IBackupManager
        {
            public string BackupFolder => BackupManager.BackupFolder;

            public void CreateBackup(CoffeeHouseEntities context)
            {
                BackupManager.CreateBackup(context);
            }

            public void RestoreBackup(CoffeeHouseEntities context, string backupPath)
            {
                BackupManager.RestoreBackup(context, backupPath);
            }
        }

        public void LoadSettings()
        {
            try
            {
                // Загрузка текущих настроек из Properties.Settings
                CafeNameTextBox.Text = Settings.Default.CafeName;
                WorkingHoursTextBox.Text = Settings.Default.WorkingHours;
                PhoneTextBox.Text = Settings.Default.PhoneNumber;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке настроек: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Валидация введенных данных
                if (string.IsNullOrWhiteSpace(CafeNameTextBox.Text))
                {
                    MessageBox.Show("Введите название заведения", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Сохранение настроек в Properties.Settings
                Settings.Default.CafeName = CafeNameTextBox.Text;
                Settings.Default.WorkingHours = WorkingHoursTextBox.Text;
                Settings.Default.PhoneNumber = PhoneTextBox.Text;
                Settings.Default.Save();

                MessageBox.Show("Настройки успешно сохранены", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении настроек: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void CreateBackup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = MessageBox.Show("Создать резервную копию базы данных?",
                    "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    using (var context = new CoffeeHouseEntities())
                    {
                        BackupManager.CreateBackup(context);
                        MessageBox.Show("Резервная копия успешно создана", "Успех",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании резервной копии: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void RestoreBackup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var openFileDialog = new Microsoft.Win32.OpenFileDialog
                {
                    Filter = "Backup files (*.bak)|*.bak|All files (*.*)|*.*",
                    InitialDirectory = BackupManager.BackupFolder,
                    Title = "Выберите файл резервной копии для восстановления"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    var result = MessageBox.Show(
                        "Внимание! Восстановление из резервной копии заменит все текущие данные.\nПродолжить?",
                        "Подтверждение",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        using (var context = new CoffeeHouseEntities())
                        {
                            BackupManager.RestoreBackup(context, openFileDialog.FileName);
                        }

                        MessageBox.Show(
                            "Данные успешно восстановлены из резервной копии.\nПриложение будет перезапущено.",
                            "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                        // Перезапуск приложения
                        System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                        Application.Current.Shutdown();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при восстановлении: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}