using backend.Data.Models;
using Razoer.Models;
using Microsoft.AspNetCore.Mvc;
using backend.Data.Repositories.Interfaces;
using User = backend.Data.Models.User;

namespace Razoer.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IUserRepository _userRepository;

        public RegisterController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            Console.WriteLine("Otwarto");
            return View();
        }

        [HttpPost]
        public IActionResult Index(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    user.isAdmin = false;
                    _userRepository.CreatePersonAsync(user);
                    return RedirectToAction("Index", "Home"); // Redirect to home page after successful registration
                }
                return View(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}