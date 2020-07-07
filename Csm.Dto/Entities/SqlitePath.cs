using System;
using System.Collections.Generic;
using System.Text;

namespace Csm.Dto.Entities
{
    public class SqlitePath : ISqlitePath
    {
        public string path { get; private set; }
        public string setPath
        {
            set
            {
                this.path = value;
            }
        }
    }
}
