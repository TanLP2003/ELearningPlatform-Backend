using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain;

public abstract class Entity<TId>
{
    public TId Id { get; set; }
    protected Entity(TId id) => Id = id;
}