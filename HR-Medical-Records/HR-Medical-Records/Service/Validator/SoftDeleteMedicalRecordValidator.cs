using FluentValidation;
using HR_Medical_Records.DTOs.MedicalRecordDTOs;
using HR_Medical_Records.Repository;

namespace HR_Medical_Records.Service.Validator
{
    public class SoftDeleteMedicalRecordValidator : AbstractValidator<SoftDeleteMedicalRecord>
    {
        private readonly IMedicalRecordRepository _medicalRecordRepository;

        public SoftDeleteMedicalRecordValidator(IMedicalRecordRepository medicalRecordRepository)
        {
            _medicalRecordRepository = medicalRecordRepository;

            RuleFor(x => x.MedicalRecordId)
                .NotEmpty().WithMessage("MedicalRecordId is required")
                .MustAsync(async (id, cancellationToken) => await MedicalRecordExists(id))
                .WithMessage("Medical Record not found with Id");

            RuleFor(x => x.DeletionReason)
                .NotEmpty().WithMessage("The 'DeletionReason' field is required");

            RuleFor(x => x)
                .MustAsync(async (request, cancellationToken) => await MedicalRecordIsActiveAndValid(request))
                .WithMessage("The Medical Record must be active to delete and have a valid StartDate");
        }

        private async Task<bool> MedicalRecordExists(int medicalRecordId)
        {
            var medicalRecord = await _medicalRecordRepository.GetById(medicalRecordId);
            return medicalRecord != null;
        }

        private async Task<bool> MedicalRecordIsActiveAndValid(SoftDeleteMedicalRecord request)
        {
            var medicalRecord = await _medicalRecordRepository.GetById(request.MedicalRecordId);
            if (medicalRecord == null)
            {
                return false;
            }

            bool isActive = medicalRecord.Status?.Name != "Inactive";

            bool isStartDateValid = medicalRecord.StartDate <= DateOnly.FromDateTime(DateTime.UtcNow);

            return isActive && isStartDateValid;
        }
    }
}
