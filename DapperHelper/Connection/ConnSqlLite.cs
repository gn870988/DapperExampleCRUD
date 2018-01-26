using System.Data.Common;
using System.Data.SQLite;

namespace DapperHelper
{
    public class ConnSqlLite : ConnectionAction
    {
        private string Connstring;
        public ConnSqlLite(string Connstring)
        {
            this.Connstring = Connstring;
        }

        public override DbConnection CreatConn()
        {
            return new SQLiteConnection(Connstring);
        }
    }
}
