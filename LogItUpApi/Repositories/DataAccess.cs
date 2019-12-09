using LogItUpApi.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using LogItUpApi.Contexts;

namespace LogItUpApi.Repositories
{
    public class DataAccess : IDataAccess
    {

        private IDbContextTransaction transaction = null;

        private ApplicationDbContext dBContext = null;
        
        public DataAccess(ApplicationDbContext context)
        {
            dBContext = context;
        }

        public void BeginTransaction()
        {
            transaction = dBContext.Database.BeginTransaction();
        }

        public void RollbackTransaction()
        {
            transaction.Rollback();
        }

        public void CommitTransaction()
        {
            transaction.Commit();
        }

        public void CloseConnection()
        {
            dBContext.Dispose();
        }

        public Task<T> GetFirst<T>(ApplicationUser user, Expression<Func<T, bool>> predicate) where T : UserEntity
        {
            try
            {
                return dBContext.Set<T>().Where(x => x.ApplicationUserId == user.Id && x.DeletionDate == null)
                                         .FirstOrDefaultAsync(predicate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<List<T>> GetList<T>(ApplicationUser user, Expression<Func<T, bool>> predicate) where T : UserEntity
        {
            try
            {
                return dBContext.Set<T>().Where(x => x.ApplicationUserId == user.Id && x.DeletionDate == null)
                                        .Where(predicate).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //List<AT_MENU> menuList = new DataAccess().GetList<AT_MENU>(x => x.MEN_VISIBLE == 1);
        public Task<List<T>> GetAll<T>() where T : MasterEntity
        {
            try
            {
                return dBContext.Set<T>().Where(x => x.DeletionDate == null).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<T> Insert<T>(ApplicationUser user, T item) where T : UserEntity
        {
            try
            {
                item.ApplicationUserId = user.Id;

                item.CreationDate = DateTime.Now;

                dBContext.Set<T>().Add(item);

                dBContext.SaveChanges();

                return GetFirst<T>(user, x => x.Id == item.Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Update<T>(ApplicationUser user, T item) where T : UserEntity
        {
            try
            {
                item.ModificationDate = DateTime.Now;

                dBContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete<T>(ApplicationUser user, T item) where T : UserEntity
        {
            try
            {
                if (item.ApplicationUserId == user.Id)
                {
                    item.DeletionDate = DateTime.Now;

                    dBContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void PhysicalDelete<T>(ApplicationUser user, T item) where T : UserEntity
        {
            try
            {
                if (item.ApplicationUserId == user.Id)
                {
                    dBContext.Set<T>().Remove(item);

                    dBContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<T> ExecuteSqlQuery<T>(ApplicationUser user, string query) where T : UserEntity
        {
            try
            {
                var result = dBContext.Set<T>().FromSqlRaw(query).ToList();

                //Por seguridad se filtra resultado
                result = result.Where(x => x.ApplicationUserId == user.Id && x.DeletionDate == null).ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ExecuteSqlCommand(string command)
        {
            try
            {
                dBContext.Database.ExecuteSqlRaw(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
