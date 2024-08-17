using Game.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Game.Client.Pages.Registration;

namespace Game.Controllers
{
    public class RegisterController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserStore<AppUser> _userStore;

        public RegisterController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IUserStore<AppUser> userStore)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userStore = userStore;
        }

        [HttpPost("/Registerr")]
        public async Task<IActionResult> Register([FromForm] RegistrationModel model)
        {

            Console.WriteLine("mach");
            if (model == null)
            {
                return BadRequest("Invalid registration data.");
            }

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                return Conflict("A user with this email already exists.");
            }

            var user = new AppUser();

            // Set the username
            await _userStore.SetUserNameAsync(user, model.Username, CancellationToken.None);

            // Set the email using GetEmailStore
            var emailStore = GetEmailStore();
            await emailStore.SetEmailAsync(user, model.Email, CancellationToken.None);

            // Create the user with the specified password
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return Ok("User created successfully.");
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest($"User creation failed: {errors}");
            }
        }

        private IUserEmailStore<AppUser> GetEmailStore()
        {
            var emailStore = _userStore as IUserEmailStore<AppUser>;
            if (emailStore == null)
            {
                throw new NotSupportedException("The user store does not support email operations.");
            }
            return emailStore;
        }

        public static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256 instance
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Compute the hash as a byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert the byte array to a string
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
