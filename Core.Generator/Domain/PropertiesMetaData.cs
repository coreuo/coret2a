using System.Collections.Generic;

namespace Core.Generator.Domain
{
    public class PropertiesMetaData
    {
        private readonly Dictionary<string, List<(string name, string size, string offset)>> _dictionary = new Dictionary<string, List<(string name, string size, string offset)>>();

        public List<(string name, string size, string offset)> Get(string name, bool isEntity = true)
        {
            return _dictionary.TryGetValue(name, out var existing) ? existing : _dictionary[name] = isEntity ? new List<(string name, string size, string offset)> { ("nameof(Free)", "sizeof(int)", "0") } : new List<(string name, string size, string offset)>();
        }
    }
}
