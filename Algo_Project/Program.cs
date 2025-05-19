using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace FixedRSA
{
    public class BigIntegerRSA
    {
        // Convert string representation to a List<int> representation
        public static List<int> StringToDigits(string number)
        {
            return number.Select(c => (int)char.GetNumericValue(c)).ToList();
        }

        // Convert List<int> representation to string
        public static string DigitsToString(List<int> digits)
        {
            return string.Concat(digits.Select(d => d.ToString()));
        }

        // Remove leading zeros
        public static List<int> RemoveLeadingZeros(List<int> number)
        {
            if (number == null || number.Count == 0)
                return new List<int> { 0 };

            int i = 0;
            while (i < number.Count - 1 && number[i] == 0)
                i++;

            return i > 0 ? number.GetRange(i, number.Count - i) : number;
        }

        // Compare two big integers (1 if a > b, 0 if a == b, -1 if a < b)
        public static int Compare(List<int> a, List<int> b)
        {
            a = RemoveLeadingZeros(a);
            b = RemoveLeadingZeros(b);

            if (a.Count != b.Count)
                return a.Count.CompareTo(b.Count);

            for (int i = 0; i < a.Count; i++)
            {
                if (a[i] != b[i])
                    return a[i].CompareTo(b[i]);
            }

            return 0;
        }

        // Addition with proper carrying
        public static List<int> Addition(List<int> vector1, List<int> vector2)
        {

            List<int> v1 = new List<int>(vector1);
            List<int> v2 = new List<int>(vector2);


            while (v1.Count < v2.Count) v1.Insert(0, 0);
            while (v2.Count < v1.Count) v2.Insert(0, 0);

            List<int> sum = new List<int>(new int[v1.Count]);
            int carry = 0;

            for (int i = v1.Count - 1; i >= 0; i--)
            {
                int tempSum = v1[i] + v2[i] + carry;

                if (tempSum >= 10)
                {
                    sum[i] = tempSum - 10;
                    carry = 1;
                }
                else
                {
                    sum[i] = tempSum;
                    carry = 0;
                }
            }

            if (carry > 0)
            {
                sum.Insert(0, carry);
            }

            return sum;
        }



        public static List<int> Subtraction(List<int> vector1, List<int> vector2)
        {

            List<int> v1 = new List<int>(vector1);
            List<int> v2 = new List<int>(vector2);


            while (v1.Count < v2.Count) v1.Insert(0, 0);
            while (v2.Count < v1.Count) v2.Insert(0, 0);

            bool isNegative = false;


            for (int i = 0; i < v1.Count; i++)
            {
                if (v1[i] < v2[i])
                {
                    isNegative = true;
                    break;
                }
                else if (v1[i] > v2[i])
                {
                    break;
                }
            }

            if (isNegative)
            {
                List<int> temp = v1;
                v1 = v2;
                v2 = temp;
            }


            List<int> result = new List<int>(new int[v1.Count]);


            for (int i = v1.Count - 1; i >= 0; i--)
            {
                if (v1[i] < v2[i])
                {
                    v1[i] += 10;
                    v1[i - 1]--;
                }
                result[i] = v1[i] - v2[i];
            }

            while (result.Count > 1 && result[0] == 0)
            {
                result.RemoveAt(0);
            }


            if (isNegative)
            {
                result[0] *= -1;
            }

            return result;
        }




        public static List<int> Multiply(List<int> vector1, List<int> vector2)
        {
            // Convert vectors to strings (remove decimal points if present)
            string num1 = string.Concat(vector1.Select(d => ((int)d).ToString()));
            string num2 = string.Concat(vector2.Select(d => ((int)d).ToString()));

            // Remove leading zeros
            num1 = num1.TrimStart('0');
            num2 = num2.TrimStart('0');

            // Handle edge cases
            if (string.IsNullOrEmpty(num1)) num1 = "0";
            if (string.IsNullOrEmpty(num2)) num2 = "0";
            if (num1 == "0" || num2 == "0")
                return new List<int> { 0 };

            // Make sure both numbers have the same length by padding with leading zeros
            int maxLength = Math.Max(num1.Length, num2.Length);
            num1 = num1.PadLeft(maxLength, '0');
            num2 = num2.PadLeft(maxLength, '0');

            // Base case: if numbers are single-digit, multiply directly
            if (maxLength == 1)
            {
                int product = int.Parse(num1) * int.Parse(num2);
                return product.ToString().Select(c => (int)char.GetNumericValue(c)).ToList();
            }

            // Split position (half the length)
            int splitPos = maxLength / 2;

            // Split each number into high and low parts
            string high1 = num1.Substring(0, num1.Length - splitPos);
            string low1 = num1.Substring(num1.Length - splitPos);
            string high2 = num2.Substring(0, num2.Length - splitPos);
            string low2 = num2.Substring(num2.Length - splitPos);

            // Convert string parts back to List<double> for helper functions
            List<int> high1List = high1.Select(c => (int)char.GetNumericValue(c)).ToList();
            List<int> low1List = low1.Select(c => (int)char.GetNumericValue(c)).ToList();
            List<int> high2List = high2.Select(c => (int)char.GetNumericValue(c)).ToList();
            List<int> low2List = low2.Select(c => (int)char.GetNumericValue(c)).ToList();

            // Recursive multiplications (Karatsuba steps)
            List<int> z0 = Multiply(low1List, low2List);
            List<int> z2 = Multiply(high1List, high2List);

            // Compute (high1 + low1) and (high2 + low2)
            List<int> sumHighLow1 = Addition(high1List, low1List);
            List<int> sumHighLow2 = Addition(high2List, low2List);

            // Compute z1 = (high1 + low1) × (high2 + low2)
            List<int> z1 = Multiply(sumHighLow1, sumHighLow2);

            // Calculate intermediate term: (z1 - z2 - z0)
            List<int> temp = Subtraction(Subtraction(z1, z2), z0);

            // Combine results: z2 * 10^(2*splitPos) + temp * 10^splitPos + z0
            List<int> term1 = ShiftLeft(z2, 2 * splitPos);
            List<int> term2 = ShiftLeft(temp, splitPos);
            List<int> result = Addition(Addition(term1, term2), z0);

            return result;
        }
        // Helper function
        private static List<int> ShiftLeft(List<int> number, int positions)
        {
            if (number.Count == 1 && number[0] == 0)
                return new List<int> { 0 };

            List<int> result = new List<int>(number);
            for (int i = 0; i < positions; i++)
            {
                result.Add(0);
            }
            return result;
        }




        public static string CheckEven_Odd(List<int> Vector)
        {

            int LastDigit = Vector[Vector.Count - 1];
            if (LastDigit % 2 == 0)
            {
                return "Even";
            }
            else
            {
                return "Odd";
            }
        }


        // Division and remainder calculation
        public static (List<int> quotient, List<int> remainder) Divide(List<int> dividend, List<int> divisor)
        {
            // Handle special cases
            if (divisor.Count == 1 && divisor[0] == 0)
                throw new DivideByZeroException();

            dividend = RemoveLeadingZeros(dividend);
            divisor = RemoveLeadingZeros(divisor);

            // If dividend < divisor, quotient = 0, remainder = dividend
            if (Compare(dividend, divisor) < 0)
                return (new List<int> { 0 }, dividend);

            // Digit-by-digit long division
            List<int> quotient = new List<int>();
            List<int> remainder = new List<int>();

            foreach (int digit in dividend)
            {
                // Bring down next digit
                remainder.Add(digit);
                remainder = RemoveLeadingZeros(remainder);

                // Find how many times divisor goes into remainder
                int q = 0;
                while (Compare(remainder, divisor) >= 0)
                {
                    remainder = Subtraction(remainder, divisor); 
                    q++;
                }

                quotient.Add(q);
            }

            return (RemoveLeadingZeros(quotient), RemoveLeadingZeros(remainder));
        }

        // Calculate a^b mod m using binary exponentiation
        public static List<int> ModPow(List<int> baseNum, List<int> exponent, List<int> modulus)
        {
            // Handle special cases
            if (exponent.Count == 1 && exponent[0] == 0)
                return new List<int> { 1 };  // x^0 = 1

            // Working with copies to avoid modifying originals
            baseNum = new List<int>(baseNum);
            exponent = new List<int>(exponent);

            // Initialize result to 1
            List<int> result = new List<int> { 1 };

            // Reduce base modulo m
            baseNum = Divide(baseNum, modulus).remainder;

            // Fixed binary exponentiation algorithm
            // Note: We'll use string representation for binary to handle large exponents better
            string binaryExponent = "";
            List<int> tempExp = new List<int>(exponent);

            // Convert exponent to binary form
            while (tempExp.Count > 1 || tempExp[0] > 0)
            {
                var divResult = Divide(tempExp, new List<int> { 2 });
                binaryExponent = divResult.remainder[0] + binaryExponent;
                tempExp = divResult.quotient;
            }

            // Square and multiply algorithm
            foreach (char bit in binaryExponent)
            {
                // Square
                result = Divide(Multiply(result, result), modulus).remainder;

                // Multiply if bit is 1
                if (bit == '1')
                {
                    result = Divide(Multiply(result, baseNum), modulus).remainder;
                }
            }

            return RemoveLeadingZeros(result);
        }

        // RSA Encryption: c = m^e mod n
        public static List<int> Encrypt(List<int> message, List<int> e, List<int> n)
        {
            return ModPow(message, e, n);
        }

        // RSA Decryption: m = c^d mod n
        public static List<int> Decrypt(List<int> ciphertext, List<int> d, List<int> n)
        {
            return ModPow(ciphertext, d, n);
        }
    }

    class Program
    {
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
                    var time_before=System.Environment.TickCount;

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
                    Console.WriteLine("Time Taken: " + (time_after - time_before)+ " ms");

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