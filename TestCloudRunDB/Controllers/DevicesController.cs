using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using TestCloudRunDB.Data.Repositories;
using TestCloudRunDB.Extensions;
using TestCloudRunDB.Model;

namespace TestCloudRunDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly IDistributedCache _cache;
        private readonly ILogger<DevicesController> _log;

        public DevicesController(IDeviceRepository deviceRepository, IDistributedCache cache, ILogger<DevicesController> log)
        {
            _deviceRepository = deviceRepository;
            _cache = cache;
            _log = log;
        }

        // GET: api/<DevicesController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var cacheKey = "listDevices";
            var listDevices = await _cache.GetRecordAsync<List<Device>>(cacheKey);

            if (listDevices != null)
            {
                _log.LogDebug("********   data from cache.....");
            }
            else
            {
                _log.LogDebug("********   data from db.....");

                var aux = await _deviceRepository.GetAll();
                listDevices = aux.ToList();
                await _cache.SetRecordAsync(cacheKey, listDevices, TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(2));
            }


            return Ok(listDevices);

        }

        // GET api/<DevicesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _deviceRepository.GetById(id));
        }

        // POST api/<DevicesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Device device)
        {
            if (device == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _deviceRepository.Insert(device);

            return Created("created", result);

        }

        // POST api/<DevicesController>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Device device)
        {
            if (device == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _deviceRepository.Update(device);

            return NoContent();

        }

        // DELETE api/<DevicesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _deviceRepository.Delete(id);
            return NoContent();
        }
    }
}
