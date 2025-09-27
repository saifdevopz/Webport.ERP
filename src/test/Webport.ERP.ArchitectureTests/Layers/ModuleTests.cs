using NetArchTest.Rules;
using System.Reflection;
using Webport.ERP.ArchitectureTests.Abstractions;
using Webport.ERP.Identity.Domain;
using Webport.ERP.Identity.Infrastructure;

namespace Webport.ERP.ArchitectureTests.Layers;

public class ModuleTests : BaseTest
{
    [Fact]
#pragma warning disable CA1707 // Identifiers should not contain underscores
    public void IdentityModule_ShouldNotHaveDependencyOn_AnyOtherModule()
    {
        string[] otherModules = [InventoryNamespace];
        string[] integrationEventsModules = [
            InventoryIntegrationEventsNamespace];

        List<Assembly> identityAssemblies =
        [
            AssemblyReference.Assembly,
            Identity.Application.AssemblyReference.Assembly,
            Identity.Presentation.AssemblyReference.Assembly,
            typeof(IdentityModule).Assembly,
        ];

        Types.InAssemblies(identityAssemblies)
            .That()
            .DoNotHaveDependencyOnAny(integrationEventsModules)
            .Should()
            .NotHaveDependencyOnAny(otherModules)
            .GetResult()
            .ShouldBeSuccessful();

    }
}
