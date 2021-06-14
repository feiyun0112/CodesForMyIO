using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;

namespace ClassLibrary1
{
    [Generator]
    public class CustomGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context) { }

        public void Execute(GeneratorExecutionContext context)
        {
            context.AnalyzerConfigOptions.GlobalOptions.TryGetValue($"build_property.RootNamespace", out var currentNamespace);
             

            context.AddSource("myGeneratedFile.cs", $@"
using System;
namespace {currentNamespace}
{{
    public class GeneratedClass
    {{
        public static void GeneratedMethod()
        {{
            Console.WriteLine(""公众号“My IO”!"");
        }}
    }}
}}");
        }
    }
}
