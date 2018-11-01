using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Rename;

namespace RenameReproTester
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var workspace = MSBuildWorkspace.Create();
            Console.WriteLine("Opening solution");
            var solution = await workspace.OpenSolutionAsync(args[0]);

            Console.WriteLine("Getting symbol");
            var project = solution.Projects.Where(proj => proj.Name == "Microsoft.CodeAnalysis").Single();
            var compilation = await project.GetCompilationAsync();

            var methodBody = compilation.GetSymbolsWithName("IMethodBodyOperation").Single();

            Console.WriteLine("Renaming");
            solution = await Renamer.RenameSymbolAsync(solution, methodBody, "IRenamedMethodBodyOperation", solution.Workspace.Options);

            Console.WriteLine("Saving");
            workspace.TryApplyChanges(solution);
        }
    }
}
