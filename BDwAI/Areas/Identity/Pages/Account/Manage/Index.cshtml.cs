using System.ComponentModel.DataAnnotations;
using BDwAI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BDwAI.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public IndexModel(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Adres Email")]
            public string Email { get; set; }

            [Display(Name = "Imię")]
            public string FirstName { get; set; }

            [Display(Name = "Nazwisko")]
            public string LastName { get; set; }

            [Display(Name = "Ulica i numer")]
            public string Street { get; set; }

            [Display(Name = "Miasto")]
            public string City { get; set; }

            [Display(Name = "Kod pocztowy")]
            public string ZipCode { get; set; }

            [Phone]
            [Display(Name = "Numer telefonu")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(AppUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var email = await _userManager.GetEmailAsync(user);

            Username = userName;

            Input = new InputModel
            {
                Email = email,
                PhoneNumber = phoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Street = user.Street,
                City = user.City,
                ZipCode = user.ZipCode
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Nie można załadować użytkownika o ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Nie można załadować użytkownika o ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            // Aktualizacja danych podstawowych (adres, imię itp.)
            if (user.FirstName != Input.FirstName) user.FirstName = Input.FirstName;
            if (user.LastName != Input.LastName) user.LastName = Input.LastName;
            if (user.Street != Input.Street) user.Street = Input.Street;
            if (user.City != Input.City) user.City = Input.City;
            if (user.ZipCode != Input.ZipCode) user.ZipCode = Input.ZipCode;

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
            }

            // Aktualizacja Emaila (i loginu, jeśli są takie same)
            var email = await _userManager.GetEmailAsync(user);
            if (Input.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
                if (!setEmailResult.Succeeded)
                {
                    StatusMessage = "Błąd podczas zmiany emaila.";
                    return RedirectToPage();
                }

                // Aktualizujemy też nazwę użytkownika, żeby była taka sama jak email
                await _userManager.SetUserNameAsync(user, Input.Email);
            }

            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);

            StatusMessage = "Twój profil został zaktualizowany";
            return RedirectToPage();
        }
    }
}