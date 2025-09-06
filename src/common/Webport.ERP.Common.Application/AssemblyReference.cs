using System.Reflection;

namespace Webport.ERP.Common.Application;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}