using CourseManager.Domain.Enums;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager.Domain.Entities
{
    public class Resource
    {
        public Guid Id { get; set; }
        public Guid LectureId { get; set; }
        public ResourceType Type { get; set; }
        public string? ExternalLinkUrl { get; set; }
        public string? VideoUrl { get; set; }
        public string? FilesType { get; set; }
    }
}
