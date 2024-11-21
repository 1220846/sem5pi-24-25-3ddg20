using System;
using System.Threading.Tasks;
using dddnetcore.Domain.Hospital;
using Microsoft.AspNetCore.Mvc;

namespace dddnetcore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HospitalsController : ControllerBase
    {
        private readonly HospitalService _hospitalService;

        public HospitalsController(HospitalService hospitalService) {
            this._hospitalService = hospitalService;
        }

        //? GET api/hospitals
        [HttpGet]
        public async Task<IActionResult> GetHospital() {
            var hospital = await _hospitalService.GetHospital();
            return Ok(hospital);
        }
    }
}