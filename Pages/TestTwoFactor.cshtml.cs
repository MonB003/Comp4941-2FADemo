using Google.Authenticator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _2FADemo.Pages
{
    public class TestTwoFactorModel : PageModel
    {
        [FromRoute] // Get value from URL parameter
        public string EmailAddress { get; set; }
        
        [FromForm]
        public string Code { get; set; }
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
                // Validate 2 factor authentication
                TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
				Result = tfa.ValidateTwoFactorPIN(user.Key, Code);
            }
		}
	}
}
