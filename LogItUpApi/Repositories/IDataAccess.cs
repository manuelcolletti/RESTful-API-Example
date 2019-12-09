using LogItUpApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LogItUpApi.Repositories
{
    public interface IDataAccess
    {
        public void BeginTransaction();

        public void RollbackTransaction();

        public void CommitTransaction();

        public void CloseConnection();

        public Task<T> GetFirst<T>(ApplicationUser user, Expression<Func<T, bool>> predicate) where T : UserEntity;

        public Task<List<T>> GetList<T>(ApplicationUser user, Expression<Func<T, bool>> predicate) where T : UserEntity;

        public Task<List<T>> GetAll<T>() where T : MasterEntity;

        public Task<T> Insert<T>(ApplicationUser user, T item) where T : UserEntity;

        public void Update<T>(ApplicationUser user, T item) where T : UserEntity;

        public void Delete<T>(ApplicationUser user, T item) where T : UserEntity;

        public void PhysicalDelete<T>(ApplicationUser user, T item) where T : UserEntity;

        public List<T> ExecuteSqlQuery<T>(ApplicationUser user, string query) where T : UserEntity;

        public void ExecuteSqlCommand(string command);
    }
}
