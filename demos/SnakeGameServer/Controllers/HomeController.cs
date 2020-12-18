﻿using AiurEventSyncer.Models;
using AiurEventSyncer.WebExtends;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SnakeGame.Models;

namespace SnakeGameServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly RepositoryContainer _repositoryContainer;

        public HomeController(RepositoryContainer repositoryContainer)
        {
            _repositoryContainer = repositoryContainer;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("repo.ares")]
        public Task<IActionResult> ReturnRepoDemo(string start)
        {
            var repo = _repositoryContainer.GetLogItemRepository();
            return new ActionBuilder().BuildWebActionResultAsync(HttpContext.WebSockets, repo, start);
        }
    }

    public class RepositoryContainer
    {
        private readonly object _obj = new object();
        private Repository<Position> _logItemRepository;

        public Repository<Position> GetLogItemRepository()
        {
            lock (_obj)
            {
                if (_logItemRepository == null)
                {
                    _logItemRepository = new Repository<Position>();
                }
            }
            return _logItemRepository;
        }
    }
}
