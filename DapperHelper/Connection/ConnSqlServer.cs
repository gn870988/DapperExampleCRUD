using System.Data.Common;
using System.Data.SqlClient;

namespace DapperHelper
{
    public class ConnSqlServer : ConnectionAction
    {
        private string Connstring;
        public ConnSqlServer(string Connstring)
        {
            this.Connstring = Connstring;
        }

        public override DbConnection CreatConn()
        {
            return new SqlConnection(Connstring);
        }
    }
}
