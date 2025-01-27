﻿using AutoMapper;
using HR_Medical_Records.DTOs.MedicalRecordDTOs;
using HR_Medical_Records.Models;

namespace HR_Medical_Records.Mapper
{
    /// <summary>
    /// AutoMapper configuration class for mapping between various medical record models and DTOs.
    /// </summary>
    public class AutoMapperConfig : Profile
    {
        /// <summary>
        /// Configures the mappings for medical record-related models.
        /// </summary>
        public AutoMapperConfig()
        {
            CreateMap<TMedicalRecord, MedicalRecordDTO>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Name))
                .ForMember(dest => dest.StatusDescription, opt => opt.MapFrom(src => src.Status.Description))
                .ForMember(dest => dest.MedicalRecordType, opt => opt.MapFrom(src => src.MedicalRecordType.Name))
                .ForMember(dest => dest.MedicalRecordTypeDescription, opt => opt.MapFrom(src => src.MedicalRecordType.Description))
                .ReverseMap();
            CreateMap<TMedicalRecord, SimpleMedicalRecordDTO>().ReverseMap();
            CreateMap<TMedicalRecord, CreateAndUpdateMedicalRecord>().ReverseMap();


        }
    }
}
