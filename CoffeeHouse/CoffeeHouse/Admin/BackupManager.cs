using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CoffeeHouse.Admin
{
    public static class BackupManager
    {
        // Изменяем путь на локальную папку приложения
        public static readonly string BackupFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
            "CoffeeHouse", "Backups");

        public static void CreateBackup(CoffeeHouseEntities context)
        {
            try
            {
                // Создаем папку, если ее нет
                Directory.CreateDirectory(BackupFolder);

                string backupFileName = $"CoffeeHouseBackup_{DateTime.Now:yyyyMMdd_HHmmss}.bak";
                string backupPath = Path.Combine(BackupFolder, backupFileName);

                // Проверяем доступность пути для SQL Server
                if (!CanWriteToFolder(BackupFolder))
                {
                    MessageBox.Show("SQL Server не имеет прав на запись в указанную папку. " +
                                  "Пожалуйста, запустите приложение от имени администратора или " +
                                  "измените права доступа к папке.",
                                  "Ошибка прав доступа", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Формируем команду BACKUP
                string backupCommand = $@"BACKUP DATABASE [{context.Database.Connection.Database}] 
                                  TO DISK = '{backupPath}' 
                                  WITH FORMAT, MEDIANAME = 'CoffeeHouseBackups', 
                                  NAME = 'Полная резервная копия CoffeeHouse';";

                // Выполняем команду
                context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, backupCommand);

                MessageBox.Show($"Резервная копия успешно создана:\n{backupPath}",
                              "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (SqlException sqlEx)
            {
                HandleSqlError(sqlEx);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании резервной копии: {ex.Message}",
                              "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static bool CanWriteToFolder(string folderPath)
        {
            try
            {
                string testFile = Path.Combine(folderPath, "test.tmp");
                File.WriteAllText(testFile, "test");
                File.Delete(testFile);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void HandleSqlError(SqlException sqlEx)
        {
            if (sqlEx.Number == 3201) // Ошибка доступа к устройству
            {
                MessageBox.Show("SQL Server не имеет прав на запись в указанную папку.\n" +
                              "Попробуйте:\n" +
                              "1. Запустить приложение от имени администратора\n" +
                              "2. Изменить путь сохранения резервных копий\n" +
                              "3. Вручную предоставить права SQL Server на папку",
                              "Ошибка доступа", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show($"SQL Ошибка ({sqlEx.Number}): {sqlEx.Message}",
                              "Ошибка базы данных", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public static void RestoreBackup(CoffeeHouseEntities context, string backupPath)
        {
            try
            {
                // Проверяем существование файла бэкапа
                if (!File.Exists(backupPath))
                {
                    MessageBox.Show("Файл резервной копии не найден",
                                  "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Закрываем все соединения с БД
                context.Database.Connection.Close();

                // Выполняем восстановление
                string restoreCommand = $"USE master; " +
                                      $"RESTORE DATABASE [{context.Database.Connection.Database}] " +
                                      $"FROM DISK = '{backupPath}' " +
                                      $"WITH REPLACE, RECOVERY;";

                // Create a new connection to the master database
                string masterConnectionString = context.Database.Connection.ConnectionString
                    .Replace($"Initial Catalog={context.Database.Connection.Database}",
                            "Initial Catalog=master");

                using (var masterConnection = new SqlConnection(masterConnectionString))
                {
                    masterConnection.Open();
                    using (var command = new SqlCommand(restoreCommand, masterConnection))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Восстановление из резервной копии успешно завершено",
                              "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при восстановлении: {ex.Message}",
                              "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
            finally
            {
                // Пересоздаем контекст после восстановления
                context.Dispose();
                context = new  CoffeeHouseEntities();
            }
        }
    }
}
