using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;

namespace ClassLibrary2
{
    [Generator]
    public class DemoSourceGenerator : ISourceGenerator
    { 
        public void Execute(GeneratorExecutionContext context)
        {
            const string cloneableAttributeText = @"using System;

namespace CloneableDemo
{
    public sealed class CloneableAttribute : Attribute
    {
        public CloneableAttribute()
        {
        }
    }
}
";

            context.AddSource("CloneableAttribute", SourceText.From(cloneableAttributeText, Encoding.UTF8));


            var compilation = context.Compilation;
 

            var classSymbols = compilation
                .SyntaxTrees
                .SelectMany(syntaxTree => syntaxTree.GetRoot().DescendantNodes())
                .Where(x => x is ClassDeclarationSyntax)
                .Cast<ClassDeclarationSyntax>()
                .Select(x => GetClassSymbol(compilation,x))
                .ToList();
           
            foreach (var classSymbol in classSymbols)
            {
                if (!classSymbol.GetAttributes().Any(a => a.ToString() == "Cloneable"))
                        continue;

                context.AddSource($"{classSymbol.Name}_clone.cs", SourceText.From(CreateCloneableCode(classSymbol), Encoding.UTF8));
            }
        }

        private static INamedTypeSymbol GetClassSymbol(Compilation compilation, ClassDeclarationSyntax clazz)
        {
            var model = compilation.GetSemanticModel(clazz.SyntaxTree);
            var classSymbol = model.GetDeclaredSymbol(clazz);
            return classSymbol;
        }

        private string CreateCloneableCode(INamedTypeSymbol classSymbol)
        {
            string namespaceName = classSymbol.ContainingNamespace.ToDisplayString();
            var propertyNames = classSymbol.GetMembers().OfType<IPropertySymbol>();
            var codes = new StringBuilder();
            foreach (var propertyName in propertyNames)
            {
                if (isCloneable(propertyName))
                {
                    codes.AppendLine($@"                {propertyName.Name} = obj.{propertyName.Name}?.Clone(),");
                }
                else
                {
                    codes.AppendLine($@"                {propertyName.Name} = obj.{propertyName.Name},");
                }
            }

            return $@"using System.Collections.Generic;

namespace {namespaceName}
{{
    public static class {classSymbol.Name}Extentions
    {{
        public static {classSymbol.Name} Clone(this {classSymbol.Name} obj)
        {{
            return new {classSymbol.Name}
            {{
 {codes.ToString()}
            }};
        }}
    }}
}}";
        }

        private bool isCloneable(IPropertySymbol propertyName)
        {
            if (!propertyName.Type.GetAttributes()
                .Any(a => a.ToString() == "Cloneable"))
            {
                return false;
            }

            return true;
        }

        public void Initialize(GeneratorInitializationContext context)
        {
        }
    }
     
}
