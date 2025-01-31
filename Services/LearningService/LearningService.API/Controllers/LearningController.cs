using AutoMapper;
using EventBus.Abstractions;
using EventBus.Events;
using LearningService.API.Applications.Services;
using LearningService.API.Dtos;
using LearningService.API.Protos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearningService.API.Controllers
{
    [Route("api/learning")]
    [ApiController]
    public class LearningController(
        ILearningServices services, 
        IMapper mapper,
        IEventBus eventBus,
        CourseManagerProtoService.CourseManagerProtoServiceClient client) : ControllerBase
    {
        [HttpGet("enrolled-courses")]
        public async Task<IActionResult> GetAllEnrolledCourse()
        {
            var userId = HttpContext.Request.Headers["X-User-Id"].ToString();
            if (userId == null) return Unauthorized();
            var enrolledCourses = await services.GetAllEnrolledCoursesByUserId(Guid.Parse(userId));
            var multipleBasicCourseInfoRequest = new MultipleBasicCourseInfoRequest();
            multipleBasicCourseInfoRequest.Requests.AddRange(enrolledCourses.Select(x => new GetBasicCourseInfoRequest { CourseId = x.CourseId.ToString() }));
            var coursesInfo = await client.GetMultipleBasicCourseInfoAsync(multipleBasicCourseInfoRequest);
            var enrolledCoursesDto = mapper.Map<List<EnrolledCourseDto>>(enrolledCourses);
            foreach(var ecDto in enrolledCoursesDto)
            {
                var courseInfo = coursesInfo.Courses.FirstOrDefault(c => c.CourseId == ecDto.CourseId.ToString());
                if(courseInfo == null) continue;
                AddBasicCourseInfoForEnrolledCourse(ecDto, courseInfo);
            }
            return Ok(enrolledCoursesDto);
        }

        [HttpPut("archive-unarchive-courses")]
        public async Task<IActionResult> ArchiveOrUnarchiveEnrolledCourse([FromBody] ArchiveCourseRequest request)
        {
            var userId = HttpContext.Request.Headers["X-User-Id"].ToString();
            if (userId == null) return Unauthorized();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await services.ArchiveOrUnarchiveEnrolledCourse(Guid.Parse(userId), request.CourseId, request.SetArchived);
            if (result.IsFailure) return BadRequest(result.Error);
            var enrolledCourseDto = mapper.Map<EnrolledCourseDto>(result.Value);
            var courseBasicInfo = await client.GetBasicCourseInfoAsync(new GetBasicCourseInfoRequest { CourseId = result.Value.CourseId.ToString() });
            AddBasicCourseInfoForEnrolledCourse(enrolledCourseDto, courseBasicInfo);
            return Ok(enrolledCourseDto);
        }

        [HttpGet("notes/enrolled-course/{courseId}")]
        public async Task<IActionResult> GetLearningNotes(Guid courseId)
        {
            var userId = HttpContext.Request.Headers["X-User-Id"].ToString();
            if (userId == null) return Unauthorized();
            return Ok(mapper.Map<LearningNoteDto>(await services.GetUserLearningNotesForEnrolledCourse(Guid.Parse(userId), courseId)));
        }

        [HttpPost("notes")]
        public async Task<IActionResult> AddLearningNote([FromBody] LearningNoteDto learningNoteDto)
        {
            var userId = HttpContext.Request.Headers["X-User-Id"].ToString();
            if (userId == null) return Unauthorized();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await services.AddLearningNote(Guid.Parse(userId), learningNoteDto.CourseId, learningNoteDto.LectureId, learningNoteDto.Content, learningNoteDto.NoteAt);
            return result.IsSuccess ? Ok(learningNoteDto) : BadRequest(result.Error);
        }

        [HttpPut("notes")]
        public async Task<IActionResult> UpdateLearningNote([FromBody] LearningNoteDto learningNoteDto)
        {
            var userId = HttpContext.Request.Headers["X-User-Id"].ToString();
            if (userId == null) return Unauthorized();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await services.UpdateLearningNote(Guid.Parse(userId), learningNoteDto.CourseId, learningNoteDto.LectureId, learningNoteDto.NoteAt, learningNoteDto.Content);
            return result.IsSuccess ? Ok(learningNoteDto) : BadRequest(result.Error);
        }

        [HttpDelete("notes/{courseId}/{lectureId}/{seconds}")]
        public async Task<IActionResult> DeleteLearningNote(Guid courseId, Guid lectureId, double seconds)
        {
            var userId = HttpContext.Request.Headers["X-User-Id"].ToString();
            if (userId == null) return Unauthorized();
            var result = await services.DeleteLearningNote(Guid.Parse(userId), courseId, lectureId, TimeSpan.FromSeconds(seconds));
            return result.IsSuccess ? NoContent() : BadRequest(result.Error);
        }

        [HttpPost("reviews")]
        public async Task<IActionResult> AddCourseReview([FromBody] CourseReviewDto courseReviewDto)
        {
            var userId = HttpContext.Request.Headers["X-User-Id"].ToString();
            if (userId == null) return Unauthorized();
            if (!ModelState.IsValid) return BadRequest(ModelState); ;
            var result = await services.AddCourseReview(Guid.Parse(userId), courseReviewDto.CourseId, courseReviewDto.Rating, courseReviewDto.ReviewText);
            if (result.IsFailure) return BadRequest(result.Error);
            await eventBus.PublishEventAsync(new CourseReviewedEvent { CourseId = result.Value.CourseId });
            var enrolledCourseDto = mapper.Map<EnrolledCourseDto>(result.Value);
            var courseBasicInfo = await client.GetBasicCourseInfoAsync(new GetBasicCourseInfoRequest { CourseId = result.Value.CourseId.ToString() });
            AddBasicCourseInfoForEnrolledCourse(enrolledCourseDto, courseBasicInfo);
            return Ok(enrolledCourseDto);
        }

        [HttpPut("reviews")]
        public async Task<IActionResult> UpdateCourseReview([FromBody] CourseReviewDto courseReviewDto)
        {
            var userId = HttpContext.Request.Headers["X-User-Id"].ToString();
            if (userId == null) return Unauthorized();
            if (!ModelState.IsValid) return BadRequest(ModelState); ;
            var result = await services.UpdateCourseReview(Guid.Parse(userId), courseReviewDto.CourseId, courseReviewDto.Rating, courseReviewDto.ReviewText);
            if (result.IsFailure) return BadRequest(result.Error);
            var enrolledCourseDto = mapper.Map<EnrolledCourseDto>(result.Value);
            var courseBasicInfo = await client.GetBasicCourseInfoAsync(new GetBasicCourseInfoRequest { CourseId = result.Value.CourseId.ToString() });
            AddBasicCourseInfoForEnrolledCourse(enrolledCourseDto, courseBasicInfo);
            return Ok(enrolledCourseDto);
        }

        [HttpDelete("reviews/enrolled-course/{courseId}")]
        public async Task<IActionResult> DeleteCourseReview(Guid courseId)
        {
            var userId = HttpContext.Request.Headers["X-User-Id"].ToString();
            if (userId == null) return Unauthorized();
            var result = await services.DeleteCourseReview(Guid.Parse(userId), courseId);
            return result.IsSuccess ? NoContent() : BadRequest(result.Error);
        }

        private void AddBasicCourseInfoForEnrolledCourse(EnrolledCourseDto enrolledCourse, CourseBasicInfo courseBasicInfo)
        {
            enrolledCourse.CourseTitle = courseBasicInfo.CourseName;
            enrolledCourse.CourseImage = courseBasicInfo.CourseImage;
            enrolledCourse.InstructorId = Guid.Parse(courseBasicInfo.AuthorId);
            enrolledCourse.InstructorName = courseBasicInfo.AuthorName;
        }
    }
}
