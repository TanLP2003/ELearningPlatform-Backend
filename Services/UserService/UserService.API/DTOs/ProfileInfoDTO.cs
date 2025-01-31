using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UserService.API.DTOs
{
    public class ProfileInfoDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Headline { get; set; }
        public string? Description { get; set; }
        public string? Language { get; set; }
        public string? Website { get; set; }
        public string? Twitter { get; set; }
        public string? Facebook { get; set; }
        public string? Linkedin { get; set; }
        public string? Youtube { get; set; }
    }
}
