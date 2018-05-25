using System;
using System.IO;
using System.Threading.Tasks;
using Foundation;
using LocalDatabaseServiceAbstraction;
using SQLite;

namespace LocalDatabaseService.LocalStorage
{
    public class StorageService : IStorageService
    {
        private SQLiteConnection connection;

        public SQLiteConnection GetConnection(string databaseName)
        {
            string path = GetDatabasePath(databaseName);
            connection = new SQLiteConnection(path);

            return connection;
        }

        public Task DropDatabase(string databaseName)
        {
            connection.Close();
            string path = GetDatabasePath(databaseName);

            if (NSFileManager.DefaultManager.FileExists(path))
            {
                NSError error = new NSError();
                NSFileManager.DefaultManager.Remove(path, out error);
            }

            return Task.FromResult(0);
        }

        private string GetDatabasePath(string databaseName)
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            string libraryPath = Path.Combine(documentsPath, "..", "Library");
            string databasePath = Path.Combine(libraryPath, databaseName);

            return databasePath;
        }
    }
}