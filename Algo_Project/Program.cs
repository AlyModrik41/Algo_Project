using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;




namespace RSA
{
    public class BigIntegerRSA
    {
        // Convert string representation to a List<int> representation
        public static List<int> StringToDigits(string number)//O(N)
        {
            return number.Select(c => (int)char.GetNumericValue(c)).ToList();
        }

        // Convert List<int> representation to string
        public static string DigitsToString(List<int> digits)//O(N)
        {
            return string.Concat(digits.Select(d => d.ToString()));
        }

        // Remove leading zeros
        public static List<int> RemoveLeadingZeros(List<int> number)//O(N)
        {
            if (number == null || number.Count == 0)//O(1)
                return new List<int> { 0 };//O(1)

            int i = 0; //O(1)
            while (i < number.Count - 1 && number[i] == 0) //O(N)
                i++;//O(1)

            return i > 0 ? number.GetRange(i, number.Count - i) : number;//O(1)
        }

        // Compare two big integers (1 if a > b, 0 if a == b, -1 if a < b)
        public static int Compare(List<int> a, List<int> b)//O(N)
        {
            a = RemoveLeadingZeros(a);//O(1)
            b = RemoveLeadingZeros(b);//O(1)

            if (a.Count != b.Count)//O(1)
                return a.Count.CompareTo(b.Count);//O(1)

            for (int i = 0; i < a.Count; i++)//O(N)
            {
                if (a[i] != b[i])//O(1)
                    return a[i].CompareTo(b[i]);//O(1)
            }

            return 0;//O(1)
        }

        // Addition with proper carrying
        public static List<int> Addition(List<int> vector1, List<int> vector2)//O(N)
        {

            List<int> v1 = new List<int>(vector1);//O(1)
            List<int> v2 = new List<int>(vector2);//O(1)


            while (v1.Count < v2.Count) v1.Insert(0, 0);//O(n)
            while (v2.Count < v1.Count) v2.Insert(0, 0); //O(n)

            List<int> sum = new List<int>(new int[v1.Count]);//O(1)
            int carry = 0;//O(1)

            for (int i = v1.Count - 1; i >= 0; i--)//O(N)
            {
                int tempSum = v1[i] + v2[i] + carry;//O(1)

                if (tempSum >= 10)//O(1)
                {
                    sum[i] = tempSum - 10;//O(1)
                    carry = 1;//O(1)
                }
                else//O(1)
                {
                    sum[i] = tempSum;//O(1)
                    carry = 0;//O(1)
                }
            }

            if (carry > 0)//O(1)
            {
                sum.Insert(0, carry);//O(1)
            }

            return sum;//O(1)
        }



        public static List<int> Subtraction(List<int> vector1, List<int> vector2)//O(N)
        {

            List<int> v1 = new List<int>(vector1);//O(1)
            List<int> v2 = new List<int>(vector2);//O(1)


            while (v1.Count < v2.Count) v1.Insert(0, 0);//O(n)
            while (v2.Count < v1.Count) v2.Insert(0, 0);//O(n)

            bool isNegative = false;//O(1)


            for (int i = 0; i < v1.Count; i++)//O(n)
            {
                if (v1[i] < v2[i])//O(1)
                {
                    isNegative = true;//O(1)
                    break;//O(1)
                }
                else if (v1[i] > v2[i])//O(1)
                {
                    break;//O(1)
                }
            }

            if (isNegative)//O(1)
            {
                List<int> temp = v1;//O(1)
                v1 = v2;//O(1)
                v2 = temp;//O(1)
            }


            List<int> result = new List<int>(new int[v1.Count]);//O(1)


            for (int i = v1.Count - 1; i >= 0; i--)//O(n)
            {
                if (v1[i] < v2[i])//O(1)
                {
                    v1[i] += 10;//O(1)
                    v1[i - 1]--;//O(1)
                }
                result[i] = v1[i] - v2[i];//O(1)
            }

            while (result.Count > 1 && result[0] == 0)//O(N)
            {
                result.RemoveAt(0);//O(1)
            }


            if (isNegative)//O(1)
            {
                result[0] *= -1;//O(1)
            }

            return result;//O(1)
        }




        public static List<int> Multiply(List<int> vector1, List<int> vector2)//O(N^1.585)
        {
            // Convert vectors to strings (remove decimal points if present)
            string num1 = string.Concat(vector1.Select(d => ((int)d).ToString()));//O(N)
            string num2 = string.Concat(vector2.Select(d => ((int)d).ToString()));//O(N)

            // Remove leading zeros
            num1 = num1.TrimStart('0');//O(N)
            num2 = num2.TrimStart('0');//O(N)

            // Handle edge cases
            if (string.IsNullOrEmpty(num1)) num1 = "0";//O(1)
            if (string.IsNullOrEmpty(num2)) num2 = "0";//O(1)
            if (num1 == "0" || num2 == "0")//O(1)
                return new List<int> { 0 };//O(1)

            // Make sure both numbers have the same length by padding with leading zeros
            int maxLength = Math.Max(num1.Length, num2.Length);//O(1)
            num1 = num1.PadLeft(maxLength, '0');//O(N)
            num2 = num2.PadLeft(maxLength, '0');//O(N)

            // Base case: if numbers are single-digit, multiply directly
            if (maxLength == 1)//O(1)
            {
                int product = int.Parse(num1) * int.Parse(num2);//O(1)
                return product.ToString().Select(c => (int)char.GetNumericValue(c)).ToList();//O(1)
            }

            // Split position (half the length)
            int splitPos = maxLength / 2;//O(1)

            // Split each number into high and low parts
            string high1 = num1.Substring(0, num1.Length - splitPos);//O(N)
            string low1 = num1.Substring(num1.Length - splitPos);//O(N)
            string high2 = num2.Substring(0, num2.Length - splitPos);//O(N)
            string low2 = num2.Substring(num2.Length - splitPos);//O(N)

            // Convert string parts back to List<double> for helper functions
            List<int> high1List = high1.Select(c => (int)char.GetNumericValue(c)).ToList();//O(N)
            List<int> low1List = low1.Select(c => (int)char.GetNumericValue(c)).ToList();//O(N)
            List<int> high2List = high2.Select(c => (int)char.GetNumericValue(c)).ToList();//O(N)
            List<int> low2List = low2.Select(c => (int)char.GetNumericValue(c)).ToList();//O(N)

            // Recursive multiplications (Karatsuba steps)
            List<int> z0 = Multiply(low1List, low2List);//O(N/2)
            List<int> z2 = Multiply(high1List, high2List);//O(N/2)

            // Compute (high1 + low1) and (high2 + low2)
            List<int> sumHighLow1 = Addition(high1List, low1List);//O(N)
            List<int> sumHighLow2 = Addition(high2List, low2List);//O(N)

            // Compute z1 = (high1 + low1) × (high2 + low2)
            List<int> z1 = Multiply(sumHighLow1, sumHighLow2);//O(N/2)

            // Calculate intermediate term: (z1 - z2 - z0)
            List<int> temp = Subtraction(Subtraction(z1, z2), z0);//O(N)

            // Combine results: z2 * 10^(2*splitPos) + temp * 10^splitPos + z0
            List<int> term1 = ShiftLeft(z2, 2 * splitPos);//O(N)
            List<int> term2 = ShiftLeft(temp, splitPos);//O(N)
            List<int> result = Addition(Addition(term1, term2), z0);//O(N)

            return result;//O(1)
        }
        // Helper function
        private static List<int> ShiftLeft(List<int> number, int positions)//O(N)
        {
            if (number.Count == 1 && number[0] == 0)//O(1)
                return new List<int> { 0 };//O(1)

            List<int> result = new List<int>(number);//O(1)
            for (int i = 0; i < positions; i++) //O(N)
            {
                result.Add(0);//O(1)
            }
            return result;//O(1)
        }




        public static string CheckEven_Odd(List<int> Vector)//O(1)
        {

            int LastDigit = Vector[Vector.Count - 1];//O(1)
            if (LastDigit % 2 == 0)//O(1)
            {
                return "Even";//O(1)
            }
            else//O(1)
            {
                return "Odd";//O(1)
            }
        }


        // Division and remainder calculation
        public static (List<int> quotient, List<int> remainder) Divide(List<int> dividend, List<int> divisor)//O(N^2)
        {
            // Handle special cases
            if (divisor.Count == 1 && divisor[0] == 0)//O(1)
                throw new DivideByZeroException();//O(1)

            dividend = RemoveLeadingZeros(dividend);//O(N)
            divisor = RemoveLeadingZeros(divisor);//O(N)

            // If dividend < divisor, quotient = 0, remainder = dividend
            if (Compare(dividend, divisor) < 0)//O(N)
                return (new List<int> { 0 }, dividend);//O(1)

            // Digit-by-digit long division
            List<int> quotient = new List<int>();//O(1)
            List<int> remainder = new List<int>();//O(1)

            foreach (int digit in dividend)//O(N^3)
            {
                // Bring down next digit
                remainder.Add(digit);//O(1)
                remainder = RemoveLeadingZeros(remainder);//O(N)

                // Find how many times divisor goes into remainder
                int q = 0;//O(1)
                while (Compare(remainder, divisor) >= 0) //O(N^2)
                {
                    remainder = Subtraction(remainder, divisor); //O(N)
                    q++;//O(1)
                }

                quotient.Add(q);//O(1)
            }

            return (RemoveLeadingZeros(quotient), RemoveLeadingZeros(remainder));//O(1)
        }

        // Calculate a^b mod m using binary exponentiation
        public static List<int> ModPow(List<int> baseNum, List<int> exponent, List<int> modulus)//O(N^4)
        {
            // Handle special cases
            if (exponent.Count == 1 && exponent[0] == 0)//O(1)
                return new List<int> { 1 };  // x^0 = 1 //O(1)

            // Working with copies to avoid modifying originals
            baseNum = new List<int>(baseNum);//O(1)
            exponent = new List<int>(exponent);//O(1)

            // Initialize result to 1
            List<int> result = new List<int> { 1 };//O(1)

            // Reduce base modulo m
            baseNum = Divide(baseNum, modulus).remainder;//O(N^3)

            // Fixed binary exponentiation algorithm
            // Note: We'll use string representation for binary to handle large exponents better
            string binaryExponent = "";//O(1)
            List<int> tempExp = new List<int>(exponent);//O(1)

            // Convert exponent to binary form
            while (tempExp.Count > 1 || tempExp[0] > 0)//O(N^4)
            {
                var divResult = Divide(tempExp, new List<int> { 2 });//O(N^3)
                binaryExponent = divResult.remainder[0] + binaryExponent;//O(1)
                tempExp = divResult.quotient;//O(1)
            }

            // Square and multiply algorithm
            foreach (char bit in binaryExponent)//O(N^3)
            {
                // Square
                result = Divide(Multiply(result, result), modulus).remainder;//O(N^3)

                // Multiply if bit is 1
                if (bit == '1')//O(1)
                {
                    result = Divide(Multiply(result, baseNum), modulus).remainder;//O(N^3)
                }
            }

            return RemoveLeadingZeros(result);//O(N)
        }

        // RSA Encryption: c = m^e mod n
        public static List<int> Encrypt(List<int> message, List<int> e, List<int> n)//O(N^4)
        {
            return ModPow(message, e, n);
        }

        // RSA Decryption: m = c^d mod n
        public static List<int> Decrypt(List<int> ciphertext, List<int> d, List<int> n)//O(N^4)
        {
            return ModPow(ciphertext, d, n);
        }
        public static string EncryptString(string message, List<int> e, List<int> n)//O(N^5)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(message);//O(1)
            List<byte> encryptedBytes = new List<byte>();//O(1)

            foreach (byte b in bytes)//O(N^5)
            {
                List<int> chunkList = new List<int> { b };//O(1)
                List<int> encryptedChunk = ModPow(chunkList, e, n);//O(N^4)
                // Convert the encrypted number to int (should fit for small n)
                int encryptedInt = int.Parse(string.Concat(encryptedChunk));//O(1)
                encryptedBytes.AddRange(BitConverter.GetBytes(encryptedInt));//O(1)
            }

            return Convert.ToBase64String(encryptedBytes.ToArray());//O(1)
        }

        public static string DecryptString(string ciphertext, List<int> d, List<int> n)//O(N^5)
        {
            byte[] encryptedBytes = Convert.FromBase64String(ciphertext);//O(1)
            List<byte> decryptedBytes = new List<byte>();//O(1)

            for (int i = 0; i < encryptedBytes.Length; i += 4)//O(N^5)
            {
                int encryptedInt = BitConverter.ToInt32(encryptedBytes, i);//O(1)
                List<int> encryptedChunk = encryptedInt.ToString().Select(c => (int)char.GetNumericValue(c)).ToList();//O(1)
                List<int> decryptedChunk = ModPow(encryptedChunk, d, n);//O(N^4)
                // Should result in a single byte
                decryptedBytes.Add((byte)int.Parse(string.Concat(decryptedChunk)));//O(1)
            }

            return Encoding.UTF8.GetString(decryptedBytes.ToArray());//O(1)

        }
        
    }

    class Program
    {
        //static void Main()
        //{
        //    // RSA parameters (as in your example)
        //    List<int> p = new List<int> { 6, 1 }; // 61
        //    List<int> q = new List<int> { 5, 3 }; // 53
        //    List<int> n = BigIntegerRSA.Multiply(p, q); // 3233
        //    List<int> e = new List<int> { 1, 7 }; // 17
        //    List<int> d = new List<int> { 2, 7, 5, 3 }; // 2753

        //    Console.Write("Enter number of test cases: ");
        //    if (!int.TryParse(Console.ReadLine(), out int testCases) || testCases < 1)
        //    {
        //        Console.WriteLine("Invalid number of test cases.");
        //        return;
        //    }

        //    for (int i = 1; i <= testCases; i++)
        //    {
        //        Console.WriteLine($"\nTest case {i}:");
        //        Console.Write("Enter message: ");
        //        string message = Console.ReadLine();

        //        // Encryption timing
        //        Stopwatch swEncrypt = Stopwatch.StartNew();
        //        string encrypted = BigIntegerRSA.EncryptString(message, e, n);
        //        swEncrypt.Stop();

        //        // Decryption timing
        //        Stopwatch swDecrypt = Stopwatch.StartNew();
        //        string decrypted = BigIntegerRSA.DecryptString(encrypted, d, n);
        //        swDecrypt.Stop();

        //        Console.WriteLine($"Encrypted: {encrypted}");
        //        Console.WriteLine($"Decrypted: {decrypted}");
        //        Console.WriteLine($"Encryption Time: {swEncrypt.Elapsed.TotalMilliseconds} ms");
        //        Console.WriteLine($"Decryption Time: {swDecrypt.Elapsed.TotalMilliseconds} ms");

        //        if (decrypted == message)
        //            Console.WriteLine("Result: Success! Decrypted message matches the original.");
        //        else
        //            Console.WriteLine("Result: Failure! Decrypted message does NOT match the original.");
        //    }

        //    Console.WriteLine("\nPress any key to exit...");
        //    Console.ReadKey();
        //    Console.ReadLine();
        //}
        static void Main(string[] args)
        {
            Console.WriteLine("===== Fixed RSA Implementation =====");
            Console.Write("Enter number of test cases: ");
            int numTestCases = int.Parse(Console.ReadLine());


            for (int t = 0; t < numTestCases; t++)
            {
                Console.WriteLine($"\nTest Case {t + 1}:");
                Console.WriteLine("------------------");

                try
                {
                    // Get modulus n
                    Console.Write("Write the n: ");
                    string n = Console.ReadLine();


                    // Get key (e/d)
                    Console.Write("Write the e/d: ");
                    string key = Console.ReadLine();

                    // Get message
                    Console.Write("Write the Message: ");
                    string message = Console.ReadLine();


                    // Get operation type
                    Console.Write("Write the Operation (0 for encryption, 1 for decryption): ");
                    int operationType = int.Parse(Console.ReadLine());

                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    var time_before = System.Environment.TickCount;

                    List<int> result;
                    if (operationType == 0)
                    {
                        result = BigIntegerRSA.Encrypt(BigIntegerRSA.StringToDigits(message), BigIntegerRSA.StringToDigits(key), BigIntegerRSA.StringToDigits(n));
                        Console.WriteLine("Encrypting...");
                    }
                    else
                    {
                        result = BigIntegerRSA.Decrypt(BigIntegerRSA.StringToDigits(message), BigIntegerRSA.StringToDigits(key), BigIntegerRSA.StringToDigits(n));
                        Console.WriteLine("Decrypting...");
                    }

                    stopwatch.Stop();
                    var time_after = System.Environment.TickCount;
                    Console.WriteLine("Time Taken: " + (time_after - time_before) + " ms");

                    Console.Write("Output: ");
                    Console.WriteLine(string.Join("", result));
                    Console.WriteLine($"Time Taken: {stopwatch.Elapsed.TotalMilliseconds} ms");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}