using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Plugin.DeviceInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using Xamarin.Essentials;

namespace PushITapp.Services
{
    public static class HashCode
    {

        public static string GetHashCode()
        {
            // Getting current device id
            string password = CrossDeviceInfo.Current.Id;

            // generate a 128-bit salt
            byte[] salt = new byte[128 / 8];
            salt[0] = 128;

            // derive a 256-bit subkey (use HMACSHA256 with 1000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 1000,
                numBytesRequested: 40 / 8)); 

            return (hashed);
        }
    }
}
