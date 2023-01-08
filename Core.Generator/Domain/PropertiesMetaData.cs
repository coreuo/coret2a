using System.Collections.Generic;

namespace Core.Generator.Domain
{
    public class PropertiesMetaData
    {
        private readonly Dictionary<string, List<(string name, string size)>> _dictionary = new Dictionary<string, List<(string name, string size)>>();

        public List<(string name, string size)> Get(string name, bool isEntity = true)
        {
            return _dictionary.TryGetValue(name, out var existing) ? existing : _dictionary[name] = isEntity ? new List<(string name, string size)> { ("nameof(Free)", "sizeof(int)") } : new List<(string name, string size)>();
        }
    }
}
