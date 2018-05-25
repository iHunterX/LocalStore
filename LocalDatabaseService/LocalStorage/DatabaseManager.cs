using LocalDatabaseServiceAbstraction;
using SQLite;
using System.Collections.Generic;
using System.Linq;

namespace LocalDatabaseService.LocalStorage
{
    public class DatabaseManager
    {
        // Properties
        private static DatabaseManager _instance;
        private readonly object _locker;
        public static DatabaseManager Instance => _instance ?? (_instance = new DatabaseManager());

        public DatabaseManager()
        {
            _locker = new object();
        }

        // Resources
        private SQLiteConnection _database;
        private const string DatabaseName = "database.local";

        // Delete
        public void DeleteDatabase()
        {
            IStorageService storageService = CrossStorageService.Current;
            storageService.DropDatabase(DatabaseName);
        }

        public void DeleteElements<T>(List<T> storedElementsDatabase)
        {
            lock (_locker)
            {
                foreach (T storedElementDatabase in storedElementsDatabase)
                {
                    _database.Delete(storedElementDatabase);
                }
            }
        }

        public void Delete<T>(T element)
        {
            lock (_locker)
            {
                _database.Delete(element);
            }
        }

        public void DeleteAll<T>()
        {
            lock (_locker)
            {
                _database.DeleteAll<T>();
            }
        }

        // Read
        public T GetElement<T>(int databaseId) where T : IIntIdEntity, new()
        {
            TableQuery<T> tableElementsDatabase = GetTable<T>();
            return tableElementsDatabase.AsEnumerable().SingleOrDefault(x => x.DatabaseID == databaseId);
        }

        public T GetElement<T>(string databaseId) where T : IStringIdEntity, new()
        {
            TableQuery<T> tableElementsDatabase = GetTable<T>();
            return tableElementsDatabase.AsEnumerable().SingleOrDefault(x => x.DatabaseID.Equals(databaseId));
        }

        public List<T> GetAllElements<T>() where T : new()
        {
            lock (_locker)
            {
                try
                {
                    TableQuery<T> tableElementsDatabase = _database.Table<T>();
                    return tableElementsDatabase.ToList();
                }
                catch
                {
                    _database.CreateTable<T>();
                    return new List<T>();
                }
            }
        }

        public TableQuery<T> GetTable<T>() where T : new()
        {
            lock (_locker)
            {
                return _database.Table<T>();
            }
        }

        // Create
        public void CreateTable<T>() where T : new()
        {
            lock (_locker)
            {
                _database.CreateTable<T>();
            }
        }

        public void CreateDatabase()
        {
            IStorageService storageService = CrossStorageService.Current;
            lock (_locker)
            {
                _database = storageService.GetConnection(DatabaseName);
            }
        }

        // Update
        public void UpdateAll<T>(List<T> elements) where T : new()
        {
            lock (_locker)
            {
                _database.UpdateAll(elements);
            }
        }

        public void Update<T>(T element) where T : new()
        {
            lock (_locker)
            {
                _database.Update(element);
            }
        }

        public void SaveIntIdEntities<T>(List<T> elements) where T : IIntIdEntity, new()
        {
            List<T> elementsDatabase = GetAllElements<T>();
            foreach (T element in elements)
            {
                bool updateElementsDatabse = InsertOrReplaceIntIdEntity(element, elementsDatabase);
                if (updateElementsDatabse)
                {
                    elementsDatabase = GetAllElements<T>();
                }
            }
        }

        public void SaveStringIdEntities<T>(List<T> elements) where T : IStringIdEntity, new()
        {
            List<T> elementsDatabase = GetAllElements<T>();
            foreach (T element in elements)
            {
                bool updateElementsDatabse = InsertOrReplaceStringIdEntity(element, elementsDatabase);
                if (updateElementsDatabse)
                {
                    elementsDatabase = GetAllElements<T>();
                }
            }
        }

        public void SaveIntIdEntity<T>(T element) where T : IIntIdEntity, new()
        {
            List<T> elementsDatabase = GetAllElements<T>();
            InsertOrReplaceIntIdEntity(element, elementsDatabase);
        }

        public void SaveStringIdEntity<T>(T element) where T : IStringIdEntity, new()
        {
            List<T> elementsDatabase = GetAllElements<T>();
            InsertOrReplaceStringIdEntity(element, elementsDatabase);
        }

        public void ReplaceAll<T>(List<T> elementsDatabase) where T : new()
        {
            lock (_locker)
            {
                _database.DeleteAll<T>();
                _database.InsertAll(elementsDatabase);
            }
        }

        // Private
        private bool InsertOrReplaceIntIdEntity<T>(T element, List<T> elements) where T : IIntIdEntity
        {
            lock (_locker)
            {
                bool exists = elements.Any(x => x.DatabaseID == element.DatabaseID);
                if (exists)
                {
                    _database.Update(element);
                    return false;
                }

                _database.Insert(element);
                return true;
            }
        }

        private bool InsertOrReplaceStringIdEntity<T>(T element, List<T> elements) where T : IStringIdEntity
        {
            lock (_locker)
            {
                bool exists = elements.Any(x => x.DatabaseID.Equals(element.DatabaseID));
                if (exists)
                {
                    _database.Update(element);
                    return false;
                }

                _database.Insert(element);
                return true;
            }
        }
    }
}
