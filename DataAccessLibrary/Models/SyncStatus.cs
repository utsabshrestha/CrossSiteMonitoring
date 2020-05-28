using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models
{
    public class SyncStatus
    {
        public enum stat
        {
            Success,
            Failed,
            Error
        }

        private stat Status { get; set; }
        public string Message { get; set; }

        public void setStatus(stat s)
        {
            this.Status = s;
        }

        public stat getStatus()
        {
            return this.Status;
        }

    }
}
