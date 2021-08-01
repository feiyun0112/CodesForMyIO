using Serilog;
using Serilog.Events;
using Serilog.Templates;
using System;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(new ExpressionTemplate("{#each name, value in @p}   {name} = {value}\n{#end}"))
                .CreateLogger();


            var user = new { Name = "张三", Password = "xxx" };
            Log.Information("登录信息：{@Name} {@Password}", user.Name,user.Password);


            Console.ReadLine();

        }
    }
}
