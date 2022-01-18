using BankApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace BankApp.Pages.Login
{
    [BindProperties]
    public class LoginModel : PageModel
    {
        private readonly BankAppDataContext _context;

        public LoginModel(BankAppDataContext context)
        {
            _context = context;
        }

        public int UserId { get; set; }

        [Required, DisplayName("User Name")]
        public string UserName { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        public string Message { get; set; }
        public void OnGet()
        {
        }
        public async Task <IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = new Models.User
                {
                    UserId = UserId,
                    LoginName = UserName,
                    
                };
                return Page();
            }
            
            if (UserName == "LoginName")
            {
                var claims = new List<Claim>();
                claims.Add(new Claim("UserName", "Password"));
                
                var identety = new ClaimsIdentity(claims, "MyCookieAuth");
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identety);
                await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal);

                return RedirectToAction("/Index");
            }

            return Page();
        }  
    }

}
