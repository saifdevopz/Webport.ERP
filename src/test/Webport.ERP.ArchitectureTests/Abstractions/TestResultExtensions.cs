using NetArchTest.Rules;

namespace Webport.ERP.ArchitectureTests.Abstractions;

internal static class TestResultExtensions
{

    internal static void ShouldBeSuccessful(this TestResult testResult)
    {
        Assert.True(testResult.FailingTypes == null || !testResult.FailingTypes.Any(),
            $"Architecture test failed for types: {string.Join(", ", testResult.FailingTypes?.Select(t => t.Name) ?? [])}");
    }
}
