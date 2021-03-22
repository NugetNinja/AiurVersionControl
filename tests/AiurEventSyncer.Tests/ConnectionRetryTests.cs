﻿using AiurEventSyncer.ConnectionProviders;
using AiurEventSyncer.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiurEventSyncer.Tests
{
    [TestClass]
    public class ConnectionRetryTests
    {
        [TestMethod]
        public async Task TestRetry()
        {
            var connection = new RetryableWebSocketConnection<Book>("wss://aaaa.bbbb.ccccccc/aaaaaa/dddddd/eeeee/fff.ares");
            var retryCounts = 0;
            var startTime = DateTime.UtcNow;
            connection.PropertyChanged += (o, e) =>
            {
                Console.WriteLine($"{(DateTime.UtcNow - startTime).TotalSeconds} seconds passed. Retried: {connection.AttemptCount} times.");
                retryCounts = connection.AttemptCount;
            };
            var pullTask = connection.PullAndMonitor(null, null, null, true);
            var waitTask = Task.Delay(8 * 1000);
            await Task.WhenAny(pullTask, waitTask);
            Assert.AreEqual(4, retryCounts);
        }
    }
}
