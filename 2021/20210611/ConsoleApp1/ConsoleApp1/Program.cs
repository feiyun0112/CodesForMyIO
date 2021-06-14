using HashidsNet;
using System;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            HashInt();
            HashLong();
            MinHashLength();
            Alphabet();
        }

        private static void Alphabet()
        {
            var hashids = new Hashids("公众号My IO", alphabet: @"あいうえおかきくけこさしすせそたちつてと");
            var str = hashids.Encode(12345);
            Console.WriteLine(str);
        }

        private static void MinHashLength()
        {
            var hashids = new Hashids("公众号My IO", minHashLength: 8);
            var str = hashids.Encode(12345);
            var num = hashids.Decode(str)[0];
            var success = (hashids.Decode("WwYQ").Any());
            Console.WriteLine(str);
            Console.WriteLine(num);
            Console.WriteLine(success);
        }

        private static void HashInt()
        {
            var hashids = new Hashids("公众号My IO");
            var str = hashids.Encode(12345);
            var num = hashids.Decode(str)[0];
            Console.WriteLine(str);
            Console.WriteLine(num);
        }

        private static void HashLong()
        {
            var hashids = new Hashids("公众号My IO");
            var str = hashids.EncodeLong(666555444333222L);
            var num = hashids.DecodeLong(str)[0];
            Console.WriteLine(str);
            Console.WriteLine(num);
        }
    }
}
