using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Ocsp;
using Wiry.Base32;

namespace Blockpasstest.Custom
{
    public class WebAssemblyRuntime
    {
        private static Random random = new Random();

        private string Multiply(string a, int b)
        {
            BigInteger bi = BigInteger.Parse(a);
            BigInteger r = new BigInteger(b);
            BigInteger an = BigInteger.Multiply(bi, r);
            return an.ToString();
        }

        private string Devide(string a, int b)
        {
            BigInteger bi = BigInteger.Parse(a);
            BigInteger r = new BigInteger(b);
            BigInteger an = BigInteger.Divide(bi, r);
            return an.ToString();
        }

        private string SimpleDevide(int i)
        {
            int[] ranArr = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int r = ranArr[random.Next(8)];
            int ma = i * r;
            return ma.ToString() + r.ToString();
        }

        private string SimpleMul(int i, int r)
        {
            int ma = i / r;
            return ma.ToString();
        }

        public (List<string>, string) ArrayDivide(List<string> arr)
        {
            List<int> ranArr = new List<int> { 2, 3, 4, 5, 6, 7, 8, 9 };
            int a = ranArr[random.Next(0, 8)];

            List<string> newIntArr = new List<string>(arr.Count);
            foreach (string item in arr)
            {
                string mun = Multiply(item, a);
                newIntArr.Add(mun);
            }

            return (newIntArr, a.ToString());
        }


        private List<string> ArrMultiply(List<string> arr, int r)
        {
            List<string> newIntArr = new List<string>(arr.Count);
            for (int i = 0; i < arr.Count; i++)
            {
                string d = Devide(arr[i], r);
                newIntArr.Add(d);
            }

            return newIntArr;
        }

        private string FormatOutput(bool startWithNumber, List<string> numbers, List<string> letters, string ending)
        {
            StringBuilder resultBuilder = new StringBuilder();

            int numIndex = 0;
            int letterIndex = 0;

            // Собираем строку, начиная с числа или буквы в зависимости от значения startWithNumber
            while (numIndex < numbers.Count || letterIndex < letters.Count)
            {
                if (startWithNumber)
                {
                    if (numIndex < numbers.Count)
                    {
                        resultBuilder.Append(numbers[numIndex]);
                        numIndex++;
                    }
                    if (letterIndex < letters.Count)
                    {
                        resultBuilder.Append(letters[letterIndex]);
                        letterIndex++;
                    }
                }
                else
                {
                    if (letterIndex < letters.Count)
                    {
                        resultBuilder.Append(letters[letterIndex]);
                        letterIndex++;
                    }
                    if (numIndex < numbers.Count)
                    {
                        resultBuilder.Append(numbers[numIndex]);
                        numIndex++;
                    }
                }
            }

            return resultBuilder.ToString() + ending;
        }

        private string FormatInput(bool startWithNumber, List<string> numbers, List<string> letters)
        {
            StringBuilder resultBuilder = new StringBuilder();

            int numIndex = 0;
            int letterIndex = 0;

            // Собираем строку, начиная с числа или буквы в зависимости от значения startWithNumber
            while (numIndex < numbers.Count || letterIndex < letters.Count)
            {
                if (startWithNumber)
                {
                    if (numIndex < numbers.Count)
                    {
                        resultBuilder.Append(numbers[numIndex]);
                        numIndex++;
                    }
                    if (letterIndex < letters.Count)
                    {
                        resultBuilder.Append(letters[letterIndex]);
                        letterIndex++;
                    }
                }
                else
                {
                    if (letterIndex < letters.Count)
                    {
                        resultBuilder.Append(letters[letterIndex]);
                        letterIndex++;
                    }
                    if (numIndex < numbers.Count)
                    {
                        resultBuilder.Append(numbers[numIndex]);
                        numIndex++;
                    }
                }
            }

            return resultBuilder.ToString();
        }

        private string EncodeToBase(string lett)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(lett);
            string base32 = Base32Encoding.Standard.GetString(bytes);
            //string base32 = Base32Encode(bytes);
            return base32.ToLowerInvariant();
        }

        private string DecodeFromBase(string ba)
        {
            string up = ba.ToUpperInvariant();
            byte[] bytes = Base32Encoding.Standard.ToBytes(up);
            return Encoding.UTF8.GetString(bytes);
        }

        private static string MakeDownCase(string str)
        {
            char[] charArray = str.ToCharArray();
            for (int i = 0; i < charArray.Length / 2; i++)
            {
                int ra = random.Next(0, charArray.Length);
                if (i == ra)
                {
                    charArray[i] = char.ToLower(charArray[i]);
                }
            }
            return new string(charArray);
        }

        private string MakeUpperCase(string str)
        {
            StringBuilder builder = new StringBuilder();
            foreach (char c in str)
            {
                builder.Append(char.ToUpper(c));
            }
            return builder.ToString();
        }

        private string EncodeToThree(string str)
        {
            string layerHex = BitConverter.ToString(Encoding.UTF8.GetBytes(str)).Replace("-", "");
            byte[] ans = new byte[layerHex.Length];

            for (int i = 0; i < layerHex.Length; i++)
            {
                ans[i] = (byte)(layerHex[i] + 13);
            }

            return Encoding.UTF8.GetString(ans);
        }

        private string DecodeFromThree(string str)
        {
            byte[] ans = new byte[str.Length];

            for (int i = 0; i < str.Length; i++)
            {
                ans[i] = (byte)(str[i] - 13);
            }

            try
            {
                string layerBase = Encoding.UTF8.GetString(HexToBytes(Encoding.UTF8.GetString(ans)));
                return layerBase;
            }
            catch (Exception)
            {
                Console.Write("First decode layer corrupted");
                return "First decode layer corrupted";
            }
        }

        private byte[] HexToBytes(string hex)
        {
            if (hex.Length % 2 != 0)
            {
                Console.Write("Invalid hex string");
            }

            int byteCount = hex.Length / 2;
            byte[] bytes = new byte[byteCount];

            for (int i = 0; i < byteCount; i++)
            {
                string byteValue = hex.Substring(i * 2, 2);
                bytes[i] = Convert.ToByte(byteValue, 16);
            }

            return bytes;
        }

        private static (List<string>, List<string>, bool) EncodeSlice(string input)
        {
            List<string> numbers = new List<string>();
            List<string> letters = new List<string>();
            bool startWithNumber = false;

            StringBuilder numberBuilder = new StringBuilder();
            StringBuilder letterBuilder = new StringBuilder();

            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];

                if (Char.IsDigit(c))
                {
                    if (letterBuilder.Length > 0)
                    {
                        letters.Add(letterBuilder.ToString());
                        letterBuilder.Clear();
                    }

                    numberBuilder.Append(c);
                }
                else
                {
                    if (numberBuilder.Length > 0)
                    {
                        numbers.Add(numberBuilder.ToString());
                        numberBuilder.Clear();
                    }

                    letterBuilder.Append(c);
                }

                // Проверка первого символа
                if (i == 0)
                {
                    startWithNumber = Char.IsDigit(c);
                }

                // Проверка последнего символа
                if (i == input.Length - 1)
                {
                    if (numberBuilder.Length > 0)
                    {
                        numbers.Add(numberBuilder.ToString());
                    }
                    else if (letterBuilder.Length > 0)
                    {
                        letters.Add(letterBuilder.ToString());
                    }
                }
            }

            return (numbers, letters, startWithNumber);
        }

        private byte[] HexDecode(string hex)
        {
            byte[] raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return raw;
        }

        public static IEnumerable<string> SplitIntoGroups(string input, int groupSize)
        {
            for (int i = 0; i < input.Length; i += groupSize)
            {
                yield return input.Substring(i, Math.Min(groupSize, input.Length - i));
            }
        }

        public string Encode(string test)
        {
            string res = "";
            byte[] b = Encoding.UTF8.GetBytes(test);
            string strHex = BitConverter.ToString(b).Replace("-", "").ToLowerInvariant();

            if (int.TryParse(strHex, out int num))
            {
                res = SimpleDevide(num);
            }
            else
            {
                (List<string> numbers, List<string> letters, bool letterIsStart) = EncodeSlice(strHex);
                (List<string> numStr, string r) = ArrayDivide(numbers);
                res = FormatOutput(letterIsStart, numStr, letters, r);
            }

            string layerTwo = EncodeToBase(res);
            string layerThree = EncodeToThree(layerTwo);

            return layerThree;
        }

        public string Decode(string code)
        {
            string layerThree = DecodeFromThree(code);
            string he = DecodeFromBase(layerThree);

            if (he.Length == 0) { return ""; }

            int r = int.Parse(he.Substring(he.Length - 1));
            string slice = he.Substring(0, he.Length - 1);
            if (int.TryParse(slice, out int n))
            {
                string h = SimpleMul(n, r);
                byte[] ans = HexDecode(h.ToUpperInvariant());
                return Encoding.UTF8.GetString(ans);
            }
            else
            {                    
                (List<string> numbers, List<string> letters, bool isStart) = EncodeSlice(slice);
                List<string> numStr = ArrMultiply(numbers, r);
                string h = FormatInput(isStart, numStr, letters);
                byte[] ans = HexDecode(h.ToUpperInvariant());
                return Encoding.UTF8.GetString(ans);
            }
        }
    }
}

