using System;
using System.IO;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
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

            Context context = Application.Context;
            context.DeleteDatabase(path);

            return Task.FromResult(0);
        }

        private string GetDatabasePath(string databaseName)
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string databasePath = Path.Combine(documentsPath, databaseName);

            return databasePath;
        }
    }
}