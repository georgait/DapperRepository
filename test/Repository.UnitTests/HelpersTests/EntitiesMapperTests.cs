namespace Repository.UnitTests.HelpersTests;

public class EntitiesMapperTests
{

    [Fact]
    public void TestMap()
    {
        var lookup = new Dictionary<int, Entity>();

        var entity = new Entity
        {
            Id = 1,
            Name = "Base Entity"
        };

        var nested1 = new NestedEntity1 { Id = 11, NestedName1 = "Nested 1" };

        var currentEntity = EntitiesMapper.Map(entity, nested1, lookup);

        Assert.NotNull(currentEntity);
        Assert.True(currentEntity.Name == "Base Entity");
        Assert.Collection(currentEntity.NestedEntities1, e =>
            e.NestedName1.First().Equals("Nested 1"));
    }
}

class Entity : BusinessEntity<int>
{
    public string Name { get; set; }

    [ManyRelationship]
    public List<NestedEntity1> NestedEntities1 { get; set; } = new();

    [ManyRelationship]
    public List<NestedEntity2> NestedEntities2 { get; set; } = new();
}

class NestedEntity1 : BusinessEntity<int>
{
    public string NestedName1 { get; set; }
}

class NestedEntity2 : BusinessEntity<int>   
{
    public string NestedName2 { get; set; }
}
