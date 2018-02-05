using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sap.SmartAccounting.Mvc.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sap.SmartAccounting.Mvc.Services.Tests
{
    [TestClass()]
    public class AlgorithmV1Tests
    {
        [TestMethod()]
        public void LinqForSubtotalTest()
        {
            var list = new List<Student>
            {
                new Student {Name = "Cyrano", Gender = true, Age = 35},
                new Student {Name = "Michael", Gender = true, Age = 38},
                new Student {Name = "Drew", Gender = true, Age = 37},
                new Student {Name = "Morgan", Gender = false, Age = 33}
            };

            var query = from p in list where p.Age < 38
                group p by p.Gender into g
                orderby g.Count() descending 
                select new { g.Key, Counts = g.Count() };

            Assert.IsTrue(query.Any());
        }

        private class Student
        {
            public string Name { get; set; }
            public bool Gender { get; set; }
            public int Age { get; set; }
        }
    }
}