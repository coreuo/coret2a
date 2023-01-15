using Core.Generator.Domain;
using Core.Generator.Extensions;
using Microsoft.CodeAnalysis;

namespace Core.Generator
{
    [Generator]
    public class LauncherGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (!context.Compilation.Assembly.HasLauncherAttribute()) return;

            var root = new Root();

            root.Resolve(context.Compilation);

            var saveCode = root.GetCode();

            context.AddSource("Save.cs", saveCode);

            foreach (var @object in root.Objects.Values)
            {
                var objectCode = @object.GetCode();

                context.AddSource(@object.Name, objectCode);
            }
        }
    }
}
