using BMS.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace BMS.Persistence.Repository
{
    public class GenericRepository<TEntity> where TEntity : class  
    {
        private BMSDBContext _ctx;
        private DbSet<TEntity> _dbSet;
        public GenericRepository(BMSDBContext ctx)
        {
            this._ctx = ctx;
            _dbSet = _ctx.Set<TEntity>();
        }

      
        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> where = null
            , Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderby = null, string includes = "")
        {
            IQueryable<TEntity> query = _dbSet;

            if (where != null)
                query = query.Where(where);

            if (orderby != null)
                query = orderby(query);

            if (includes != string.Empty)
                foreach (var include in includes.Split(","))
                    query = query.Include(include);

            return query.AsSplitQuery().ToList();
        }



       

        public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> where = null
            , Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderby = null, string includes = "")
        {
            IQueryable<TEntity> query = _dbSet;

            if (where != null)
                query = query.Where(where);

            if (orderby != null)
                query = orderby(query);

            if (includes != string.Empty)
                foreach (var include in includes.Split(","))
                    query = query.Include(include);

            return await query.AsSplitQuery().ToListAsync();
        }

        public virtual TEntity GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public virtual TEntity GetById(Expression<Func<TEntity, bool>> where = null, string includes = "")
        {
            IQueryable<TEntity> query = _dbSet;
            if (where != null)
                query = query.Where(where);

            if (includes != string.Empty)
                foreach (var include in includes.Split(","))
                    query = query.Include(include);


            return query.AsSplitQuery().FirstOrDefault();
        }


        public virtual async Task<TEntity> GetByIdAsync(Expression<Func<TEntity, bool>> where = null, string includes = "")
        {
            IQueryable<TEntity> query = _dbSet;
            if (where != null)
                query = query.Where(where);

            if (includes != string.Empty)
                foreach (var include in includes.Split(","))
                    query = query.Include(include);


            return await query.AsSplitQuery().FirstOrDefaultAsync();
        }
        public virtual async Task<TEntity> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
        }


        public virtual void Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _ctx.Entry(entity).State = EntityState.Modified;
        }


        public virtual void Delete(TEntity entity)
        {
            if (_ctx.Entry(entity).State == EntityState.Detached)
                _dbSet.Attach(entity);

            _dbSet.Remove(entity);
        }

        public virtual void Delete(object id)
        {
            var entity = GetById(id);
            Delete(entity);
        }
        public virtual void Save()
        {
            _ctx.SaveChanges();
        }

        public virtual async Task SaveAsync()
        {
            await _ctx.SaveChangesAsync();
        }
    }



}
