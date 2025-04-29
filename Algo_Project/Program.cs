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

        

            Console.WriteLine("Addition:");
            
            var result = bigInt.Addition(vec1, vec2);       
            Console.WriteLine(string.Join("", result));
            var resultt = bigInt.Addition(vec3, vec4);
            Console.WriteLine(string.Join("", resultt));


            Console.WriteLine("Subtraction:");

            var sub= bigInt.Subtraction(vec1, vec2);    
            Console.WriteLine(string.Join("", sub));
            var subb = bigInt.Subtraction(vec4, vec3);
            Console.WriteLine(string.Join("", subb));
            var subt = bigInt.Subtraction(vec3, vec4);
            Console.WriteLine(string.Join("", subt));
            var subtr = bigInt.Subtraction(vec2, vec1);
            Console.WriteLine(string.Join("", subtr));
            var subtra = bigInt.Subtraction(vec1, vec1);
            Console.WriteLine(string.Join("", subtra));


            Console.WriteLine("Even or Odd:");

            var number1 = bigInt.CheckEven_Odd(vec1);
            Console.WriteLine(string.Join("", number1));
            var number2 = bigInt.CheckEven_Odd(vec2);
            Console.WriteLine(string.Join("", number2));
            var number3 = bigInt.CheckEven_Odd(vec3);
            Console.WriteLine(string.Join("", number3));
            var number4 = bigInt.CheckEven_Odd(vec4);
            Console.WriteLine(string.Join("", number4));




            Console.ReadLine();
        }
    


    }
}

