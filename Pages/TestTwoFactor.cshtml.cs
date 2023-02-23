using Google.Authenticator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _2FADemo.Pages
{
    public class TestTwoFactorModel : PageModel
    {
        [FromRoute] // Get value from URL parameter
        public string EmailAddress { get; set; }
        
        [FromForm] // Security code
        public string Code { get; set; }

        // Stores result of whether the user was verified
		public bool Result { get; set; }



		public void OnGet()
        {
        }


		public void OnPost([FromServices] AppDbContext db)
		{
            Result = false; // Reset form boolean

			// Get user's email from database
			var user = db.Users.FirstOrDefault(user => user.EmailAddress == EmailAddress);

            // If user exists
            if (user != null)
            {
				// Validate 2 factor authentication using the built in ASP.NET class from Google.Authenticator Nuget package
				TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
				Result = tfa.ValidateTwoFactorPIN(user.Key, Code);  // Returns true if user is validated
            }
		}
	}
}
