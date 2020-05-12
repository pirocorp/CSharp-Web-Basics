using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CheckForConstant
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CheckForConstantAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "CheckForConstant";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Usage";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            var analyze = GeneratedCodeAnalysisFlags.Analyze;
            var none = GeneratedCodeAnalysisFlags.None;
            var localDeclartions = SyntaxKind.LocalDeclarationStatement;

            context.ConfigureGeneratedCodeAnalysis(analyze | none);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeNode, localDeclartions);
        }

        private void AnalyzeNode(SyntaxNodeAnalysisContext context)
        {
            var localDeclaration = (LocalDeclarationStatementSyntax)
                context.Node;

            if (localDeclaration.Modifiers.Any(SyntaxKind.ConstKeyword))
            {
                return;
            }

            var dataFlowAnalysis = context.SemanticModel.AnalyzeDataFlow(localDeclaration);
            var variable = localDeclaration.Declaration.Variables.Single();
            var variableSymbol = context.SemanticModel.GetDeclaredSymbol
                (variable);

            if (dataFlowAnalysis.WrittenOutside.Contains(variableSymbol))
            {
                return;
            }

            context.ReportDiagnostic(Diagnostic.Create(Rule, context.Node
                .GetLocation()));

        }
    }
}
