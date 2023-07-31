namespace HouseRentingSystem.Services.Tests
{
    using Microsoft.EntityFrameworkCore;

    using Data;
    using Data.Interfaces;
    using HouseRentingSystem.Data;

    using static DatabaseSeeder;

    public class AgentServiceTests
    {
        // First way: Using InMemory Database
        // Pros: Testing is as close to the production scenario as possible
        // Cons: You are testing EFCore functionality as well, so this is not good UNIT test
        // Hard to arrange the scenario
        // Second way: Using Mock of IRepository
        // Pros: Good unit testing, tests single unit, easy push test data
        // Cons: You need to have repository pattern
        private DbContextOptions<HouseRentingDbContext> dbOptions;
        private HouseRentingDbContext dbContext;

        private IAgentService agentService;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            this.dbOptions = new DbContextOptionsBuilder<HouseRentingDbContext>()
                .UseInMemoryDatabase("HouseRentingInMemory" + Guid.NewGuid().ToString())
                .Options;
            this.dbContext = new HouseRentingDbContext(this.dbOptions, false);

            this.dbContext.Database.EnsureCreated();
            SeedDatabase(this.dbContext);

            this.agentService = new AgentService(this.dbContext);
        }

        [Test]
        public async Task AgentExistsByUserIdAsyncShouldReturnTrueWhenExists()
        {
            string existingAgentUserId = AgentUser.Id.ToString();

            bool result = await this.agentService.AgentExistsByUserIdAsync(existingAgentUserId);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task AgentExistsByUserIdAsyncShouldReturnFalseWhenNotExists()
        {
            string existingAgentUserId = RenterUser.Id.ToString();

            bool result = await this.agentService.AgentExistsByUserIdAsync(existingAgentUserId);

            Assert.IsFalse(result);
        }
    }
}