/**
 * Copyright (c) 2022-present, Colizeum Association
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the
 * License. You may obtain a copy of the License at
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "
 * AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific
 * language governing permissions and limitations under the License.
 */

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ColizeumSDK.Utils
{
    internal static class Cryptography
    {
        private static readonly byte[] Bytes =
            { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 };

        /// <summary>
        /// Encrypts the provided string value using the Colizeum encryption key
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Encrypted string</returns>
        public static string Encrypt(string value)
        {
            var clearBytes = Encoding.Unicode.GetBytes(value);

            using var encryptor = Aes.Create();

            var pdb = new Rfc2898DeriveBytes(Constants.EncryptionKey, Bytes);
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);

            using var ms = new MemoryStream();

            using (var cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(clearBytes, 0, clearBytes.Length);
                cs.Close();
            }

            return Convert.ToBase64String(ms.ToArray());
        }

        /// <summary>
        /// Tries to decrypt the encrypted string using the Colizeum encryption key
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Decrypted string</returns>
        public static string Decrypt(string value)
        {
            value = value.Replace(" ", "+");

            var cipherBytes = Convert.FromBase64String(value);

            using var encryptor = Aes.Create();
            var pdb = new Rfc2898DeriveBytes(Constants.EncryptionKey, Bytes);
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);

            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cs.Write(cipherBytes, 0, cipherBytes.Length);
                cs.Close();
            }

            return Encoding.Unicode.GetString(ms.ToArray());
        }
    }
}