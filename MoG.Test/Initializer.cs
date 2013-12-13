using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoG;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Test
{
  [TestClass()]
    public class Initializer
    {
      [AssemblyInitialize()]
      public static void AssemblyInit(TestContext context)
      {

          Database.SetInitializer(new MyDataContextDbInitializer());

          MoG.Domain.Repository.MogDbContext dbContext = new MoG.Domain.Repository.MogDbContext();
       
          dbContext.Database.Initialize(false);
         
      }
    }
}
