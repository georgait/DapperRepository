namespace Repository.Domain.Attributes;

/// <summary>
///     Use this attribute to annotate a child entity as the 'MANY' of a one-to-many relationship.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class ManyRelationshipAttribute : Attribute
{
}
