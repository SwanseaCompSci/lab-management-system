using NUnit.Framework;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Allocation
{
    public abstract class TestBase
    {
        [SetUp]
        public async Task TestSetUp()
        {
            await Testing.ResetState();
        }
    }
}
