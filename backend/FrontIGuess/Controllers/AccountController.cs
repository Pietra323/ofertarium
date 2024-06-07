using backend.Data.Models;
using backend.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

public class AccountController : Controller
{
    private readonly IUserRepository _userRepo;

    public AccountController(IUserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                Password = model.Password // Note: Password should be hashed
            };

            var result = await _userRepo.CreatePersonAsync(user);

            if (result != null)
            {
                // Log the user in and redirect to home page or another appropriate page
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "An error occurred while creating the user.");
        }

        return View(model);
    }
}