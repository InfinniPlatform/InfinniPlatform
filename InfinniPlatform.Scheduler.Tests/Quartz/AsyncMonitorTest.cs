using System.Threading.Tasks;

using InfinniPlatform.Scheduler.Quartz;

using NUnit.Framework;

namespace InfinniPlatform.Scheduler.Tests.Quartz
{
    [TestFixture(Category = TestCategories.UnitTest)]
    public class AsyncMonitorTest
    {
        [Test]
        public async Task ShouldLockAsync()
        {
            // Given

            var monitor = new AsyncMonitor();

            const int taskDelay = 1000;

            var syncObj1 = new object();
            var syncObj2 = new object();

            var result1 = "";
            var result2 = "";

            var task11 = Task.Run(async () =>
                                        {
                                            using (await monitor.LockAsync(syncObj1))
                                            {
                                                result1 += "A";
                                                await Task.Delay(taskDelay);
                                                result1 += "A";
                                            }
                                        });

            var task12 = Task.Run(async () =>
                                        {
                                            using (await monitor.LockAsync(syncObj1))
                                            {
                                                result1 += "B";
                                                await Task.Delay(taskDelay);
                                                result1 += "B";
                                            }
                                        });

            var task21 = Task.Run(async () =>
                                        {
                                            using (await monitor.LockAsync(syncObj2))
                                            {
                                                result2 += "C";
                                                await Task.Delay(taskDelay);
                                                result2 += "C";
                                            }
                                        });

            var task22 = Task.Run(async () =>
                                        {
                                            using (await monitor.LockAsync(syncObj2))
                                            {
                                                result2 += "D";
                                                await Task.Delay(taskDelay);
                                                result2 += "D";
                                            }
                                        });

            // When

            await Task.WhenAll(task11, task12, task21, task22);

            // Then

            Assert.IsTrue(result1 == "AABB" || result1 == "BBAA");
            Assert.IsTrue(result2 == "CCDD" || result2 == "DDCC");
        }
    }
}