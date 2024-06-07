using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using backend.Data.Models;
using backend.Data.Repositories.Interfaces;

namespace YourNamespace.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IUserRepository _userRepo;

        public RegisterModel(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            public string Name { get; set; }

            [Required]
            public string LastName { get; set; }

            [Required]
            public string Username { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = new User
            {
                Name = Input.Name,
                LastName = Input.LastName,
                Username = Input.Username,
                Email = Input.Email,
                Password = Input.Password,
                isAdmin = false
            };

            await _userRepo.CreatePersonAsync(user);

            // Assuming you want to redirect the user to a different page after successful registration
            return RedirectToPage("/Index");
        }
    }
}
