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


        public void addition(List<double> Vector1, List<double> Vector2)
        {
            List<double> Sum = new List<double>(new double[Vector1.Count]);
            for (int i = Vector1.Count - 1; i >= 0; i--)
            {
                if (Vector1[i] + Vector2[i] >= 10)
                {
                    Sum[i] += Vector1[i] + Vector2[i] - 10;
                    Vector1[i - 1] = Vector1[i - 1] - 1;
                }
                else
                {
                    Sum[i] = Vector1[i] + Vector2[i];
                }
            }
            for (int i = 0; i < Vector1.Count; i++)
            {
                Console.Write(Sum[i]);
            }
        }

        public void subtraction(List<double> Vector1, List<double> Vector2)
        {
            int m = Vector1.Count;
            int n = Vector2.Count;
            bool swapped = false;
            if ( n > m || Vector1[m - 1] < Vector2[n - 1]) //vector1 < vector 2
            { 
                swapped = true;
                List<double> templ = new List<double>();
                templ = Vector1;
                Vector1 = Vector2;
                Vector2 = templ;
            }
   
            List<double> Resault = new List<double>(new double[Vector1.Count]);
            for (int i = Vector1.Count - 1; i >= 0; i--)
            {
                double temp = Vector1[i] - Vector2[i];
                if (temp < 0 )
                {
                    temp += 10;
                    Vector1[i - 1]--;
                }
                Resault[i] = temp;

            }
  
            if (swapped)
            {
                Resault[0] = -Resault[0];
            }
            for (int i = 0; i < Vector1.Count; i++)
            {
                Console.Write(Resault[i]);
            }
        }
        public void CheckEven(List<double> Vector)
        {
            int m = Vector.Count;
            double n = Vector[m - 1] % 2;
            if (n == 1 )
            {
                Console.WriteLine("Odd"); 
            }
            else
            {
                Console.WriteLine("Even");
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");


            MyBigInteger bigInt = new MyBigInteger();
            Console.WriteLine("addition:");
            bigInt.addition(new List<double> { 1, 2, 3, 5 }, new List<double> { 4, 5, 6, 9 });
            Console.WriteLine("");

            Console.WriteLine("addition:");
            bigInt.addition(new List<double> { 2, 5, 9, 1 }, new List<double> { 7, 5, 3, 8 });
            Console.WriteLine("");

            Console.WriteLine("addition:");
            bigInt.addition(new List<double> { 1, 2, 3, 5, 6 }, new List<double> { 3, 4, 5, 6, 9 });
            Console.WriteLine("");

            Console.WriteLine("subtraction:");
            bigInt.subtraction(new List<double> { 1, 2, 3, 5 }, new List<double> { 4, 5, 6, 9 });
            Console.WriteLine("");

            Console.WriteLine("subtraction:");
            bigInt.subtraction(new List<double> { 4, 5, 6, 9 }, new List<double> { 1, 2, 3, 5 });
            Console.WriteLine("");

            Console.WriteLine("subtraction:");
            bigInt.subtraction(new List<double> { 1, 2, 3, 5 , 6 }, new List<double> {3, 4, 5, 6, 9 });
            Console.WriteLine("");

            Console.WriteLine("subtraction:");
            bigInt.subtraction(new List<double> { 3, 5, 9, 1 }, new List<double> { 7, 5 ,3, 8 });
            Console.WriteLine("");

            Console.WriteLine("Even or Odd:");
            bigInt.CheckEven(new List<double> { 7, 5, 3, 8 });


            Console.WriteLine("Even or Odd:");
            bigInt.CheckEven(new List<double> { 3, 5, 9, 1 });

            Console.ReadLine();
        }
    }

}

