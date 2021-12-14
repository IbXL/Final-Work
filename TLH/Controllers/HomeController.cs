using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TLH.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TLH.Models.Home;
using X.PagedList;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace TLH.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _context;

        public HomeController(ILogger<HomeController> logger, DataContext dataContext)
        {
            _context = dataContext;
            _logger = logger;
        }
        public IActionResult Index()
        {
            //var x = generateHash("GETproducts/full-feed/?format=xml&show_extended_info=1/0b52848d202d9f9e3f12df873994edcc" + "2021-10-07T13:12:32", "4923efbd41f600e53620108a2125feb7");
            return View();
        }
        public static string generateHash(string str, string cypherkey)
        {
            // based on CryptoJS.enc.Base64.parse
            byte[] keyBytes = System.Convert.FromBase64String(cypherkey);

            using (var hmacsha256 = new HMACSHA256(keyBytes))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(str));
                return Convert.ToBase64String(hashmessage);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
