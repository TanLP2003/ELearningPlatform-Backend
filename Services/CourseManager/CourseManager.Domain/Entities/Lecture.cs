using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager.Domain.Entities;
public class Lecture : Entity<Guid>
{
    private readonly List<Resource> _resources = new();
    public Guid SectionId { get; set; }
    public string Name { get; set; }
    public string? VideoName { get; set; }
    public string? LectureContentUrl { get; set; }
    public int LectureLength { get; set; }
    public string? Description { get; set; }
    
    public int LectureNumber { get; set; }
    public IReadOnlyList<Resource> Resources => _resources.ToList().AsReadOnly();

    private Lecture(Guid id, Guid sectionId, string name, int lectureNumber) : base(id) 
    {
        SectionId = sectionId;
        Name = name;
        LectureNumber = lectureNumber;
    }
    public static Lecture Create(Guid id, Guid sectionId, string title, int lectureNumber)
    {
        return new Lecture(id, sectionId, title, lectureNumber);
    }
    public Result CheckIfCanBePublished()
    {
        return (Name != null && VideoName != null && LectureContentUrl != null && Description != null && LectureNumber > 0) 
            ? Result.Success() 
            : Result.Failure(Error.Create("Lecture.InvalidState", $"Lecture {Id} is not in valid state to be published"));
    }
}