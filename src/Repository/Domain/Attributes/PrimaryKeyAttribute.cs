namespace Repository.Domain.Attributes;

/// <summary>
///     Use this attribute to set a property of the entity as the primary key.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class PrimaryKeyAttribute : Attribute
{
}
