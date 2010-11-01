using System;
using System.Data.Services.Common;
using Microsoft.WindowsAzure.StorageClient;

namespace QuantoEh.Infra.EntidadesAzure
{
    [DataServiceEntity]
    internal class UltimoId : TableServiceEntity
    {
        public UltimoId() : base("UID", "0") { }

        public ulong Id
        {
            get { return Convert.ToUInt64(RowKey); }
            set { RowKey = value.ToString(); }
        }
    }
}