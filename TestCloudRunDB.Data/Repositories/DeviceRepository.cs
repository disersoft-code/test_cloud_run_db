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
            var cnn = NewMysqlUnixSocketConnectionString();
            _log.LogDebug("dbConnection:{dbConnection}", _mySQLConfiguration.ConnectionString);
            _log.LogDebug("dbConnection:{dbConnection}", cnn);
            //return new MySqlConnection(_mySQLConfiguration.ConnectionString);
            return new MySqlConnection(cnn);
        }

        protected string NewMysqlUnixSocketConnectionString()
        {
            // Equivalent connection string:
            // "Server=<INSTANCE_UNIX_SOCKET>;Uid=<DB_USER>;Pwd=<DB_PASS>;Database=<DB_NAME>;Protocol=unix"
            var connectionString = new MySqlConnectionStringBuilder()
            {
                // The Cloud SQL proxy provides encryption between the proxy and instance.
                SslMode = MySqlSslMode.Disabled,
                

                // Note: Saving credentials in environment variables is convenient, but not
                // secure - consider a more secure solution such as
                // Cloud Secret Manager (https://cloud.google.com/secret-manager) to help
                // keep secrets safe.
                Server = "/cloudsql/testcloudrun-372814:us-central1:disersoft", // e.g. '/cloudsql/project:region:instance'
                UserID = "root",   // e.g. 'my-db-user
                Password = "Disersoft1*2040", // e.g. 'my-db-password'
                Database = "dsfcontrol", // e.g. 'my-database'
                ConnectionProtocol = MySqlConnectionProtocol.UnixSocket
            };
            connectionString.Pooling = true;
            
            // Specify additional properties here.
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
