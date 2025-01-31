using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserService.API.DTOs;
using UserService.API.Infrastructure.Repositories;

namespace UserService.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IProfileRepository repo;
        private IMapper mapper;
        private ILogger<UserController> logger;

        public UserController(IProfileRepository _repo, IMapper _mapper, ILogger<UserController> _logger)
        {
            repo = _repo;
            mapper = _mapper;
            logger = _logger;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetUserProfile()
        {
            var userId = HttpContext.Request.Headers["X-User-Id"].ToString();
            if (userId is null) return Unauthorized();
            var profile = await repo.GetUserProfileById(Guid.Parse(userId));
            if (profile is null)
            {
                return NotFound();
            }
            return Ok(new
            {
                BasicInfo = mapper.Map<ProfileInfoDTO>(profile),
                PrivacySetting = new PrivacySettingDTO
                {
                    ShowParticipatedCourses = profile.ShowParticipatedCourses,
                    ShowProfile = profile.ShowProfile
                }
            });
        }

        [HttpPut("update-privacy")]
        public async Task<IActionResult> UpdatePrivacySetting([FromBody] PrivacySettingDTO settings)
        {
            var userId = HttpContext.Request.Headers["X-User-Id"].ToString();
            if (userId is null) return Unauthorized();
            var profile = await repo.GetUserProfileById(Guid.Parse(userId));
            if (profile is null)
            {
                return NotFound();
            }
            profile.ShowProfile = settings.ShowProfile;
            profile.ShowParticipatedCourses = settings.ShowParticipatedCourses;
            int result = await repo.SaveChanges();
            return (result > 0) ? Ok(settings) : BadRequest();
        }

        [HttpPut("update-basic-info")]
        public async Task<IActionResult> UpdateProfileInfo([FromBody] ProfileInfoDTO profileDto)
        {
            var userId = HttpContext.Request.Headers["X-User-Id"].ToString();
            if (userId is null) return Unauthorized();
            logger.LogInformation($"=======> UserId: {userId}");
            var profile = await repo.GetUserProfileById(Guid.Parse(userId));
            if (profile is null)
            {
                return BadRequest();
            }
            mapper.Map(profileDto, profile);
            await repo.SaveChanges();
            return Ok(mapper.Map<ProfileInfoDTO>(profile));
        }

        [HttpPut("update-avatar")]
        public async Task<IActionResult> UpdateAvatar()
        {
            var userId = HttpContext.Request.Headers["X-User-Id"].ToString();
            if (userId is null) return Unauthorized();
            return Ok();
        }

        [HttpGet("get-public-profile/{profileId}")]
        public async Task<IActionResult> GetPublicProfile(Guid profileId)
        {
            var userId = HttpContext.Request.Headers["X-User-Id"].ToString();
            if (userId is null) return Unauthorized();
            var profile = await repo.GetUserProfileById(profileId);
            if (profile is null)
            {
                return NotFound();
            }
            if (!profile.ShowProfile) return BadRequest();
            return Ok(mapper.Map<ProfileInfoDTO>(profile));
        }
    }
}
