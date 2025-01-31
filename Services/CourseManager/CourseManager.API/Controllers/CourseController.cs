using AutoMapper;
using CourseManager.API.Applications.Commands.AddDescriptionForLecture;
using CourseManager.API.Applications.Commands.AddLectureToSection;
using CourseManager.API.Applications.Commands.AddSection;
using CourseManager.API.Applications.Commands.AddVideoToLectureFromLibrary;
using CourseManager.API.Applications.Commands.CreateCourse;
using CourseManager.API.Applications.Commands.MakeCoursePublic;
using CourseManager.API.Applications.Commands.UpdateCourseInfo;
using CourseManager.API.Applications.Commands.UpdateInstructorInfo;
using CourseManager.API.Applications.Queries.GetAllCategories;
using CourseManager.API.Applications.Queries.GetAllCourse;
using CourseManager.API.Applications.Queries.GetCourse;
using CourseManager.API.Applications.Queries.GetMyTeachingCourse;
using CourseManager.API.Dtos;
using CourseManager.API.Protos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourseManager.API.Controllers
{
    [Route("api/course")]
    [ApiController]
    [Authorize]
    public class CourseController(ISender sender, IMapper mapper,
        //StudyListProtoService.StudyListProtoServiceClient client
        LearningServiceProtoGrpc.LearningServiceProtoGrpcClient client
        ) : ControllerBase
    {
        [HttpPost("create-course")]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseRequest request)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var command = mapper.Map<CreateCourseCommand>(request);
            command.InstructorId = Guid.Parse(userId);
            var result = await sender.Send(command);
            return Ok(result);
        }

        [HttpPost("add-section-to-course")]
        public async Task<IActionResult> AddSectionToCourse([FromBody] AddSectionToCourseRequest request)
        {
            //var command = new AddSectionToCourseCommand(courseId, "Introduction");
            var command = mapper.Map<AddSectionToCourseCommand>(request);
            var result = await sender.Send(command);
            if (result.IsFailure)
            {
                return Problem(detail: result.Error.Message, statusCode: StatusCodes.Status400BadRequest);
            }
            return Ok(result.Value);
        }
        [HttpPost("add-lecture-to-section")]
        public async Task<IActionResult> AddLectureToSection([FromBody] AddLectureToSectionRequest request)
        {
            var command = mapper.Map<AddLectureToSectionCommand>(request);
            var result = await sender.Send(command);
            if (result.IsFailure)
            {
                return Problem(detail: result.Error.Message, statusCode:  StatusCodes.Status400BadRequest);
            }
            return Ok(result.Value);
        }

        [HttpGet("{courseId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCourse(Guid courseId)
        {
            var getCourseQuery = new GetCourseQuery(courseId);
            var result = await sender.Send(getCourseQuery);
            return Ok(mapper.Map<CourseOverview>(result));  
        }

        [HttpPut("add-video-to-lecture")]
        public async Task<IActionResult> AddVideoToLecture([FromBody] AddVideoToLectureRequest request)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var newCmd = new AddVideoToLectureFromLibraryCommand(Guid.Parse(userId), request.CourseId, request.LectureId, request.VideoId);
            var result = await sender.Send(newCmd);
            return Ok(result);
        }
        [HttpGet("get-my-teaching-course")]
        public async Task<IActionResult> GetMyTeachingCourse()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var newQuery = new GetMyTeachingCourseQuery(Guid.Parse(userId));
            var result = await sender.Send(newQuery);
            return Ok(result);
        }

        [HttpPut("add-description-for-lecture")]
        public async Task<IActionResult> AddDescriptionForLecture([FromBody] AddDescriptionForLectureRequest request)
        {
            var command = new AddDescriptionForLectureCommand
            {
                Description = request.Description,
                LectureId = request.LectureId,
                CourseId = request.CourseId
            };
            var result = await sender.Send(command);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpGet("secure-{courseId}")]
        public async Task<IActionResult> GetCourseForLearning(Guid courseId)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (userId is null) return Unauthorized();
            var requestAccessResult = await client.AuthenticateRequestAccessCourseAsync(new RequestAccessCourse
            {
                CourseId = courseId.ToString(),
                UserId = userId
            });
            if (!requestAccessResult.Result) return BadRequest("You don't have permission to access this course");
            var newCmd = new GetCourseQuery(courseId);
            var result = await sender.Send(newCmd);
            return Ok(result);
        }
        [HttpGet("preview-{courseId}")]
        public async Task<IActionResult> GetCourseForPreviewing(Guid courseId)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (userId is null) return Unauthorized();
            var newCmd = new GetCourseQuery(courseId);
            var result = await sender.Send(newCmd);
            if (result.InstructorId.ToString() != userId) return BadRequest("You don't have permission to access this course");
            return Ok(result);    
        }

        [HttpPut("make-course-public/{courseId}")]
        public async Task<IActionResult> MakeCoursePublic(Guid courseId)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null) return Unauthorized();
            var command = new MakeCoursePublicCommand(courseId);
            var result = await sender.Send(command);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpPut("update-course-info/{courseId}")]
        public async Task<IActionResult> UpdateCourseInfo(Guid courseId, [FromBody] UpdateCourseInfoRequest request)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null) return Unauthorized();
            var command = new UpdateCourseInfoCommand
            {
                CourseId = courseId,
                Title = request.Title,
                Description = request.Description,
                Level = request.Level,
                Price = request.Price,
                CategoryId = request.Category,
                Language = request.Language
            };
            var result = await sender.Send(command);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpGet("get-all")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCourse()
        {
            var newGetAllCourseQuery = new GetAllCourseQuery();
            var result = await sender.Send(newGetAllCourseQuery);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpPut("update-instructor-info-of-courses/{teacherName}")]
        public async Task<IActionResult> UpdateCourseInfoCommand(string teacherName)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null) return Unauthorized();
            var cmd = new UpdateInstructorInfoCommand(Guid.Parse(userId),  teacherName);
            var result = await sender.Send(cmd);
            return result.IsSuccess ? Ok(result) : BadRequest(result.Error);
        }

        [HttpGet("get-all-categories")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCategories()
        {
            var query = new GetAllCategoriesQuery();
            var result = await sender.Send(query);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
    }
}
