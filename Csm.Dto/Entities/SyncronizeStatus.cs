using System;
using System.Collections.Generic;
using System.Text;

namespace Csm.Dto.Entities
{
    public class SyncronizeStatus
    {
        private readonly Status syncStatus;
        public string Message { get; set; }

        public SyncronizeStatus(Status status)
        {
            this.syncStatus = status;
        }

        public Status getstatus => this.syncStatus;

    }
    public enum Status
    {
        Failed,
        Success,
        Error
    }
}
