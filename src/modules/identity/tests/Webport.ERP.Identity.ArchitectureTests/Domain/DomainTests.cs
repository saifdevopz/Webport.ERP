using NetArchTest.Rules;
using System.Reflection;
using Webport.ERP.Common.Domain.Abstractions;
using Webport.ERP.Identity.ArchitectureTests.Abstractions;

namespace Webport.ERP.Identity.ArchitectureTests.Domain;

public class DomainTests : BaseTest
{
    [Fact]
    public void DomainEvents_Should_BeSealed()
    {
        Types.InAssembly(DomainAssembly)
            .That()
            .ImplementInterface(typeof(IDomainEvent))
            .Or()
            .Inherit(typeof(DomainEvent))
            .Should()
            .BeSealed()
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void DomainEvent_ShouldHave_DomainEventPostfix()
    {
        Types.InAssembly(DomainAssembly)
            .That()
            .ImplementInterface(typeof(IDomainEvent))
            .Or()
            .Inherit(typeof(DomainEvent))
            .Should()
            .HaveNameEndingWith("DomainEvent", StringComparison.CurrentCulture)
            .GetResult()
            .ShouldBeSuccessful();
    }

    [Fact]
    public void Entities_ShouldHave_PrivateParameterlessConstructor()
    {
        var entityTypes = GetEntityTypes();

        var failingTypes = entityTypes
            .Where(t =>
                !t.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
                  .Any(c => c.IsPrivate && c.GetParameters().Length == 0))
            .ToList();

        Assert.True(failingTypes.Count == 0,
            $"Entities missing private parameterless constructor: {string.Join(", ", failingTypes.Select(t => t.Name))}");
    }

    [Fact]
    public void Entities_ShouldOnlyHave_PrivateConstructors()
    {
        IEnumerable<Type> entityTypes = GetEntityTypes();

        var failingTypes = entityTypes
            .Where(t =>
                t.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Length > 0)
            .ToList();

        Assert.True(failingTypes.Count == 0,
            $"Entities with public constructors: {string.Join(", ", failingTypes.Select(t => t.Name))}");
    }

    [Fact]
    public void Entities_Should_BeSealed()
    {
        var entityTypes = Types
            .InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(AggregateRoot)) // Replace with AggregateRoot or your base entity            
            .GetTypes();

        var failingTypes = entityTypes
            .Where(t => !t.IsSealed)
            .ToList();

        Assert.True(failingTypes.Count == 0,
            $"These entity classes are not sealed: {string.Join(", ", failingTypes.Select(t => t.Name))}");
    }


    private static IEnumerable<Type> GetEntityTypes() =>
        Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(AggregateRoot))
            .GetTypes();
}