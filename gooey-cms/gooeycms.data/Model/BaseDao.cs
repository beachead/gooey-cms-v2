using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beachead.Persistence.Hibernate;
using NHibernate.Criterion;
using NHibernate;

namespace Gooeycms.Data.Model
{
    public class BaseDao
    {
        public virtual IQuery NewHqlQuery(String hql)
        {
            return Session.CreateQuery(hql);
        }

        public void SaveObject(object entity)
        {
            Session.SaveOrUpdate(entity);
        }

        public void DeleteObject(object entity)
        {
            Session.Delete(entity);
        }

        internal ISession Session
        {
            get { return SessionProvider.Instance.GetOpenSession(); }
        }

        public virtual IList<T> FindAll<T>()
        {
            return Session.CreateCriteria(typeof(T)).List<T>();
        }

        /// <summary>
        /// Finds the object based upon the primary key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T FindByPrimaryKey<T>(object key)
        {
            ICriteria criteria = Session.CreateCriteria(typeof(T))
                .Add(Expression.Eq("Id", key));

            return criteria.UniqueResult<T>();
        }

        /// <summary>
        /// Saves the specified entity object
        /// </summary>
        /// <param name="entity"></param>
        public void Save<T>(T entity)
        {
            SaveObject(entity);
        }

        /// <summary>
        /// Deletes the specified entity object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void Delete<T>(T entity)
        {
            DeleteObject(entity);
        }
    }
}
