using System.Data.Common;

namespace DapperHelper
{
    public abstract class ConnectionAction
    {
        public abstract DbConnection CreatConn();
    }
}
