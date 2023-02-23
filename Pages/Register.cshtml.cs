using _2FADemo.Models;
using Google.Authenticator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Security.Cryptography;

namespace _2FADemo.Pages
{
    public class RegisterModel : PageModel
    {
        public string QrCodeUrl { get; set; }
        public string ManualEntryCode { get; set; }

		[FromForm] // We expect this to get populated from the request
		public string EmailAddress { get; set; }

        public void OnGet()
        {

        }


        public void OnPost([FromServices] AppDbContext db)
        {
            //string key = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
            // Make key cryptographically sound
            string key = GenerateRandomString(10);

            // Save to database
            db.Users.Add(new User
            {
                Id = Guid.NewGuid(),
                EmailAddress = EmailAddress,
                Key = key
            });
            db.SaveChanges();

            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            SetupCode setupInfo = tfa.GenerateSetupCode("Test Two Factor", EmailAddress, key, false, 3);

            QrCodeUrl = setupInfo.QrCodeSetupImageUrl;
            ManualEntryCode = setupInfo.ManualEntryKey;
        }


        public static string GenerateRandomString(int length, string allowableChars = null)
        {
            if (string.IsNullOrEmpty(allowableChars))
            {
                allowableChars = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            }

            // Generate random data
            var data = new byte[length];

            //RandomNumberGenerator randomNumberGenerator = new RandomNumberGenerator();
            //using (var rng = randomNumberGenerator)
            //    rng.GetBytes(data);

            // Generate random data
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(data);

            var allowable = allowableChars.ToCharArray();
            var len = allowable.Length;
            var chars = new char[length];
            for (var i = 0; i < length; i++)
            {
                // Create random number for index value
                int randomIndex = data[i] % len;
                chars[i] = allowable[randomIndex];
            }

            return new string(chars);
        }
    }
}
