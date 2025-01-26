using FluentValidation;
using HR_Medical_Records.DTOs.MedicalRecordDTOs;
using HR_Medical_Records.Repository;

namespace HR_Medical_Records.Service.Validator
{
    /// <summary>
    /// Validator class for validating the soft deletion of a medical record.
    /// This class ensures that the necessary conditions are met before deleting a medical record, 
    /// including verifying that the medical record exists, the deletion reason is provided, 
    /// and the record is active and has a valid start date.
    /// </summary>
    public class SoftDeleteMedicalRecordValidator : AbstractValidator<SoftDeleteMedicalRecord>
    {
        private readonly IMedicalRecordRepository _medicalRecordRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="SoftDeleteMedicalRecordValidator"/> class.
        /// </summary>
        /// <param name="medicalRecordRepository">The repository for accessing medical records.</param>
        public SoftDeleteMedicalRecordValidator(IMedicalRecordRepository medicalRecordRepository)
        {
            _medicalRecordRepository = medicalRecordRepository;

            RuleFor(x => x.MedicalRecordId)
                .NotEmpty().WithMessage("MedicalRecordId is required")
                .MustAsync(async (id, cancellationToken) => await MedicalRecordExists(id))
                .WithMessage("Medical Record not found with Id");

            RuleFor(x => x.DeletionReason)
                .NotEmpty().WithMessage("The 'DeletionReason' field is required")
                .MaximumLength(2000).WithMessage("OBSERVATIONS cannot exceed 2000 characters");

            RuleFor(x => x)
                .MustAsync(async (request, cancellationToken) => await MedicalRecordIsActiveAndValid(request))
                .WithMessage("The Medical Record must be active to delete and have a valid StartDate");
        }

        /// <summary>
        /// Checks if a medical record exists by its ID.
        /// </summary>
        /// <param name="medicalRecordId">The ID of the medical record.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean value indicating if the record exists.</returns>
        private async Task<bool> MedicalRecordExists(int medicalRecordId)
        {
            var medicalRecord = await _medicalRecordRepository.GetById(medicalRecordId);
            return medicalRecord != null;
        }

        /// <summary>
        /// Checks if the medical record is active and if the start date is valid for deletion.
        /// </summary>
        /// <param name="request">The soft delete request containing the medical record ID.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean value indicating if the record is active and valid for deletion.</returns>
        private async Task<bool> MedicalRecordIsActiveAndValid(SoftDeleteMedicalRecord request)
        {
            var medicalRecord = await _medicalRecordRepository.GetById(request.MedicalRecordId);
            if (medicalRecord == null)
            {
                return false;
            }

            bool isActive = medicalRecord.StatusId != 2;// Inactive record

            bool isStartDateValid = medicalRecord.StartDate <= DateOnly.FromDateTime(DateTime.UtcNow);

            return isActive && isStartDateValid;
        }
    }
}
