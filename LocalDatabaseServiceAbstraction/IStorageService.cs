using SQLite;
using System;
using System.Threading.Tasks;

namespace LocalDatabaseServiceAbstraction
{
    public interface IStorageService
    {
        SQLiteConnection GetConnection(string databaseName);

        Task DropDatabase(string databaseName);
    }
}
