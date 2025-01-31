using CourseManager.Domain.Enums;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager.Domain.Entities;

public class Course : AggregateRoot<Guid>
{
    private readonly List<Section> _sections = new();
    public IReadOnlyList<Section> Sections => _sections.OrderBy(s => s.SectionNumber).ToList().AsReadOnly();
    public string Title { get; private set; }
    public string? Description { get; private set; } 
    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; }
    public Guid? SubCategoryId { get; private set; }
    public SubCategory? SubCategory { get; private set; }
    public CourseLevel Level { get; private set; }
    public CourseVisuability Visuability { get; private set; }
    public string? Language { get; private set; }
    public string? CourseImage { get; private set; }
    public int Price { get; private set; }
    public Guid InstructorId { get; private set; }
    public string InstructorName { get; set; }
    
    public CourseMetadata Metadata { get; private set; }
    private Course(
        Guid id,
        string title,
        CourseLevel level,
        Guid instructorId,
        Guid categoryId,
        string instructorName
    ) : base(id)
    {
        Title = title;
        Level = level;
        InstructorId = instructorId;
        Visuability = CourseVisuability.Draft;
        CategoryId = categoryId;
        InstructorName = instructorName;
    }
    public static Course Create(string title, CourseLevel level, Guid instructorId, Guid categoryId, string instructorName)
    {
        Guid id = Guid.NewGuid();
        return new Course(id, title, level, instructorId, categoryId, instructorName);
    }
    public Result<Section> AddSection(string sectionName)
    {
        var newSection = Section.Create(Guid.NewGuid(), Id, sectionName, _sections.Count + 1);
        _sections.Add(newSection);
        return newSection;
    }
    public Result<Lecture> AddLectureToSection(Guid sectionId, string lectureName)
    {
        var section = _sections.FirstOrDefault(s => s.Id == sectionId);
        if(section is not null)
        {
            var newLecture = Lecture.Create(Guid.NewGuid(), sectionId, lectureName, section.Lectures.Count + 1);
            section.AddLecture(newLecture);
            return newLecture;
        }
        return Result.Failure<Lecture>(Error.Create("Error.Section", "Section is null!"));
    }

    public Result MakeCoursePublic()
    {
        if(Title == null || Language == null  || Description == null || Sections.Count == 0 || Price <= 0)
        {
            return Result.Failure(Error.Create("Course.MakePublicError", "Course don't meet business rule"));
        }
        foreach(var section in Sections)
        {
            var sectionCheckResult = section.CheckIfCanBePublished();
            if (sectionCheckResult.IsFailure) return sectionCheckResult;
        }
        Visuability = CourseVisuability.Public;
        return Result.Success();
    }

    public Result UpdateCourseInfo(string? title, string? description, string? level, int? price, Guid? categoryId, string? language)
    {
        if (title != null && title.Trim().Length > 0)
        {
            Title = title;
        }
        if (description != null && description.Trim().Length > 0)
        {
            Description = description;
        }
        if(level != null)
        {
            var result = Enum.TryParse(typeof(CourseLevel), level.Trim(), out var levelValue);
            if (!result) return Result.Failure(Error.Create("Course.EnumError", $"{level} is not valid value"));
            Level = (CourseLevel)levelValue!;
        }
        if(price != null && price <= 0)
        {
            return Result.Failure(Error.Create("Course.InvalidValue", $"Price {price} is not a valid value"));
        }
        Console.WriteLine($"============> Price: {price}");
        if (price != null) Price = (int)price;
        if(categoryId is not null)
        {
            CategoryId = categoryId.Value;
        }
        if (language != null && language.Trim().Length > 0)
        {
            Language = language;
        }
        return Result.Success();
    }
}