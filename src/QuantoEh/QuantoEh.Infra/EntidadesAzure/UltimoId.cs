using System;
using System.Data.Services.Common;
using Microsoft.WindowsAzure.StorageClient;

namespace QuantoEh.Infra.EntidadesAzure
{
    [DataServiceEntity]
    internal class UltimoId : TableServiceEntity
    {
        public UltimoId() : base("UID", "0") { }

        public void SetId(ulong value)
        {
            RowKey = value.ToString();
        }
        public ulong GetId()
        {
            return Convert.ToUInt64(RowKey);
        }
    }
}