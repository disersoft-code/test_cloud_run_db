using TestCloudRunDB.Model;

namespace TestCloudRunDB.Data.Repositories
{
    public interface IDeviceRepository
    {
        Task<IEnumerable<Device>> GetAll();
        Task<Device> GetById(int id);
        Task<bool> Insert(Device device);
        Task<bool> Update(Device device);
        Task<bool> Delete(int id);

    }
}
