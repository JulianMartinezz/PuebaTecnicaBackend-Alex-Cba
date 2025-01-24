﻿using FluentValidation;
using HR_Medical_Records.Data;
using HR_Medical_Records.DTOs.MedicalRecordDTOs;
using Microsoft.EntityFrameworkCore;

namespace HR_Medical_Records.Service.Validator
{
    public class CreateMedicalRecordValidator : AbstractValidator<CreateAndUpdateMedicalRecord>
    {
        private readonly HRContext _context;

        public CreateMedicalRecordValidator(HRContext context)
        {
            // 2.1. Date validations
            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("START_DATE is required")
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now)).WithMessage("START_DATE cannot be a future date");

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate)
                .When(x => x.EndDate.HasValue)
                .WithMessage("END_DATE must be later than START_DATE");

            RuleFor(x => x.CreationDate)
                .NotEmpty().WithMessage("CREATION_DATE is required")
                .Must(BeAValidCreationDate).WithMessage("CREATION_DATE must be automatically generated");

            // 2.2. Mandatory fields
            RuleFor(x => x.FileId)
                .NotEmpty().WithMessage("FILE_ID is required");

            RuleFor(x => x.FileId)
                .MustAsync((fileId, cancellationToken) => ExistInTMedicalRecord(fileId, cancellationToken))
                .WithMessage("FILE_ID already register");

            RuleFor(x => x.Diagnosis)
                .NotEmpty().WithMessage("DIAGNOSIS is required")
                .MaximumLength(100).WithMessage("DIAGNOSIS cannot exceed 100 characters");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("START_DATE is required");

            RuleFor(x => x.StatusId)
                .NotEmpty().WithMessage("STATUS_ID is required");

            RuleFor(x => x.MedicalRecordTypeId)
                .NotEmpty().WithMessage("MEDICAL_RECORD_TYPE_ID is required");

            // 2.3. Related records validation
            RuleFor(x => x.StatusId)
                .MustAsync((statusId, cancellationToken) => ExistInStatusTable(statusId, cancellationToken))
                .WithMessage("STATUS_ID must exist in the Status table");

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
                .Must(BeYesOrNo).WithMessage("AUDIOMETRY must be 'YES' or 'NO'");

            RuleFor(x => x.PositionChange)
                .Must(BeYesOrNo).WithMessage("POSITION_CHANGE must be 'YES' or 'NO'");

            RuleFor(x => x.ExecuteMicros)
                .Must(BeYesOrNo).WithMessage("EXECUTE_MICROS must be 'YES' or 'NO'");

            RuleFor(x => x.ExecuteExtra)
                .Must(BeYesOrNo).WithMessage("EXECUTE_EXTRA must be 'YES' or 'NO'");

            RuleFor(x => x.VoiceEvaluation)
                .Must(BeYesOrNo).WithMessage("VOICE_EVALUATION must be 'YES' or 'NO'");

            RuleFor(x => x.Disability)
                .Must(BeYesOrNo).WithMessage("DISABILITY must be 'YES' or 'NO'");

            RuleFor(x => x.AreaChange)
                .Must(BeYesOrNo).WithMessage("AREA_CHANGE must be 'YES' or 'NO'");
        }

        private bool BeAValidCreationDate(DateOnly? creationDate)
        {
            return creationDate.HasValue && creationDate.Value <= DateOnly.FromDateTime(DateTime.Now);
        }

        private async Task<bool> ExistInTMedicalRecord(int fileId, CancellationToken cancellationToken)
        {
            return !await _context.TMedicalRecords
                                .AnyAsync(s => s.FileId == fileId, cancellationToken);
        }

        private async Task<bool> ExistInStatusTable(int statusId, CancellationToken cancellationToken)
        {
            return await _context.Statuses
                                .AnyAsync(s => s.StatusId == statusId, cancellationToken);
        }

        private async Task<bool> ExistInMedicalRecordTypeTable(int typeId, CancellationToken cancellationToken)
        {
            return await _context.MedicalRecordTypes
                                .AnyAsync(m => m.MedicalRecordTypeId == typeId, cancellationToken);
        }

        private bool BeYesOrNo(string? value)
        {
            return !string.IsNullOrEmpty(value) && (value.ToUpperInvariant() == "YES" || value.ToUpperInvariant() == "NO");
        }
    }
}
