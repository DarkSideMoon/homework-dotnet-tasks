using Metrics.DotNet.Samples.Mvc.UI.Models;
using Metrics.DotNet.Samples.Services.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Metrics.DotNet.Samples.Mvc.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPostgresBookRepository _postgresRepository;

        public HomeController(IPostgresBookRepository postgresRepository)
        {
            _postgresRepository = postgresRepository;
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
            var book = await _postgresRepository.GetRandomBook();
            return View(book);
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
