using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        new Program().TestLock();

        Console.Read();
    }

    void TestLock()
    {
        lock (this)
        {
            var task = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("-------开始-------");
                Deadlock();
                Console.WriteLine("---------完成--------");
            });

            task.Wait();
        }
    }

    void Deadlock()
    {
        lock (this)
        {
            Console.WriteLine("公众号“My IO”");
        }
    }
}