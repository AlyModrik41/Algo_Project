using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Numerics;

namespace Algo_Project
{

    public class MyBigInteger
    {

        public List<double> Addition(List<double> vector1, List<double> vector2)
        {

            List<double> v1 = new List<double>(vector1);
            List<double> v2 = new List<double>(vector2);


            while (v1.Count < v2.Count) v1.Insert(0, 0);
            while (v2.Count < v1.Count) v2.Insert(0, 0);

            List<double> sum = new List<double>(new double[v1.Count]);
            double carry = 0;

            for (int i = v1.Count - 1; i >= 0; i--)
            {
                double tempSum = v1[i] + v2[i] + carry;

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



        public List<double> Subtraction(List<double> vector1, List<double> vector2)
        {

            List<double> v1 = new List<double>(vector1);
            List<double> v2 = new List<double>(vector2);


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
                List<double> temp = v1;
                v1 = v2;
                v2 = temp;
            }


            List<double> result = new List<double>(new double[v1.Count]);


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




        public List<double> Multiply(List<double> vector1, List<double> vector2)
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
                return new List<double> { 0 };

            // Make sure both numbers have the same length by padding with leading zeros
            int maxLength = Math.Max(num1.Length, num2.Length);
            num1 = num1.PadLeft(maxLength, '0');
            num2 = num2.PadLeft(maxLength, '0');

            // Base case: if numbers are single-digit, multiply directly
            if (maxLength == 1)
            {
                int product = int.Parse(num1) * int.Parse(num2);
                return product.ToString().Select(c => (double)char.GetNumericValue(c)).ToList();
            }

            // Split position (half the length)
            int splitPos = maxLength / 2;

            // Split each number into high and low parts
            string high1 = num1.Substring(0, num1.Length - splitPos);
            string low1 = num1.Substring(num1.Length - splitPos);
            string high2 = num2.Substring(0, num2.Length - splitPos);
            string low2 = num2.Substring(num2.Length - splitPos);

            // Convert string parts back to List<double> for helper functions
            List<double> high1List = high1.Select(c => (double)char.GetNumericValue(c)).ToList();
            List<double> low1List = low1.Select(c => (double)char.GetNumericValue(c)).ToList();
            List<double> high2List = high2.Select(c => (double)char.GetNumericValue(c)).ToList();
            List<double> low2List = low2.Select(c => (double)char.GetNumericValue(c)).ToList();

            // Recursive multiplications (Karatsuba steps)
            List<double> z0 = Multiply(low1List, low2List);
            List<double> z2 = Multiply(high1List, high2List);

            // Compute (high1 + low1) and (high2 + low2)
            List<double> sumHighLow1 = Addition(high1List, low1List);
            List<double> sumHighLow2 = Addition(high2List, low2List);

            // Compute z1 = (high1 + low1) × (high2 + low2)
            List<double> z1 = Multiply(sumHighLow1, sumHighLow2);

            // Calculate intermediate term: (z1 - z2 - z0)
            List<double> temp = Subtraction(Subtraction(z1, z2), z0);

            // Combine results: z2 * 10^(2*splitPos) + temp * 10^splitPos + z0
            List<double> term1 = ShiftLeft(z2, 2 * splitPos);
            List<double> term2 = ShiftLeft(temp, splitPos);
            List<double> result = Addition(Addition(term1, term2), z0);

            return result;
        }
        // Helper function
        private List<double> ShiftLeft(List<double> number, int positions)
        {
            if (number.Count == 1 && number[0] == 0)
                return new List<double> { 0 };

            List<double> result = new List<double>(number);
            for (int i = 0; i < positions; i++)
            {
                result.Add(0);
            }
            return result;
        }




        public string CheckEven_Odd(List<double> Vector)
        {
         
            double LastDigit = Vector[Vector.Count - 1] ;
            if (LastDigit % 2 == 0)
            {
                return "Even";
            }
            else
            {
                return "Odd"; 
            }
        }



        public  (List<double> quotient, List<double> remainder) Divide(List<double> a, List<double> b)
        {
            // Handle division by zero - O(1)
            if (b.Count == 1 && b[0] == 0) throw new DivideByZeroException();

            // Handle zero dividend - O(1)
            if (a.Count == 1 && a[0] == 0) return (new List<double> { 0 }, new List<double> { 0 });

            // Determine signs - O(1)
            bool aNegative = a[0] < 0;
            bool bNegative = b[0] < 0;
            bool quotientNegative = aNegative != bNegative;
            bool remainderNegative = aNegative;

            // Work with absolute values - O(N)
            List<double> absA = a.Select(Math.Abs).ToList();
            List<double> absB = b.Select(Math.Abs).ToList();

            // Base case - O(N) for Compare
            if (Compare(absA, absB) < 0)
            {
                var remainder = remainderNegative ? Negate(absA) : absA;
                return (new List<double> { 0 }, remainder);
            }

            // Recursive division - T(N/2)
            var twoB = Addition(absB, absB); // O(N)
            var (q, r) = Divide(absA, twoB);

            // Double quotient - O(N)
            q = Addition(q, q);

            // Adjust results - O(N) for Compare and Subtraction
            if (Compare(r, absB) < 0)
            {
                q = quotientNegative ? Negate(q) : q;
                r = remainderNegative ? Negate(r) : r;
                return (q, r);
            }
            else
            {
                var adjustedQ = Addition(q, new List<double> { 1 }); // O(N)
                var adjustedR = Subtraction(r, absB); // O(N)
                adjustedQ = quotientNegative ? Negate(adjustedQ) : adjustedQ;
                adjustedR = remainderNegative ? Negate(adjustedR) : adjustedR;
                return (adjustedQ, adjustedR);
            }
        }



        // Helper methods

        private static bool IsZero(List<double> num)
            => num.Count == 0 || (num.Count == 1 && num[0] == 0);

        private static List<double> AbsoluteValue(List<double> num)
            => num.Select(Math.Abs).ToList();

        private static List<double> Negate(List<double> num)
        {
            if (IsZero(num)) return new List<double> { 0 };
            var result = new List<double>(num);
            result[0] *= -1;
            return result;
        }

        private static int Compare(List<double> a, List<double> b)
        {
            a = RemoveLeadingZeros(a);
            b = RemoveLeadingZeros(b);
            if (a.Count != b.Count) return a.Count.CompareTo(b.Count);
            for (int i = 0; i < a.Count; i++)
                if (a[i] != b[i]) return a[i].CompareTo(b[i]);
            return 0;
        }

        private static List<double> RemoveLeadingZeros(List<double> num)
        {
            while (num.Count > 1 && num[0] == 0)
                num.RemoveAt(0);
            return num;
        }



        public List<double> Encrypt (List<double> message, List<double> e , List<double> n)
        {
            List<double> EncryptedMessage = Power(message,e) ;
            EncryptedMessage = Modulus(EncryptedMessage, n);

            return EncryptedMessage;
        }




        //helper
        public List<double> Power(List<double> baseNum, List<double> exponent)
        {
            // Handle exponent = 0 (any number^0 = 1)
            if (exponent.Count == 1 && exponent[0] == 0)
            {
                return new List<double> { 1 };
            }

            // Handle exponent = 1 (any number^1 = itself)
            if (exponent.Count == 1 && exponent[0] == 1)
            {
                return new List<double>(baseNum);
            }

            // Check for negative exponent (not supported)
            if (exponent[0] < 0)
            {
                throw new ArgumentException("Negative exponents are not supported");
            }

            // Divide exponent by 2
            List<double> halfExponent = Divide(exponent, new List<double> { 2 }).quotient;
            List<double> result = Power(baseNum, halfExponent);




            if (CheckEven_Odd(exponent) == "Odd")
            {
                // Odd exponent: result = baseNum * result * result
                return Multiply(Multiply(baseNum, result), result);
            }
            else
            {
                // Even exponent: result = result * result
                return Multiply(result, result);
            }
        }



        public List<double> Modulus(List<double> a, List<double> b)
        {
            // Handle division by zero - O(1)
            if (b.Count == 1 && b[0] == 0) throw new DivideByZeroException();
            // Handle zero dividend - O(1)
            if (a.Count == 1 && a[0] == 0) return new List<double> { 0 };
            // Determine signs - O(1)
            bool aNegative = a[0] < 0;
            bool bNegative = b[0] < 0;
            bool remainderNegative = aNegative;
            // Work with absolute values - O(N)
            List<double> absA = a.Select(Math.Abs).ToList();
            List<double> absB = b.Select(Math.Abs).ToList();
            // Base case - O(N) for Compare
            if (Compare(absA, absB) < 0)
                return remainderNegative ? Negate(absA) : absA;
            // Recursive modulus - T(N/2)
            var twoB = Addition(absB, absB); // O(N)
            var (q, r) = Divide(absA, twoB);
            // Adjust results - O(N) for Compare and Subtraction
            if (Compare(r, absB) < 0)
                return remainderNegative ? Negate(r) : r;
            else
                return Subtraction(r, absB);
        }


    }


    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Testing");
            var vec1 = new List<double> { 7}; 
            var vec2 = new List<double> { 3,7,1,3};
            var vec3 = new List<double> { 1, 2, 3, 4 };
            var vec4 = new List<double> { 4, 5, 6, 7 }; 
            var vec5 = new List<double> { 2,0,0,3};
           

            MyBigInteger bigInt = new MyBigInteger();



            //Console.WriteLine("Addition:");

            //var result = bigInt.Addition(vec1, vec2);       
            //Console.WriteLine(string.Join("", result));
            //var resultt = bigInt.Addition(vec3, vec4);
            //Console.WriteLine(string.Join("", resultt));


            //Console.WriteLine("Subtraction:");

            //var sub= bigInt.Subtraction(vec1, vec2);    
            //Console.WriteLine(string.Join("", sub));
            //var subb = bigInt.Subtraction(vec4, vec3);
            //Console.WriteLine(string.Join("", subb));
            //var subt = bigInt.Subtraction(vec3, vec4);
            //Console.WriteLine(string.Join("", subt));
            //var subtr = bigInt.Subtraction(vec2, vec1);
            //Console.WriteLine(string.Join("", subtr));
            //var subtra = bigInt.Subtraction(vec1, vec1);
            //Console.WriteLine(string.Join("", subtra));


            //Console.WriteLine("Even or Odd:");

            //var number1 = bigInt.CheckEven_Odd(vec1);
            //Console.WriteLine(string.Join("", number1));
            //var number2 = bigInt.CheckEven_Odd(vec2);
            //Console.WriteLine(string.Join("", number2));
            //var number3 = bigInt.CheckEven_Odd(vec3);
            //Console.WriteLine(string.Join("", number3));
            //var number4 = bigInt.CheckEven_Odd(vec4);
            //Console.WriteLine(string.Join("", number4));


            //Console.WriteLine("Multiply:");
            //var mult = bigInt.Multiply(vec3, vec4);
            //Console.WriteLine(string.Join("", mult));


            // Test Case 1: 100 / 25 = 4 R0
            //var (q1, r1) = bigInt.Divide(new List<double> { 1, 0, 0 }, new List<double> { 2, 5 });
            //Console.WriteLine($"100/25 = {string.Join("", q1)} R {string.Join("", r1)}");

            //// Test Case 2: -101 / 25 = -4 R-1
            //var (q2, r2) = bigInt.Divide(new List<double> { -1, 0, 1 }, new List<double> { 2, 5 });
            //Console.WriteLine($"-101/25 = {string.Join("", q2)} R {string.Join("", r2)}");

            //// Test Case 3: 123456789 / 123 = 1003713 R90
            //var (q3, r3) = bigInt.Divide(new List<double> { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, new List<double> { 1, 2, 3 });
            //Console.WriteLine($"123456789/123 = {string.Join("", q3)} R {string.Join("", r3)}");

            //// Test Case 4: 0 / 100 = 0 R0
            //var (q4, r4) = bigInt.Divide(new List<double> { 0 }, new List<double> { 1, 0, 0 });
            //Console.WriteLine($"0/100 = {string.Join("", q4)} R {string.Join("", r4)}");

            //var (q5, r5) = bigInt.Divide(new List<double> { 2,0,0,1 }, new List<double> { 2,0,0,1 });
            //Console.WriteLine($"2001/2001 = {string.Join("", q5)} R {string.Join("", r5)}");

            //var (q6, r6) = bigInt.Divide(new List<double> { 2,0,0,0,1 }, new List<double> { 1, 0, 0 });
            //Console.WriteLine($"20001/100 = {string.Join("", q6)} R {string.Join("", r6)}");

            //Console.WriteLine("Power:");
            //var power = bigInt.Power(vec2, vec1);
            //Console.WriteLine(string.Join("", power));

            //Console.WriteLine("Modulus:");
            //var mod = bigInt.Modulus(vec3, vec2);
            //Console.WriteLine(string.Join("", mod));
            //Console.ReadLine();

            Console.WriteLine("Encryption:");
            var encryption = bigInt.Encrypt(vec5,vec1,vec2);
            Console.WriteLine(string.Join("", encryption));
            Console.ReadLine();

        }
       
       


    }
}

