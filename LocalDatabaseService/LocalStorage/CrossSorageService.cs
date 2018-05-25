using LocalDatabaseServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocalDatabaseService.LocalStorage
{
    public class CrossStorageService
    {
        static readonly Lazy<IStorageService> storageService = new Lazy<IStorageService>(CreateStorageService, System.Threading.LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Current settings to use 
        /// </summary>
        public static IStorageService Current
        {
            get
            {
                IStorageService ret = storageService.Value;
                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }
                return ret;
            }
        }

        static IStorageService CreateStorageService()
        {
#if NETSTANDARD2_0
            return null;
#else
            return new StorageService();
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly()
        {
            return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the Amaris.Mobile.Services.Storage NuGet package from your main application project in order to reference the platform-specific implementation.");
        }
    }
}
