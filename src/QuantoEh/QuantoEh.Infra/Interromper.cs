using System;
using System.Data.Services.Common;
using Microsoft.WindowsAzure.StorageClient;

namespace QuantoEh.Infra
{
    [DataServiceEntity]
    public class Interromper : TableServiceEntity
    {
        public Interromper() : base("IR", "0") { }
        
        public bool GetInterrompe()
        {
            return Convert.ToBoolean(RowKey);
        }
        public void SetInterrompe(bool value)
        {
            RowKey = value.ToString();
        }
    }
}