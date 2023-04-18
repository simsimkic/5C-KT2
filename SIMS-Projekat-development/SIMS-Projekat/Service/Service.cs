using Microsoft.Win32;
using SIMS_Projekat.Repository;
using SIMS_Projekat.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_Projekat.Service
{
    public class Service<T> where T : ISerializable, new()
    {
        protected Repository<T> _repository;
        public Service(string filePath)
        {
            _repository = new Repository<T>(filePath);
        }
        public List<T> GetAll()
        {
            return _repository.GetAll();
        }
        public T Save(T entity)
        {
            return _repository.Save(entity);
        }
        public void Delete(T entity)
        {
            _repository.Delete(entity);
        }

        public T Update(T entity)
        {
            return _repository.Update(entity);
        }

        public T GetById(uint id)
        {
            return _repository.GetById(id);
        }

        public IEnumerable<T> Get(Func<T, bool> predicate)
        {
            return _repository.GetAll().Where(predicate);
        }

        public T GetFirst(Func<T, bool> predicate)
        {
            return _repository.GetAll().FirstOrDefault(predicate);
        }
    }
}
