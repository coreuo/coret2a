using System.Collections.Generic;
using System.Linq;

namespace Core.Generator.Domain
{
    public class EntitiesMetaData
    {
        private readonly HashSet<EntityMetaData> _collection = new HashSet<EntityMetaData>();

        public EntitiesMetaData()
        {
        }

        public void Add(string fullName, string name, bool cached = false, bool synchronized = false)
        {
            var record = new EntityMetaData(fullName, name, cached, synchronized);

            _collection.Add(record);
        }

        public IEnumerable<string> GetStoreProperties()
        {
            return _collection.Select(e => e.ToStoreProperty());
        }

        public IEnumerable<string> GetStoreAssignments()
        {
            return _collection.Select(e => e.ToStoreAssignment(e.Synchronized));
        }

        public IEnumerable<string> GetStoreFlushes()
        {
            return _collection.Where(e => !e.Cached).Select(e => e.ToStoreFlush());
        }
    }
}
