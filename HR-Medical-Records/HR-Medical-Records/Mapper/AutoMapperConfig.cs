using AutoMapper;
using HR_Medical_Records.DTOs.MedicalRecordDTOs;
using HR_Medical_Records.Models;
using System;

namespace HR_Medical_Records.Mapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {

            CreateMap<TMedicalRecord, MedicalRecordDTO>().ReverseMap();
        }
    }
}
