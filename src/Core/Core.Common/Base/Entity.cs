using System.ComponentModel.DataAnnotations;

namespace Core.Common.Base;

public abstract class Entity
{
    [Key]
    public virtual string Id { get; private set; }

    public Entity()
    {
        Id = Guid.NewGuid().ToString();
    }
}