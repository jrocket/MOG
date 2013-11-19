using AutoMapper;
using MoG.Domain.Models;
using MoG.Domain.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoG
{
    public class MogAutomapper
    {
        public static void RegisterAutomapper()
        {
         
            //Mapper.CreateMap<Project, VMProjectList>();
        }

        //public static IEnumerable<VMProjectList> Map(IEnumerable<Project> projects)
        //{
        //    return AutoMapper.Mapper.Map<IEnumerable<Project>, IEnumerable<VMProjectList>>(projects);
        //}
    }
}