using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CSM.Dal.Repositories
{
    internal class RepositoryBase
    {
        protected IDbTransaction Transaction { get; private set; }
        protected IDbConnection Connection{ get { return Transaction.Connection; } }

        public RepositoryBase(IDbTransaction transaction)
        {
            Transaction = transaction;
        }
    }
}
