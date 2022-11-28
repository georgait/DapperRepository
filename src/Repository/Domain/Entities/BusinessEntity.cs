using System.Text.Json.Serialization;

namespace Repository.Domain.Entities;

/// <summary>
///     Represents the business entity that holds the business ID.
/// </summary>
/// <typeparam name="TId">The type of ID</typeparam>
public class BusinessEntity<TId>
{

    /// <summary>
    /// Get or Set the business entity ID.
    /// </summary>
    [PrimaryKey]
    [JsonPropertyOrder(-2)]
    public TId Id { get; set; }
}
