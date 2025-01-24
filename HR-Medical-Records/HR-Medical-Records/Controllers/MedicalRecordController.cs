using HR_Medical_Records.DTOs.MedicalRecordDTOs;
using HR_Medical_Records.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace HR_Medical_Records.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicalRecordController : Controller
    {
        private readonly IMedicalRecordService _medicalRecordService;
        public MedicalRecordController(IMedicalRecordService medicalRecordService)
        {
            _medicalRecordService = medicalRecordService;
        }

        //[HttpGet("filter")]
        //public async Task<IActionResult> GetFilterMedicalRecords()
        //{

        //}

        [HttpGet("{medicalRecordId}")]
        public async Task<IActionResult> GetMedicalRecordById([FromRoute] int medicalRecordId)
        {
            var response = await _medicalRecordService.GetMedicalRecordById(medicalRecordId);
            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> AddMedicalRecord([FromBody] CreateMedicalRecord request,
                                                          [FromHeader(Name = "x-user-id")] Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest("The 'x-user-id' header is required");
            }

            var response = await _medicalRecordService.AddMedicalRecord(request, userId);
            return Ok(response);
        }


        //[HttpPut("update")]
        //public async Task<IActionResult> UpdateMedicalRecord([FromBody] UpdateMedicalRecord request)
        //{

        //}

        //[HttpPatch("softDelete/{medicalRecordId}")]
        //public async Task<IActionResult> DeleteMedicalRecord([FromRoute] int medicalRecordId)
        //{

        //}
    }
}
