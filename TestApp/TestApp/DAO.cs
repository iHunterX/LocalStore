using LocalDatabaseService.LocalStorage;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestApp
{
    public class DataManager
    {
        // Properties
        public static DataManager Instance => _instance ?? (_instance = new DataManager());

        // Resources
        private static DataManager _instance;
        private readonly object _locker;

        // Constructor
        public DataManager()
        {
            _locker = new object();
            lock (_locker)
            {
                CreateDatabase();
            }
        }

        public void SaveTicket(TicketDatabase ticketDatabase)
        {
            lock (_locker)
            {
                DatabaseManager.Instance.SaveIntIdEntity(ticketDatabase);
            }
        }

        public TicketDatabase GetFirstTicket()
        {
            lock (_locker)
            {
                TableQuery<TicketDatabase> ticketsDatabase = DatabaseManager.Instance.GetTable<TicketDatabase>();
                return ticketsDatabase.FirstOrDefault();
            }
        }

        public IEnumerable<TicketDatabase> GetAllTicket()
        {
            lock (_locker)
            {
                TableQuery<TicketDatabase> ticketsDatabase = DatabaseManager.Instance.GetTable<TicketDatabase>();
                return ticketsDatabase.AsEnumerable();
            }
        }

        public void DeleteTicket(int ticketId)
        {
            lock (_locker)
            {
                TableQuery<TicketDatabase> ticketsDatabase = DatabaseManager.Instance.GetTable<TicketDatabase>();
                var ticketDatabase = ticketsDatabase
                    .AsEnumerable()
                    .FirstOrDefault(x => x.DatabaseID == ticketId);

                DatabaseManager.Instance.Delete(ticketDatabase);
            }
        }

        public void EraseStorage()
        {
            lock (_locker)
            {
                DatabaseManager.Instance.DeleteDatabase();
                CreateDatabase();
            }
        }

        // Private
        private void CreateDatabase()
        {
            try
            {
                DatabaseManager.Instance.CreateDatabase();
                DatabaseManager.Instance.CreateTable<TicketDatabase>();
            }
            catch (Exception e)
            {
                EraseStorage();
            }
        }
    }
}
