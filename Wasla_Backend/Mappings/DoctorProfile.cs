namespace Wasla_Backend.Mappings
{
    public class DoctorProfile : Profile
    {
        public DoctorProfile()
        {
            CreateMap<DoctorCompleteDto, Doctor>()
            .ForMember(d => d.hospitalname, o => o.MapFrom(s => s.hospitalName));


            CreateMap<Doctor, DoctorProfileResponse>()
            .ForMember(dest => dest.fullName, opt => opt.MapFrom(src => src.FullName))
             .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.Email))
             .ForMember(dest => dest.phone, opt => opt.MapFrom(src => src.Phone))
             .ForMember(dest => dest.image, opt => opt.MapFrom(src => src.ProfilePhoto))
             .ForMember(dest => dest.latitude, opt => opt.MapFrom(src => src.Latitude))
             .ForMember(dest => dest.longitude, opt => opt.MapFrom(src => src.Longitude))
             .ForMember(dest => dest.birthDay, opt => opt.MapFrom(src => src.BirthDay))
             .ForMember(dest => dest.cv, opt => opt.MapFrom(src => src.CV))
             .ForMember(dest => dest.universityName, opt => opt.MapFrom(src => src.UniversityName))
             .ForMember(dest => dest.description, opt => opt.MapFrom(src => src.Description))
             .ForMember(dest => dest.experienceYears, opt => opt.MapFrom(src => src.ExperienceYears))
             .ForMember(dest => dest.graduationYear, opt => opt.MapFrom(src => src.GraduationYear))
             .ForMember(dest => dest.hospitalname, opt => opt.MapFrom(src => src.hospitalname))
             .ForMember(dest => dest.specializationName,
                                opt => opt.MapFrom((src, dest, destMember, context) =>
                                src.Specialization?.Specialization?.GetText(context.Items["lan"]?.ToString())))
             .ForMember(dest => dest.numberOfpatients, opt => opt.MapFrom(src => src.numberOfpatients))
             .ForMember(dest => dest.rating, opt => opt.MapFrom(src => src.Rating));


            CreateMap<Doctor, AllDoctorDataDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.FullName, o => o.MapFrom(s => s.FullName))
                .ForMember(d => d.specialtyName, o => o.MapFrom(s => s.Specialization.Specialization.GetText("en")))
                .ForMember(d => d.ExperienceYears, o => o.MapFrom(s => s.ExperienceYears))
                .ForMember(d => d.Rating, o => o.MapFrom(s => s.Rating))
                .ForMember(d => d.UniversityName, o => o.MapFrom(s => s.UniversityName))
                .ForMember(d => d.GraduationYear, o => o.MapFrom(s => s.GraduationYear))
                .ForMember(d => d.hospitalname, o => o.MapFrom(s => s.hospitalname))
                .ForMember(d => d.numberOfpatients, o => o.MapFrom(s => s.numberOfpatients))
                .ForMember(d => d.BirthDay, o => o.MapFrom(s => s.BirthDay))
                .ForMember(d => d.Phone, o => o.MapFrom(s => s.Phone))
                .ForMember(d => d.Latitude, o => o.MapFrom(s => s.Latitude))
                .ForMember(d => d.Longitude, o => o.MapFrom(s => s.Longitude))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Description))
                .ForMember(d => d.ImageUrl, o => o.MapFrom(s => s.ProfilePhoto))
                .ForMember(d => d.CVUrl, o => o.MapFrom(s => s.CV));


            CreateMap<UpdateDoctorDto, Doctor>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.fullName))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.phone))
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.latitude))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.longitude))
                .ForMember(dest => dest.BirthDay, opt => opt.MapFrom(src => src.birthDay))
                .ForMember(dest => dest.ExperienceYears, opt => opt.MapFrom(src => src.experienceYears))
                .ForMember(dest => dest.UniversityName, opt => opt.MapFrom(src => src.universityName))
                .ForMember(dest => dest.GraduationYear, opt => opt.MapFrom(src => src.graduationYear))
                .ForMember(dest => dest.hospitalname, opt => opt.MapFrom(src => src.hospitalName));
            
        }

    }
}
