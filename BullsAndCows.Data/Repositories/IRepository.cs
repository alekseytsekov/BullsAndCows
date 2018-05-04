using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BullsAndCows.Data.Repositories
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void Add(IEnumerable<T> entities);
        void Update();
        T FirstOrDefault();
        T FirstOrDefault(Expression<Func<T, bool>> predicate);
        T GetById(object id);
        bool Any();
        bool Any(Expression<Func<T, bool>> predicate);
        IQueryable<T> AllAsQueryable();
        IEnumerable<T> All();
        IEnumerable<T> Where(Expression<Func<T, bool>> predicate);
        BullsCowsDbContext Context { get; }

        void RemovePermanent(T entity);
    }
}
