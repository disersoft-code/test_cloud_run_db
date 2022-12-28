using Dapper;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using TestCloudRunDB.Model;

namespace TestCloudRunDB.Data.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly MySQLConfiguration _mySQLConfiguration;
        private readonly ILogger<DeviceRepository> _log;

        public DeviceRepository(MySQLConfiguration mySQLConfiguration, ILogger<DeviceRepository> log)
        {
            _mySQLConfiguration = mySQLConfiguration;
            _log = log;
        }

        protected MySqlConnection dbConnection()
        {
            var cnn = NewMysqlUnixSocketConnectionString_New();
            _log.LogDebug("dbConnection:{dbConnection}", _mySQLConfiguration.ConnectionString);
            _log.LogDebug("dbConnection:{dbConnection}", cnn);
            //return new MySqlConnection(_mySQLConfiguration.ConnectionString);
            return new MySqlConnection(cnn);
        }

        protected string NewMysqlUnixSocketConnectionString()
        {
            var connectionString = new MySqlConnectionStringBuilder()
            {
                SslMode = MySqlSslMode.Disabled,
                Server = "/cloudsql/testcloudrun-372814:us-central1:disersoft", // e.g. '/cloudsql/project:region:instance'
                UserID = "testuser",   // e.g. 'my-db-user
                Password = "Disersoft1*2040", // e.g. 'my-db-password'
                Database = "dsfcontrol", // e.g. 'my-database'
                ConnectionProtocol = MySqlConnectionProtocol.UnixSocket
            };
            connectionString.Pooling = true;
            
            return connectionString.ConnectionString;
        }

        protected string NewMysqlUnixSocketConnectionString_New()
        {
            var connectionString = new MySqlConnectionStringBuilder()
            {
                SslMode = MySqlSslMode.Disabled,
                Server = "/cloudsql/testcloudrun-372814:us-central1:quickstart-cloud-run-mysql-instance", // e.g. '/cloudsql/project:region:instance'
                UserID = "quickstart-mysql-user",   // e.g. 'my-db-user
                Password = "password", // e.g. 'my-db-password'
                Database = "quickstart-db", // e.g. 'my-database'
                ConnectionProtocol = MySqlConnectionProtocol.UnixSocket
            };
            connectionString.Pooling = true;

            return connectionString.ConnectionString;
        }


        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Device>> GetAll()
        {
            var db = dbConnection();
            var sql = "Select DeviceId, DevicesUID, DeviceName, DeviceIP from Devices limit 10";
            return db.QueryAsync<Device>(sql);


        }

        public Task<Device> GetById(int id)
        {
            var db = dbConnection();
            var sql = "Select DeviceId, DevicesUID, DeviceName, DeviceIP from Devices Where DeviceId=@id ";
            return db.QueryFirstOrDefaultAsync<Device>(sql, new { id });
        }

        public Task<bool> Insert(Device device)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Device device)
        {
            throw new NotImplementedException();
        }
    }
}
