using System.Data;

using Microsoft.Data.SqlClient;

namespace FFS.Application.Entities {
    public class DapperContext : IDapperContext {
        public IDbConnection Connection { get; }

        public DapperContext(string connectionString)
        {
            Connection = new SqlConnection(connectionString);
        }

    }

    public interface IDapperContext {
        IDbConnection Connection { get; }
    }
}
