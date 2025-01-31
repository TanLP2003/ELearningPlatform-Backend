using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager.Domain.Entities
{
    public class SubCategory : Entity<Guid>
    {
        public Guid ParentCategoryId { get; private set; }
        public string Name { get; private set; }
        private SubCategory(Guid id, Guid parentCategoryId, string name) : base(id)
        {
            ParentCategoryId = parentCategoryId;
            Name = name;
        }

        public static SubCategory Create(Guid parentCategoryId, string name)
        {
            var subCategory = new SubCategory(Guid.NewGuid(), parentCategoryId, name);
            return subCategory;
        }
    }
}
