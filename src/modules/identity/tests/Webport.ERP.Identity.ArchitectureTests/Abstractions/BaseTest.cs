using System.Reflection;
using Webport.ERP.Identity.Domain.Entities.User;
using Webport.ERP.Identity.Infrastructure;

namespace Webport.ERP.Identity.ArchitectureTests.Abstractions;

public abstract class BaseTest
{
    protected static readonly Assembly ApplicationAssembly = typeof(Application.AssemblyReference).Assembly;

    protected static readonly Assembly DomainAssembly = typeof(UserM).Assembly;

    protected static readonly Assembly InfrastructureAssembly = typeof(IdentityModule).Assembly;

    protected static readonly Assembly PresentationAssembly = typeof(Presentation.AssemblyReference).Assembly;
}