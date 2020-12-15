﻿using AiurEventSyncer.Models;
using AiurEventSyncer.Remotes;
using AiurEventSyncer.Tests.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace AiurEventSyncer.Tests
{
    [TestClass]
    public class MergeTest
    {
        private Repository<int> _localRepo;

        [TestInitialize]
        public void BuildBasicRepo()
        {
            _localRepo = new Repository<int>();
            _localRepo.Commit(1);
            _localRepo.Commit(2);
            _localRepo.Commit(3);
            _localRepo.Assert(1, 2, 3);
        }

        [TestMethod]
        public async Task TestPushWithDifferentTree()
        {
            var remoteRepo = new Repository<int>();
            remoteRepo.Commit(20);
            remoteRepo.Assert(20);

            await _localRepo.AddRemoteAsync(new ObjectRemote<int>(remoteRepo));

            await _localRepo.PullAsync();
            remoteRepo.Commit(50);

            _localRepo.Assert(20, 1, 2, 3);
            remoteRepo.Assert(20, 50);

            await _localRepo.PushAsync();
            _localRepo.Assert(20, 1, 2, 3);
            remoteRepo.Assert(20, 50, 1, 2, 3);

            await _localRepo.PullAsync();
            // 20,1,2,3
            _localRepo.Assert(20, 50, 1, 2, 3);
            remoteRepo.Assert(20, 50, 1, 2, 3);
        }

        [TestMethod]
        public async Task TestPullWithDifferentTree()
        {
            var remoteRepo = new Repository<int>();
            remoteRepo.Commit(20);
            remoteRepo.Assert(20);

            await _localRepo.AddRemoteAsync(new ObjectRemote<int>(remoteRepo));

            await _localRepo.PullAsync();
            remoteRepo.Commit(50);

            _localRepo.Assert(20, 1, 2, 3);
            remoteRepo.Assert(20, 50);

            await _localRepo.PullAsync();
            _localRepo.Assert(20, 50, 1, 2, 3);
            remoteRepo.Assert(20, 50);

            await _localRepo.PushAsync();
            _localRepo.Assert(20, 50, 1, 2, 3);
            remoteRepo.Assert(20, 50, 1, 2, 3);
        }
    }
}
