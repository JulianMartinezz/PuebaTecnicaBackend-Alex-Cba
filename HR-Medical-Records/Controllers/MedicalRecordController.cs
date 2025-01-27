using HR_Medical_Records.DTOs.MedicalRecordDTOs;
using HR_Medical_Records.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace HR_Medical_Records.Controllers
{
    /// <summary>
    /// Controller responsible for managing medical record-related operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class MedicalRecordController : Controller
    {
        private readonly IMedicalRecordService _medicalRecordService;
        public MedicalRecordController(IMedicalRecordService medicalRecordService)
        {
            _medicalRecordService = medicalRecordService;
        }

        /// <summary>
        /// Retrieves a list of medical records based on the provided filters.
        /// </summary>
        /// <param name="request">The filter parameters for retrieving medical records.</param>
        /// <returns>A list of medical records matching the filters.</returns>
        [HttpGet("filter")]
        public async Task<IActionResult> GetFilterMedicalRecords([FromQuery] MedicalRecordFilterRequest request)
        {
            var response = await _medicalRecordService.GetFilterMedicalRecords(request);
            return Ok(response);
        }

        /// <summary>
        /// Retrieves the details of a specific medical record by its ID.
        /// </summary>
        /// <param name="medicalRecordId">The ID of the medical record to retrieve.</param>
        /// <returns>The detailed information of the specified medical record.</returns>
        [HttpGet("{medicalRecordId}")]
        public async Task<IActionResult> GetMedicalRecordById([FromRoute] int medicalRecordId)
        {
            var response = await _medicalRecordService.GetMedicalRecordById(medicalRecordId);
            return Ok(response);
        }

        /// <summary>
        /// Creates a new medical record.
        /// </summary>
        /// <param name="request">The details of the medical record to create.</param>
        /// <param name="userId">The ID of the user performing the operation, passed in the "x-user-id" header.</param>
        /// <returns>A confirmation of the record creation.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> AddMedicalRecord([FromBody] CreateAndUpdateMedicalRecord request,
                                                          [FromHeader(Name = "x-user-id")] Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest("The 'x-user-id' header is required");
            }

            var response = await _medicalRecordService.AddUpdateMedicalRecord(request, userId);
            return Ok(response);
        }

        /// <summary>
        /// Updates an existing medical record.
        /// </summary>
        /// <param name="request">The updated details of the medical record.</param>
        /// <param name="userId">The ID of the user performing the operation, passed in the "x-user-id" header.</param>
        /// <returns>A confirmation of the record update.</returns>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateMedicalRecord([FromBody] CreateAndUpdateMedicalRecord request,
                                                             [FromHeader(Name = "x-user-id")] Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest("The 'x-user-id' header is required");
            }

            var response = await _medicalRecordService.AddUpdateMedicalRecord(request, userId);
            return Ok(response);
        }

        /// <summary>
        /// Performs a logical deletion of a medical record.
        /// </summary>
        /// <param name="request">The details required for the soft deletion of the medical record.</param>
        /// <param name="userId">The ID of the user performing the operation, passed in the "x-user-id" header.</param>
        /// <returns>A confirmation of the record deletion.</returns>
        [HttpPatch("softDelete")]
        public async Task<IActionResult> DeleteMedicalRecord([FromBody] SoftDeleteMedicalRecord request,
                                                             [FromHeader(Name = "x-user-id")] Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest("The 'x-user-id' header is required");
            }

            var response = await _medicalRecordService.DeleteMedicalRecord(request, userId);
            return Ok(response);
        }
    }
}
