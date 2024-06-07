using Razoer.Models;
using Microsoft.AspNetCore.Mvc;
using backend.Data.Repositories.Interfaces;

namespace YourNamespace.Controllers
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
            return View();
        }

        [HttpPost]
        public IActionResult Index(User user)
        {
            if (ModelState.IsValid)
            {
                user.isAdmin = false;
                _userRepository.CreatePersonAsync(user);
                return RedirectToAction("Index", "Home"); // Redirect to home page after successful registration
            }
            return View(user);
        }
    }
}