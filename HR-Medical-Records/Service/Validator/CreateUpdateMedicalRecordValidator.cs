using FluentValidation;
using HR_Medical_Records.Data;
using HR_Medical_Records.DTOs.MedicalRecordDTOs;
using Microsoft.EntityFrameworkCore;

namespace HR_Medical_Records.Service.Validator
{
    /// <summary>
    /// Validator class for validating Create and Update requests for medical records.
    /// This class includes various rules for date validation, mandatory fields, related record checks, 
    /// maximum length validation, and specific field constraints.
    /// </summary>
    public class CreateUpdateMedicalRecordValidator : AbstractValidator<CreateAndUpdateMedicalRecord>
    {
        private readonly HRContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateUpdateMedicalRecordValidator"/> class.
        /// </summary>
        /// <param name="context">The database context used to perform async validation checks.</param>
        public CreateUpdateMedicalRecordValidator(HRContext context)
        {
            _context = context;

            // 2.1. Date validations
            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("START_DATE is required")
                .Must(date => date == DateOnly.FromDateTime(DateTime.Now))
                .When(x => !x.MedicalRecordId.HasValue)
                .WithMessage("START_DATE must be the current date");

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate)
                .When(x => x.EndDate.HasValue)
                .WithMessage("END_DATE must be later than START_DATE");

            // 2.2. Mandatory fields
            RuleFor(x => x.FileId)
                .NotEmpty().WithMessage("FILE_ID is required");

            RuleFor(x => x.FileId)
                .MustAsync((fileId, cancellationToken) => ExistInFileId(fileId, cancellationToken))
                .When(x => !x.MedicalRecordId.HasValue)
                .WithMessage("FILE_ID already register");

            RuleFor(x => x.MedicalRecordId)
                .MustAsync((medicalRecordId, cancellationToken) => ExistInTMedicalRecord(medicalRecordId, cancellationToken))
                .When(x => x.MedicalRecordId.HasValue)
                .WithMessage("MEDICAL_RECORD_ID not found");

            RuleFor(x => x.Diagnosis)
                .NotEmpty().WithMessage("DIAGNOSIS is required")
                .MaximumLength(100).WithMessage("DIAGNOSIS cannot exceed 100 characters");

            RuleFor(x => x.StatusId)
                .NotEmpty().WithMessage("STATUS_ID is required");

            RuleFor(x => x.MedicalRecordTypeId)
                .NotEmpty().WithMessage("MEDICAL_RECORD_TYPE_ID is required");

            // 2.3. Related records validation
            RuleFor(x => x.StatusId)
                .MustAsync((statusId, cancellationToken) => ExistInStatusTable(statusId, cancellationToken))
                .WithMessage("STATUS_ID must exist in the Status table");

            RuleFor(x => x.StatusId)
                .Must(statusId => statusId != 2)
                .WithMessage("Cannot assign status 'Inactive' (StatusId = 2) when creating or modifying a record.");


            RuleFor(x => x.MedicalRecordTypeId)
                .MustAsync((typeId, cancellationToken) => ExistInMedicalRecordTypeTable(typeId, cancellationToken))
                .WithMessage("MEDICAL_RECORD_TYPE_ID must exist in the MedicalRecordType table");

            // 2.4. Maximum length validation
            RuleFor(x => x.MotherData)
                .MaximumLength(2000).WithMessage("MOTHER_DATA cannot exceed 2000 characters");

            RuleFor(x => x.FatherData)
                .MaximumLength(2000).WithMessage("FATHER_DATA cannot exceed 2000 characters");

            RuleFor(x => x.OtherFamilyData)
                .MaximumLength(2000).WithMessage("OTHER_FAMILY_DATA cannot exceed 2000 characters");

            RuleFor(x => x.MedicalBoard)
                .MaximumLength(200).WithMessage("MEDICAL_BOARD cannot exceed 200 characters");

            RuleFor(x => x.Observations)
                .MaximumLength(2000).WithMessage("OBSERVATIONS cannot exceed 2000 characters");

            // 2.5. Validation of 'YES' or 'NO' fields
            RuleFor(x => x.Audiometry)
                .Must(BeYesOrNo).WithMessage("AUDIOMETRY must be 'Y' or 'N'");

            RuleFor(x => x.PositionChange)
                .Must(BeYesOrNo).WithMessage("POSITION_CHANGE must be 'Y' or 'N'");

            RuleFor(x => x.Observations)
                .NotEmpty()
                .When(x => x.PositionChange?.ToUpperInvariant() == "Y")
                .WithMessage("OBSERVATIONS is required when POSITION_CHANGE is 'Y'");

            RuleFor(x => x.ExecuteMicros)
                .Must(BeYesOrNo).WithMessage("EXECUTE_MICROS must be 'Y' or 'N'");

            RuleFor(x => x.ExecuteExtra)
                .Must(BeYesOrNo).WithMessage("EXECUTE_EXTRA must be 'Y' or 'N'");

            RuleFor(x => x.VoiceEvaluation)
                .Must(BeYesOrNo).WithMessage("VOICE_EVALUATION must be 'Y' or 'N'");

            RuleFor(x => x.Disability)
                .Must(BeYesOrNo).WithMessage("DISABILITY must be 'Y' or 'N'");

            RuleFor(x => x.DisabilityPercentage)
                .InclusiveBetween(0, 100)
                .When(x => x.Disability?.ToUpperInvariant() == "Y")
                .WithMessage("DISABILITY_PERCENTAGE must be between 0 and 100 when DISABILITY is 'Y'");

            RuleFor(x => x.AreaChange)
                .Must(BeYesOrNo).WithMessage("AREA_CHANGE must be 'Y' or 'N'");
        }

        /// <summary>
        /// Checks if the medical record ID exists in the TMedicalRecords table.
        /// </summary>
        /// <param name="medicalRecordId">The medical record ID to check.</param>
        /// <param name="cancellationToken">The cancellation token for async operations.</param>
        /// <returns>True if the record exists, otherwise false.</returns>
        private async Task<bool> ExistInTMedicalRecord(int? medicalRecordId, CancellationToken cancellationToken)
        {
            return await _context.TMedicalRecords
                                .AnyAsync(s => s.MedicalRecordId == medicalRecordId, cancellationToken);
        }

        /// <summary>
        /// Checks if the File ID exists in the TMedicalRecords table.
        /// </summary>
        /// <param name="fileId">The file ID to check.</param>
        /// <param name="cancellationToken">The cancellation token for async operations.</param>
        /// <returns>True if the file ID does not exist, otherwise false.</returns>
        private async Task<bool> ExistInFileId(int fileId, CancellationToken cancellationToken)
        {
            return !await _context.TMedicalRecords
                                .AnyAsync(s => s.FileId == fileId, cancellationToken);
        }

        /// <summary>
        /// Checks if the status ID exists in the Status table.
        /// </summary>
        /// <param name="statusId">The status ID to check.</param>
        /// <param name="cancellationToken">The cancellation token for async operations.</param>
        /// <returns>True if the status ID exists, otherwise false.</returns>
        private async Task<bool> ExistInStatusTable(int statusId, CancellationToken cancellationToken)
        {
            return await _context.Statuses
                                .AnyAsync(s => s.StatusId == statusId, cancellationToken);
        }

        /// <summary>
        /// Checks if the medical record type ID exists in the MedicalRecordType table.
        /// </summary>
        /// <param name="typeId">The type ID to check.</param>
        /// <param name="cancellationToken">The cancellation token for async operations.</param>
        /// <returns>True if the medical record type ID exists, otherwise false.</returns>
        private async Task<bool> ExistInMedicalRecordTypeTable(int typeId, CancellationToken cancellationToken)
        {
            return await _context.MedicalRecordTypes
                                .AnyAsync(m => m.MedicalRecordTypeId == typeId, cancellationToken);
        }

        /// <summary>
        /// Validates if a value is 'Y' or 'N' (Yes or No).
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>True if the value is 'Y' or 'N', otherwise false.</returns>
        private bool BeYesOrNo(string? value)
        {
            return !string.IsNullOrEmpty(value) && (value.ToUpperInvariant() == "Y" || value.ToUpperInvariant() == "N");
        }
    }
}
