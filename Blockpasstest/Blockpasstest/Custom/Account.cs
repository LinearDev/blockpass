using System;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System.Text;
using System.Linq;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Util;
using Nethereum.Web3.Accounts;
using Nethereum.Signer;

namespace Blockpasstest.Custom
{
	public class Account
	{
        public struct User
        {
            public string publicKey;
            public string privateKey;
            public string address;
        }
		private static Random random = new Random();

        private static string BytesToHex(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
        }

        string GetLast40Characters(string publicKeyHex)
        {
            // Преобразование publicKeyHex в байтовый массив
            byte[] publicKeyBytes = publicKeyHex.HexToByteArray();

            // Вычисление Keccak-256 хэша
            byte[] keccakHash = new Sha3Keccack().CalculateHash(publicKeyBytes);

            // Преобразование хэша обратно в строку
            string keccakHashString = keccakHash.ToHex();

            // Взятие последних 40 символов хэша
            string last40Characters = keccakHashString.Substring(keccakHashString.Length - 40);

            return last40Characters;
        }

        public User Create(string word)
        {
            // String to Byte Array
            byte[] wordBytes = Encoding.UTF8.GetBytes(word);

            // Generate a new private key
            var ecKey = EthECKey.GenerateKey(wordBytes);

            // Convert private key to hexadecimal string
            var privateKey = ecKey.GetPrivateKeyAsBytes().ToHex();

            // Derive public key from private key
            var publicKey = ecKey.GetPubKey().ToHex();

            // Compute Ethereum address from public key
            var address = new AddressUtil().ConvertToChecksumAddress(ecKey.GetPublicAddress());

            var user = new User();
            user.privateKey = "0x" + privateKey;
            user.publicKey = "0x" + publicKey;
            user.address = address;
            return user;
        }

        public User CreateSpesificAccount(string privateKeyHex)
        {
            //var account = new Nethereum.Web3.Accounts.Account(privateKeyHex.Substring(2));
            //string publicKeyHex = account.Address;
            //string publicKeyHex = "0x" + CreatePublicFromPrivate(privateKeyHex.Substring(2));
            //publicKeyHex = "0x" + GetLast40Characters(publicKeyHex);
            //publicKeyHex = new AddressUtil().ConvertToChecksumAddress(publicKeyHex);

            // Convert the private key from hexadecimal string to byte array
            var privateKeyBytes = privateKeyHex.Substring(2).HexToByteArray();

            // Create an instance of EthECKey using the private key bytes
            var ecKey = new EthECKey(privateKeyBytes, true);

            // Derive the public key from the private key
            var publicKey = ecKey.GetPubKey().ToHex();

            // Compute the Ethereum address from the public key
            var address = new AddressUtil().ConvertToChecksumAddress(ecKey.GetPublicAddress());

            var user = new User();
            user.privateKey = privateKeyHex;
            user.publicKey = "0x" + publicKey;
            user.address = address;
            return user;
        }

        private string CreatePublicFromPrivate(string priv)
        {
            SHA256 sha256 = SHA256.Create();

            byte[] publicKeyBytes = Encoding.UTF8.GetBytes(priv);
            byte[] hashBytes = sha256.ComputeHash(publicKeyBytes);
            string hashedHex = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            string publicKeyHex = hashedHex.Substring(0, 64);
            return publicKeyHex;
        }
	}
}

