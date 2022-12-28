using Microsoft.AspNetCore.Mvc;
using TestCloudRunDB.Data.Repositories;
using TestCloudRunDB.Model;

namespace TestCloudRunDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly IDeviceRepository _deviceRepository;

        public DevicesController(IDeviceRepository deviceRepository)
        {
            _deviceRepository = deviceRepository;
        }

        // GET: api/<DevicesController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _deviceRepository.GetAll());
            
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
            if (device == null )
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
