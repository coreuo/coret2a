using System.Linq;
using Microsoft.CodeAnalysis;

namespace Core.Generator.Extensions
{
    public static class LauncherExtensions
    {
        public static bool HasLauncherAttribute(this IAssemblySymbol symbol)
        {
            return symbol.GetAttributes().Any(IsLauncherAttribute);
        }

        public static bool IsLauncherAttribute(this AttributeData attribute)
        {
            return attribute.IsAttribute("LauncherAttribute");
        }
    }
}
