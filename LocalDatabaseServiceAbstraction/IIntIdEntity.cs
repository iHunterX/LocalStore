using System;
using System.Collections.Generic;
using System.Text;

namespace LocalDatabaseServiceAbstraction
{
    public interface IIntIdEntity
    {
        int DatabaseID { get; set; }
    }
}
