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

            // Recursive multiplications (Karatsuba steps)
            List<double> z0 = Multiply(
                low1.Select(c => (double)char.GetNumericValue(c)).ToList(),
                low2.Select(c => (double)char.GetNumericValue(c)).ToList());

            List<double> z1 = Multiply(
                Add(low1, high1).Select(c => (double)char.GetNumericValue(c)).ToList(),
                Add(low2, high2).Select(c => (double)char.GetNumericValue(c)).ToList());

            List<double> z2 = Multiply(
                high1.Select(c => (double)char.GetNumericValue(c)).ToList(),
                high2.Select(c => (double)char.GetNumericValue(c)).ToList());

            // Calculate intermediate terms
            List<double> temp = Subtract(Subtract(z1, z2), z0);

            // Combine results: z2 * 10^(2*splitPos) + temp * 10^splitPos + z0
            List<double> term1 = ShiftLeft(z2, 2 * splitPos);
            List<double> term2 = ShiftLeft(temp, splitPos);
            List<double> result = Add(Add(term1, term2), z0);

            return result;
        }

        private string Add(string a, string b)
        {
            int maxLength = Math.Max(a.Length, b.Length);
            a = a.PadLeft(maxLength, '0');
            b = b.PadLeft(maxLength, '0');

            int carry = 0;
            string result = "";

            for (int i = maxLength - 1; i >= 0; i--)
            {
                int sum = (a[i] - '0') + (b[i] - '0') + carry;
                carry = sum / 10;
                result = (sum % 10) + result;
            }

            return carry > 0 ? carry + result : result;
        }

        private List<double> Add(List<double> a, List<double> b)
        {
            string aStr = string.Concat(a.Select(d => ((int)d).ToString()));
            string bStr = string.Concat(b.Select(d => ((int)d).ToString()));
            string sum = Add(aStr, bStr);
            return sum.Select(c => (double)char.GetNumericValue(c)).ToList();
        }



        private List<double> Subtract(List<double> a, List<double> b)
        {
            string aStr = string.Concat(a.Select(d => ((int)d).ToString()));
            string bStr = string.Concat(b.Select(d => ((int)d).ToString()));

            int maxLength = Math.Max(aStr.Length, bStr.Length);
            aStr = aStr.PadLeft(maxLength, '0');
            bStr = bStr.PadLeft(maxLength, '0');

            int borrow = 0;
            string result = "";

            for (int i = maxLength - 1; i >= 0; i--)
            {
                int digitA = (aStr[i] - '0') - borrow;
                int digitB = bStr[i] - '0';

                if (digitA < digitB)
                {
                    digitA += 10;
                    borrow = 1;
                }
                else
                {
                    borrow = 0;
                }

                result = (digitA - digitB) + result;
            }

            return result.TrimStart('0').Select(c => (double)char.GetNumericValue(c)).ToList();
        }

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
    }


    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Testing");
            var vec1 = new List<double> { 1, 2, 3 }; 
            var vec2 = new List<double> { 4, 5 };
            var vec3 = new List<double> { 1, 2, 3, 4 };
            var vec4 = new List<double> { 4, 5, 6, 7 };

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
            Console.WriteLine("Multiply:");
            var mult = bigInt.Multiply(vec2, vec3);
            Console.WriteLine(string.Join("", mult));


            Console.ReadLine();
        }
    


    }
}

