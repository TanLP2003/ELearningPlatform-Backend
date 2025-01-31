using AutoMapper;
using LearningService.API.Dtos;
using LearningService.API.Entities;

namespace LearningService.API.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EnrolledCourse, EnrolledCourseDto>();
            CreateMap<CourseReview, CourseReviewDto>();
            CreateMap<LearningNote, LearningNoteDto>();
        }
    }
}
