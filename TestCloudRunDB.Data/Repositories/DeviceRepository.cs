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
