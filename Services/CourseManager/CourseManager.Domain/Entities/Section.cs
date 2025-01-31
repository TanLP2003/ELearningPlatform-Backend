using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager.Domain.Entities;

public class Section : Entity<Guid>
{
    private readonly List<Lecture> _lectures = new();
    public Guid CourseId { get; private set; }
    public string Name { get; private set; }
    public int SectionNumber { get; private set; }
    public IReadOnlyList<Lecture> Lectures => _lectures.OrderBy(l => l.LectureNumber).ToList().AsReadOnly();
    
    private Section(Guid id, Guid courseId, string name, int sectionNumber) 
        : base(id) 
    { 
        CourseId = courseId;
        Name = name;
        SectionNumber = sectionNumber;
    } 

    public static Section Create(Guid id, Guid courseId, string name, int sectionNumber)
    {
        return new Section(id, courseId, name, sectionNumber);
    }
    public void AddLecture(Lecture lecture)
    {
        _lectures.Add(lecture);
    }
    public Result CheckIfCanBePublished()
    {
        if (_lectures.Count == 0) return Result.Failure(Error.Create("Course.MakePublicError", "Course don't meet business rule"));
        foreach(var lecture in _lectures)
        {
            var lectureCheckResult = lecture.CheckIfCanBePublished();
            if (lectureCheckResult.IsFailure) return lectureCheckResult;
        }
        return Result.Success();
    }
}