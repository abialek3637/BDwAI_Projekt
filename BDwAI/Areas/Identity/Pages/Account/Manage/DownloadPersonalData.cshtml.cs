using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using BDwAI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BDwAI.Areas.Identity.Pages.Account.Manage
{
    public class DownloadPersonalDataModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<DownloadPersonalDataModel> _logger;

        public DownloadPersonalDataModel(
            UserManager<AppUser> userManager,
            ILogger<DownloadPersonalDataModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Nie można załadować użytkownika o ID '{_userManager.GetUserId(User)}'.");
            }

            _logger.LogInformation("Użytkownik z ID '{UserId}' poprosił o swoje dane osobowe.", _userManager.GetUserId(User));

            var personalData = new Dictionary<string, string>();

            var personalDataProps = typeof(AppUser).GetProperties().Where(
                            prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));

            foreach (var p in personalDataProps)
            {
                personalData.Add(p.Name, p.GetValue(user)?.ToString() ?? "null");
            }

            var logins = await _userManager.GetLoginsAsync(user);
            foreach (var l in logins)
            {
                personalData.Add($"{l.LoginProvider} external login provider key", l.ProviderKey);
            }

            var authenticatorKey = await _userManager.GetAuthenticatorKeyAsync(user);
            if (!string.IsNullOrEmpty(authenticatorKey))
            {
                personalData.Add("Authenticator Key", authenticatorKey);
            }

            personalData.Add("Imię", user.FirstName ?? "Brak danych");
            personalData.Add("Nazwisko", user.LastName ?? "Brak danych");
            personalData.Add("Ulica", user.Street ?? "Brak danych");
            personalData.Add("Miasto", user.City ?? "Brak danych");
            personalData.Add("Kod pocztowy", user.ZipCode ?? "Brak danych");
            personalData.Add("Email (Login)", user.Email ?? "Brak danych");

            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All), 
                WriteIndented = true 
            };

            Response.Headers.Add("Content-Disposition", "attachment; filename=DaneOsobowe.json");

            return new FileContentResult(JsonSerializer.SerializeToUtf8Bytes(personalData, options), "application/json");
        }
    }
}