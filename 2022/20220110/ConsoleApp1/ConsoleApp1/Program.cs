using Microsoft.Playwright;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var app = new CommandApp();
            app.Configure(config =>
            {
                config.AddCommand<DemoCommand>("demo");
                config.AddCommand<AnotherCommand>("another");
            });

            return await app.RunAsync(args);
        }
    }

    public class DemoCommand : AsyncCommand<DemoCommandSettings>
    {
        public override async Task<int> ExecuteAsync(CommandContext context, DemoCommandSettings settings)
        {
            string name=settings.Name;
            await AnsiConsole.Progress()
                .StartAsync(async ctx =>
                { 
                    var task1 = ctx.AddTask("执行中......");

                    while (!ctx.IsFinished)
                    {
                        // 模拟工作
                        await Task.Delay(100);
                         
                        task1.Increment(1);
                    }
                });

            Console.WriteLine($@"Hello {name}!");

            return 0;
        }
    }

    public class DemoCommandSettings : CommandSettings
    {
        [CommandOption("-u|--username")]
        [Description("需要显示的名称")]
        public string Name { get; set; }

    }

    internal class AnotherCommand : AsyncCommand<AnotherCommandSettings>
    {
        public override Task<int> ExecuteAsync(CommandContext context, AnotherCommandSettings settings)
        {
            throw new NotImplementedException();
        }
    }

    internal class AnotherCommandSettings : CommandSettings
    {
    }
}
