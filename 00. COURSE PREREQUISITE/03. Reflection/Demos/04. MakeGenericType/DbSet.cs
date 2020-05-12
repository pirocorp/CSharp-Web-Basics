namespace _04._MakeGenericType
{
    using System.Collections.Generic;

    public class DbSet<TEntity>
    {
        private ICollection<TEntity> _entities;

        public DbSet()
        {
            this._entities = new List<TEntity>();
        }

        public int Count => this._entities.Count;

        public void Add(TEntity entity)
        {
            this._entities.Add(entity);
        }
    }
}
