using gild_repo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace gild_repo_tests
{
    [TestClass]
    public class DayRepoTests
    {
        private DayRepo _dayRepo;
        private FileSystemConnectionContext _connectionContext;

        [TestInitialize]
        public void Setup()
        {
            _connectionContext = new FileSystemConnectionContext { DataRoot = @"TEST:\ThisIsNotARealDirectory\" };

            var fileSystem = new InMemoryFileSystem();
            _dayRepo = new DayRepo(fileSystem);
        }

        [TestMethod]
        public void Day_repo__current_day_should_start_at_zero()
        {
            var currentDay = _dayRepo.GetCurrentDay(_connectionContext);
            currentDay.ShouldBe(0);
        }

        [TestMethod]
        public void Day_repo__advancing_should_increment_the_day()
        {
            const int TotalDaysToAdvance = 10;
            for (var i = 0; i < TotalDaysToAdvance; i++)
            {
                _dayRepo.AdvanceDay(_connectionContext);
                _dayRepo.GetCurrentDay(_connectionContext).ShouldBe(i + 1);
            }
        }
    }
}
