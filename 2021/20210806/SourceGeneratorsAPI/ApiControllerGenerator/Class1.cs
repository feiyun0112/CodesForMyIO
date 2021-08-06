using System;
using System.Text.RegularExpressions;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.CSharp;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Collections.Generic;

namespace ApiControllerGenerator
{
    [Generator]
    public class ApiControllerGenerator : ISourceGenerator
    { 
        public void Initialize(GeneratorInitializationContext context)
        {
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var compilation = context.Compilation;
            var allClasses = compilation.SyntaxTrees.
               SelectMany(x => x.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>());

            Dictionary<string,List<ClassDeclarationSyntax>> controllers = GetControllers(allClasses);
           
            foreach (var controller in controllers)
            {
                var sourceBuilder = new StringBuilder();
                sourceBuilder.Append($@"
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiControllerGenerator
{{   
    [Route(""api/[controller]"")]
    [ApiController]
    public class {controller.Key}NewController : ControllerBase
    {{
        IMediator _mediator;
        public {controller.Key}NewController(IMediator mediator)
        {{
            _mediator = mediator;
        }}
");
                foreach (var request in controller.Value)
                {
                    sourceBuilder.Append(CreateMethodText(compilation, request));
                }

                sourceBuilder.Append(@"    }
}");

                context.AddSource(controller.Key+"Controller", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
            }
        }

        private string CreateMethodText(Compilation compilation, ClassDeclarationSyntax request)
        {
            var sourceBuilder = new StringBuilder();
            var fullName = GetClassFullName(compilation, request);
            if (fullName.EndsWith("Command"))
            {
                sourceBuilder.Append($@"
        [HttpPost]
        public async Task<{((GenericNameSyntax)request.BaseList.Types[0].Type).TypeArgumentList.Arguments[0].ToFullString()}> {request.Identifier.ToFullString().Substring(0, request.Identifier.ToFullString().Length-8)}([FromBody]{fullName} cmd)
        {{
            return await _mediator.Send(cmd, HttpContext.RequestAborted);
        }}
                ");
            }
            else
            {
                sourceBuilder.Append($@"
        [HttpGet]
        public async Task<{((GenericNameSyntax)request.BaseList.Types[0].Type).TypeArgumentList.Arguments[0].ToFullString()}> {request.Identifier.ToFullString().Substring(0, request.Identifier.ToFullString().Length - 6)}([FromQuery]{fullName} query)
        {{
            return await _mediator.Send(query);
        }}
                ");
            }
            return sourceBuilder.ToString();
        }

        private string GetClassFullName(Compilation compilation, ClassDeclarationSyntax classDeclarationSyntax)
        {
            var semanticModel = compilation.GetSemanticModel(classDeclarationSyntax.SyntaxTree);
            var symbol = ModelExtensions.GetDeclaredSymbol(semanticModel, classDeclarationSyntax);
            return symbol.OriginalDefinition.ToString();
        }

        private Dictionary<string, List<ClassDeclarationSyntax>> GetControllers(IEnumerable<ClassDeclarationSyntax> allClasses)
        {
            var requests = new List<ClassDeclarationSyntax>();
            foreach (var classDeclarationSyntax in allClasses)
            {   
                if (classDeclarationSyntax.BaseList != null)
                {
                    if(classDeclarationSyntax.BaseList.Types[0].Type is GenericNameSyntax type && type.Identifier.ValueText == "IRequest")
                        requests.Add(classDeclarationSyntax);
                }
            }

            return requests.GroupBy(r => GetControllerName(r)).ToDictionary(g => g.Key, g => g.ToList());
        }

        private string GetControllerName(ClassDeclarationSyntax classDeclarationSyntax)
        {
            string[] split = Regex.Split(classDeclarationSyntax.Identifier.ValueText, @"(?<!^)(?=[A-Z])");
            return split[1];
        }
    }
}
