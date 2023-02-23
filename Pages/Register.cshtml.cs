using _2FADemo.Models;
using Google.Authenticator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Security.Cryptography;

namespace _2FADemo.Pages
{
    // Registration page
    public class RegisterModel : PageModel
    {
        public string QrCodeUrl { get; set; }   // QR code
        public string ManualEntryCode { get; set; } // Numeric code

		[FromForm] // We expect the email field to get populated from the request, in the form on the page
		public string EmailAddress { get; set; }    // User's email

        public void OnGet()
        {

        }


        public void OnPost([FromServices] AppDbContext db)
        {
            // Make random key
            string key = GenerateRandomString(10);

            // Save new user to database
            db.Users.Add(new User
            {
                Id = Guid.NewGuid(),
                EmailAddress = EmailAddress,
                Key = key
            });
            db.SaveChanges();

            // Setup 2FA
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            SetupCode setupInfo = tfa.GenerateSetupCode("Test Two Factor", EmailAddress, key, false, 3);

            // Store QR code values into instance variables
            QrCodeUrl = setupInfo.QrCodeSetupImageUrl;
            ManualEntryCode = setupInfo.ManualEntryKey;
        }


		// Generates a string with random characters, which is used to create a key
		public static string GenerateRandomString(int length, string allowableChars = null)
        {
			// Check for empty value of allowableChars variable
			if (string.IsNullOrEmpty(allowableChars))
            {
                // Set the default allowed characters to the alphabet
                allowableChars = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            }

            // Generate random data
            var data = new byte[length];

            // Generate random number
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(data); // Fills the data array with random values

			// Convert allowed characters to array
			var allowable = allowableChars.ToCharArray();
            var len = allowable.Length;

			// Create string of random letters
			var chars = new char[length];
			for (var i = 0; i < length; i++)
            {
                // Create random number for index value
                int randomIndex = data[i] % len;
				// Get the letter at that index from the array of allowed characters
				chars[i] = allowable[randomIndex];
            }

            return new string(chars);
        }
    }
}
