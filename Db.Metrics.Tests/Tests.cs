using System;
using System.Threading;
using NUnit.Framework;

namespace Db.Metrics.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Ignore("Криво пока")]
        [Test]
        public void Should_save_all_events()
        {
            var context = new MetricsContext();
            var counter = context.CreateIntegerCounter("test", TimeSpan.FromSeconds(10));
            var random = new Random();

            for (int i = 0; i < 3; i++)
            {
                var value = random.Next(1, 10);
                counter.Add(value);
                Console.WriteLine(value);
                Thread.Sleep(10 * 1000);
            }
            
            Thread.Sleep(1000);
            // и совпадают ли
            foreach (var @event in context.GetIntegerCountMetrics("test"))
            {
                Console.WriteLine($"{@event.Value}: {@event.DateTime}");
            }
            
            //Assert.AreEqual(3, context.GetIntegerCountMetrics("test"));
        }
    }
}