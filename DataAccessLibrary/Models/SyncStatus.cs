using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models
{
    public class SyncStatus
    {
        public enum stat
        {
            Failed,
            Success,
            Error
        }

        private stat Status { get; set; }
        public string Message { get; set; }

        public stat getstatus => this.Status;

        public void setStatus(stat s)
        {
            this.Status = s;
        }
    }
}
