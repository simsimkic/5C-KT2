using SIMS_Projekat.Serializer;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SIMS_Projekat.Repository
{
    public class Repository<T> where T : ISerializable, new()
    {
        protected string FilePath;
        protected readonly Serializer<T> _serializer;
        protected List<T> _list;

        public Repository(string filePath)
        {
            _serializer = new Serializer<T>();
            FilePath = filePath;
            if (!File.Exists(FilePath))
            {
                FileStream fs = File.Create(FilePath);
                fs.Close();
            }
            _list = _serializer.FromCSV(FilePath);
        }

        public uint NextId()
        {
            _list = _serializer.FromCSV(FilePath);
            if (_list.Count < 1)
            {
                return 1;
            }
            return _list.Max(c => c.Id) + 1;
        }
        public List<T> GetAll()
        {
            return _serializer.FromCSV(FilePath).Where(x => !x.IsDeleted).ToList();
        }
        public T Save(T entity)
        {
            entity.Id = NextId();
            _list = _serializer.FromCSV(FilePath);
            _list.Add(entity);
            if (!_serializer.ToCSV(FilePath, _list))
            {
                return default(T);
            }
            return entity;
        }
        public void Delete(T entity)
        {
            _list = _serializer.FromCSV(FilePath);
            T foundEntity = _list.Find(c => c.Id == entity.Id && !c.IsDeleted);
            if (foundEntity == null) return;
            foundEntity.IsDeleted = true;
            //_list.Remove(foundEntity);
            _serializer.ToCSV(FilePath, _list);
        }

        public T Update(T entity)
        {
            _list = _serializer.FromCSV(FilePath);
            T current = _list.Find(c => c.Id == entity.Id && !c.IsDeleted);
            if (current == null) return default(T);
            int index = _list.IndexOf(current);
            _list.Remove(current);
            _list.Insert(index, entity);       // keep ascending order of ids in file 
            _serializer.ToCSV(FilePath, _list);
            return entity;
        }

        public T GetById(uint id)
        {
            _list = _serializer.FromCSV(FilePath);
            return _list.FirstOrDefault(c => c.Id == id && !c.IsDeleted);
        }
    }
}
