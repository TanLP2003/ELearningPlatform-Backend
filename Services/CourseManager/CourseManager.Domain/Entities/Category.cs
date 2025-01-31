using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager.Domain.Entities;

public class Category : Entity<Guid>
{
    public string Name { get; private set; }
    public IReadOnlyList<SubCategory> SubCategories { get; set; }
    private Category(Guid id, string name) : base(id)
    {
        Name = name;
    }
    public static Category Create(string name)
    {
        var id = Guid.NewGuid();
        return new Category(id, name);
    }
}