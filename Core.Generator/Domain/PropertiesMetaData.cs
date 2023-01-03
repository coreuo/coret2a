using System.Collections.Generic;

namespace Core.Generator.Domain
{
    public class PropertiesMetaData
    {
        private readonly Dictionary<string, List<string>> _dictionary = new Dictionary<string, List<string>>();

        public PropertiesMetaData()
        {
        }

        public List<string> Get(string name)
        {
            return _dictionary.TryGetValue(name, out var existing) ? existing : _dictionary[name] = new List<string> { $@"
            new(nameof(Free), sizeof(int))" };
        }
    }
}
