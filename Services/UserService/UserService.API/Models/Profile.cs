namespace UserService.API.Models
{
    public class Profile
    {
        public Guid Id { get; set; }
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
        public string? Avatar { get; set; }
        public bool ShowProfile { get; set; } = false;
        public bool ShowParticipatedCourses { get; set; } = false;
    }
}
