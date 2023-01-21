using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Core.Generator.Domain.Members.Methods;

namespace Core.Generator.Domain
{
    public class Call
    {
        public Object Object { get; set; }

        public MethodMember.MethodMerge MethodMerge { get; set; }

        public string Caller { get; set; }

        public (string subject, string property, int value) Case { get; set; }

        public (string parameter, int value) Also { get; set; }

        public double Priority { get; }

        public string Name { get; }

        public ImmutableArray<(string fullType, string type, string name)> Parameters { get; }

        public bool Return { get; }

        public Call(string name, IEnumerable<(string fullType, string type, string name)> parameters, bool @return = false)
        {
            Name = name;
            Parameters = parameters.ToImmutableArray();
            Return = @return;
        }

        public Call(double priority, string name, IEnumerable<(string fullType, string type, string name)> parameters, bool @return = false) : this(name, parameters, @return)
        {
            Priority = priority;
        }

        public string GetCode()
        {
            return $"{(Return ? "return " : string.Empty)}{Name}({string.Join(", ", Parameters.Select(p => p.name == Also.parameter ? $"{Also.value}" : $"{p.name}"))});";
        }
    }
}
