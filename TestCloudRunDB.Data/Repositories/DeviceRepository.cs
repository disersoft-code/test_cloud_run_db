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
            
            _log.LogDebug("dbConnection:{dbConnection}", _mySQLConfiguration.ConnectionString);
            return new MySqlConnection(_mySQLConfiguration.ConnectionString);

            //var cnn = NewMysqlUnixSocketConnectionString();
            //_log.LogDebug("dbConnection:{dbConnection}", cnn);
            //return new MySqlConnection(cnn);
        }

        //protected string NewMysqlUnixSocketConnectionString()
        //{
        //    var connectionString = new MySqlConnectionStringBuilder()
        //    {
        //        SslMode = MySqlSslMode.None,
        //        Server = "/cloudsql/testcloudrun-372814:us-central1:disersoft", // e.g. '/cloudsql/project:region:instance'
        //        UserID = "testuser",   // e.g. 'my-db-user
        //        Password = "Disersoft1*2040", // e.g. 'my-db-password'
        //        Database = "dsfcontrol", // e.g. 'my-database'
        //        ConnectionProtocol = MySqlConnectionProtocol.UnixSocket,
        //        Pooling = true,
        //        ConnectionTimeout = 10,
        //        ConnectionLifeTime = 1,
        //        MaximumPoolSize = 1
        //    };
            
            
        //    return connectionString.ConnectionString;
        //}

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Device>> GetAll()
        {
            try
            {
                var db = dbConnection();
                var sql = "Select DeviceId, DevicesUID, DeviceName, DeviceIP from Devices limit 10".ToLower();
                _log.LogDebug("sql:{sql}", sql);
                return db.QueryAsync<Device>(sql);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error exception");
                throw;
            }
        }


        public Task<Device> GetById(int id)
        {
            var db = dbConnection();
            var sql = "Select DeviceId, DevicesUID, DeviceName, DeviceIP from Devices Where DeviceId=@id ".ToLower();
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
