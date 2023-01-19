using Core.Generator.Domain.Members.Methods;

namespace Core.Generator.Domain
{
    public class Call
    {
        public Object Object { get; }

        public MethodMember.MethodMerge MethodMerge { get; }

        public string Caller { get; }

        public (string subject, string property, int value)? Case { get; }

        public double Priority { get; }

        public string Name { get; }

        public string Parameters { get; }

        public bool Return { get; }

        public Call(Object @object, MethodMember.MethodMerge methodMerge, string caller, (string subject, string property, int value)? @case, double priority, string name, string parameters, bool @return = false)
        {
            Object = @object;
            MethodMerge = methodMerge;
            Caller = caller;
            Case = @case;
            Priority = priority;
            Name = name;
            Parameters = parameters;
            Return = @return;
        }

        public string GetCode()
        {
            return $"{(Return ? "return " : string.Empty)}{Name}({Parameters});";
        }
    }
}
