using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineMon.Core.Repositories
{
    public interface IGenericRepository
    {
        IEnumerable<T> GetAll<T>(object parameters = null);

        void Insert<T>(T instance);

        // Last resort: execute arbitrary SQL directly
        void Execute(string sql, object parameters = null);
    }
}
