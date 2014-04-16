using MoG.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace MoG.Domain.Repository
{
    public class ParameterRepository : BaseRepository, IParameterRepository
    {

        public ParameterRepository(IdbContextProvider provider)
            : base(provider)
        {

        }





        public int Create(Parameter item)
        {
            this.dbContext.Parameters.Add(item);
            this.dbContext.SaveChanges();
            return item.Id;
        }




        public bool SaveChanges(Parameter item)
        {
            this.dbContext.Entry(item).State = System.Data.Entity.EntityState.Modified;
            int result = this.dbContext.SaveChanges();
            return result > 0;
        }



        public Parameter GetById(int id)
        {
            return this.dbContext.Parameters.Find(id);
        }

        public Parameter GetByKey(string key)
        {
            return this.dbContext.Parameters.Where(i => i.Key == key).FirstOrDefault();
        }

        public bool Delete(Parameter item)
        {

            if (item != null)
            {
                this.dbContext.Parameters.Remove(item);
                this.dbContext.SaveChanges();
                return true;
            }
            return false;
        }
    }

    public interface IParameterRepository
    {


        Parameter GetById(int id);

       int Create(Parameter item);


       bool SaveChanges(Parameter item);

       Parameter GetByKey(string email);

       bool Delete(Parameter item);
    }
}
