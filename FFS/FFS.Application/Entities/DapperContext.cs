using System.Data;

using Microsoft.Data.SqlClient;

namespace FFS.Application.Entities {
    public class DapperContext {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;
        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public IDbConnection connection => new SqlConnection(connectionString);

    }
}
