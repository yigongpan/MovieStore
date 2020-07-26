﻿using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using MoviesStore.Core.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using MovieStore.Core.ServiceInterfaces;

namespace MovieStore.Infrastructure.Services
{
    public class CryptoService : ICryptoService
    {
        //popular Hashing methods
        //Pbkdf2
        //Aarogn
        //BCrypt

        public string GenerateSalt()
        {
            byte[] randomBytes = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return Convert.ToBase64String(randomBytes);
        }

        //this method will hash the password with salt into HashedPassword
        public string HashPassword(string password, string salt)
        {
            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                                                     password: password,
                                                                     salt: Convert.FromBase64String(salt),
                                                                     prf: KeyDerivationPrf.HMACSHA512,
                                                                     iterationCount: 10000,
                                                                     numBytesRequested: 256 / 8));
            return hashed;
        }
    }
}
