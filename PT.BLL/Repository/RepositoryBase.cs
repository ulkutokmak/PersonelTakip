using PT.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PT.BLL.Repository
{
    public class RepositoryBase<T, TId> where T : class, new()
    {
        protected internal MyContext dbContext; //başka projeden erişilmememesi için-diğer türlü UI'dan erişilebilir.
        public virtual List<T> GetAll()
        {
            try
            {
                dbContext = new MyContext(); 
                return dbContext.Set<T>().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public virtual T GetByID(TId id)
        {
            try
            {
                dbContext = new MyContext();
                return dbContext.Set<T>().Find(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public virtual int Insert(T entity)
        {
            try
            {
                dbContext = dbContext ?? new MyContext(); //dbContext boşsa newle değilse var olanı kullan DbSingleTone Mantığıyla.
                dbContext.Set<T>().Add(entity);
                return dbContext.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public virtual int Delete(T entity) //virtual olmasının nedeni override edebiliriz diye.
        {
            try
            {
                dbContext = dbContext ?? new MyContext(); //dbContext boşsa newle değilse var olanı kullan DbSingleTone Mantığıyla.
                dbContext.Set<T>().Remove(entity);
                return dbContext.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public virtual int Update() 
        {
            try
            {
                dbContext = dbContext ?? new MyContext(); 
                return dbContext.SaveChanges(); //SaveChanges() kaç kayıtın etkilendiğini int olarak döndürür.
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
