using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL
{
    public interface IRepository<TSource>
    {
        public Task<int> Create(IEnumerable<TSource> records);

        public Task<IEnumerable<TSource>> Read(IEnumerable<int> ids = null);

        public Task<int> Update(IEnumerable<TSource> records);

        public Task<int> Delete(IEnumerable<int> ids);
    }
}
