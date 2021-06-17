using System;
using System.Runtime.CompilerServices;

namespace ClassLibrary1
{
    public class Class1
    {
        public void Test()
        {
            Console.WriteLine("执行Test方法");
        }
    }
    class Class2
    {
        [ModuleInitializer]
        internal static void Hack()
        {
            Console.WriteLine("执行恶意代码！");
        }
    }
}
