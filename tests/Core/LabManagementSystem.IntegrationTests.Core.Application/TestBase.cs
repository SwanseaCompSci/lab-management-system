using NUnit.Framework;
using System.Threading.Tasks;
using static SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Testing;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application
{
    public class TestBase
    {
        [SetUp]
        public async Task TestSetUp()
        {
            await ResetState();
        }
    }

}
