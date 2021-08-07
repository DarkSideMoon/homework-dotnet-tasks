using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Homework.Dotnet.Tasks.Contracts;
using Homework.Dotnet.Tasks.Mvc.UI.Models;
using Homework.Dotnet.Tasks.Services.Cache;
using Homework.Dotnet.Tasks.Services.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Homework.Dotnet.Tasks.Mvc.UI.Controllers
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
