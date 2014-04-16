using MoG.Domain.Models;
using MoG.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain.Service
{
    public class ParameterService : IParameterService
    {
        private IParameterRepository repo = null;

        public ParameterService(IParameterRepository repo)
        {
            this.repo = repo;
        }


        public int Parameter(string key, string value)
        {


            Parameter test = this.repo.GetByKey(key);
            if (test != null)
            {
                return test.Id;
            }

            Parameter item = new Parameter()
            {
                Key = key,
                Value = value

            };
            int createdId = this.repo.Create(item);


            return createdId;
        }



        private bool SaveChanges(Parameter item)
        {
            return this.repo.SaveChanges(item);
        }


        public bool Delete(int id)
        {
            Parameter item = this.GetById(id);

            return this.repo.Delete(item); ;
        }



        public Parameter GetById(int id)
        {
            return this.repo.GetById(id);

        }




        public Parameter GetByKey(string p)
        {
            return this.repo.GetByKey(p);
        }
    }

    public interface IParameterService
    {

        int Parameter(string email, string ip);
        Parameter GetById(int id);


        bool Delete(int id);


        Parameter GetByKey(string p);
    }
}
