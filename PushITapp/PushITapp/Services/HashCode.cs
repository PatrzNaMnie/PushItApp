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
            //var ni = NetworkInterface.GetAllNetworkInterfaces()
            //.OrderBy(intf => intf.NetworkInterfaceType)
            //.Where(intf => intf.OperationalStatus == OperationalStatus.Up);

            //var hw = ni.First().GetPhysicalAddress();

            //string password = string.Join(":", (from ma in hw.GetAddressBytes() select ma.ToString("X2")).ToArray());

            string password = CrossDeviceInfo.Current.Id;

            // generate a 128-bit salt using a cryptographically strong random sequence of nonzero values
            byte[] salt = new byte[128 / 8];
            //using (var rngCsp = new RNGCryptoServiceProvider())
            //{
            //    rngCsp.GetNonZeroBytes(salt);
            //}
            //Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");

            salt[0] = 128;

            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 40 / 8)); 

            return (hashed);
        }
    }
}
