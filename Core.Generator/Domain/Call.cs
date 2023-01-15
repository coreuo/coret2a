using Core.Generator.Domain.Members.Methods;

namespace Core.Generator.Domain
{
    public class Call
    {
        public Object Object { get; }

        public MethodMember.MethodMerge MethodMerge { get; }

        public double Priority { get; }

        public string Name { get; }

        public string Parameters { get; }

        public bool Return { get; }

        public Call(Object @object, MethodMember.MethodMerge methodMerge, double priority, string name, string parameters, bool @return = false)
        {
            Object = @object;
            MethodMerge = methodMerge;
            Priority = priority;
            Name = name;
            Parameters = parameters;
            Return = @return;
        }
    }
}
