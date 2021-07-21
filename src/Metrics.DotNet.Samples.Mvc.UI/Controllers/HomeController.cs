using Metrics.DotNet.Samples.Contracts;
using Metrics.DotNet.Samples.Mvc.UI.Models;
using Metrics.DotNet.Samples.Services.Cache;
using Metrics.DotNet.Samples.Services.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Metrics.DotNet.Samples.Mvc.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPostgresBookRepository _postgresRepository;
        private readonly IStorage<Book> _cacheStorage;

        public HomeController(IPostgresBookRepository postgresRepository, IStorage<Book> cacheStorage)
        {
            _postgresRepository = postgresRepository;
            _cacheStorage = cacheStorage;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Project()
        {
            return View();
        }

        public async Task<IActionResult> Book()
        {
            var bookId = new Random().Next(1, 10000);
            var bookCache = await _cacheStorage.GetOrSetItem(bookId.ToString(), async () => await _postgresRepository.GetRandomBook());
            return View(bookCache);
        }

        public async Task<IActionResult> Books(int count)
        {
            var books = await _postgresRepository.GetBooks(count);
            return View(books);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
