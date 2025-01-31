using AutoMapper;
using CourseManager.API.Applications.Commands.AddLectureToSection;
using CourseManager.API.Applications.Commands.AddSection;
using CourseManager.API.Applications.Commands.CreateCourse;
using CourseManager.API.Dtos;
using CourseManager.Domain.Entities;
using CourseManager.Domain.Enums;

namespace CourseManager.API.Applications.AutoMapperProfile;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateCourseRequest, CreateCourseCommand>()
            .ForMember(des => des.Level, opt => opt.MapFrom(src => (CourseLevel)Enum.Parse(typeof(CourseLevel), src.Level)));
        CreateMap<AddSectionToCourseRequest, AddSectionToCourseCommand>();
        CreateMap<AddLectureToSectionRequest, AddLectureToSectionCommand>();
        CreateMap<Lecture, LectureOverview>();
        CreateMap<Section, SectionOverview>();
        CreateMap<Course, CourseOverview>();
        CreateMap<CourseMetadata, CourseMetadataDto>();
    }
}