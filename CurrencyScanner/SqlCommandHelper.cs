using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyScanner
{
    public abstract class SqlCommandHelper
    {
        ILogger logger;
        string sqlExpr;
        protected SqlCommand command;

        protected abstract object Execute();

        public SqlCommandHelper(ILogger logger, SqlConnection connection, string sqlExpr)
        {
            this.logger = logger;
            this.command = new SqlCommand(sqlExpr, connection);
            this.sqlExpr = sqlExpr;
        }

        protected void Log(string msg, bool error = false)
        {
            logger.Log(msg, error);
        }

        public object TryExecute()
        {
            try
            {                                
                object res = Execute();
                Log($"SUCCESS: {sqlExpr}");
                return res;
            }
            catch (Exception ex)
            {
                Log($"FAIL: {sqlExpr}: {ex.Message}", true);
                return null;
            }
        }
    }

    public class SqlCommandNonqueryHelper : SqlCommandHelper
    {
        public SqlCommandNonqueryHelper(ILogger logger, SqlConnection connection, string sqlExpr) : base(logger, connection, sqlExpr) {}

        protected override object Execute()
        {
            int number = command.ExecuteNonQuery();
            Log($"Добавлено объектов: {number}");
            return number;
        }
    }

    public class SqlCommandReaderHelper : SqlCommandHelper
    {
        public SqlCommandReaderHelper(ILogger logger, SqlConnection connection, string sqlExpr) : base(logger, connection, sqlExpr) {}

        protected override object Execute()
        {
            return command.ExecuteReader();
        }
    }
}
