using System;
using System.Text.Json;
using CloneableDemo;
namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var obj = new Class2()
            {
                A = "My IO",
            };
            var deep = new Class1()
            {
                A = "My IO",
                B = obj
            };
            var clone = deep.Clone();
            var shallow = deep.ShallowClone();
            deep.B.A = "old name";

            Console.WriteLine(JsonSerializer.Serialize(shallow, new JsonSerializerOptions { WriteIndented = true }));
            Console.WriteLine(JsonSerializer.Serialize(clone, new JsonSerializerOptions { WriteIndented = true }));
        }
    }

    [Cloneable]
    public class Class1
    {
        public string A { get; set; }
        public Class2 B { get; set; }

        public Class1 ShallowClone()
        {
            return (Class1)this.MemberwiseClone();
        }
    }

    [Cloneable]
    public class Class2
    {
        public string A { get; set; }
    }
}
