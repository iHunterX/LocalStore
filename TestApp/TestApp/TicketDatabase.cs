using LocalDatabaseServiceAbstraction;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp
{
    [Table("Ticket")]
    public class TicketDatabase : IIntIdEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        public int DatabaseID { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public byte[] LogsFile { get; set; }

    }
}
