using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Numerics;

namespace Algo_Project
{
    public class BigInteger
    {


        public void addition(List<double> Vector1,List<double> Vector2)
        {
            List<double> Sum= new List<double>(new double[Vector1.Count]);

           for(int i = Vector1.Count-1; i >=0; i--)
            {
                if (Vector1[i] + Vector2[i] >= 10)
                {
                    Sum[i] += Vector1[i] + Vector2[i] - 10;
                    Sum[i - 1] = 1;
                }
                else
                {
                    Sum[i] = Vector1[i] + Vector2[i];
                }
            }
           for(int i = 0; i < Vector1.Count; i++)
            {
                Console.Write(Sum[i]);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine("Hello World");
            
            BigInteger bigInt = new BigInteger();
            bigInt.addition(new List<double> { 1, 2, 3, 5 }, new List<double> { 4, 5, 6, 9 });
            Console.ReadLine();
        }
    }
}