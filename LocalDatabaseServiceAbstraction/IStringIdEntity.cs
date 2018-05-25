using System;
using System.Collections.Generic;
using System.Text;

namespace LocalDatabaseServiceAbstraction
{
    public interface IStringIdEntity
    {
        string DatabaseID { get; set; }
    }
}
