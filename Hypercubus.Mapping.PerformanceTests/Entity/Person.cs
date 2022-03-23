using System;
using System.Collections.Generic;

namespace Hypercubus.Mapping.PerformanceTests.Entity
{
    public class Person
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public int Age { get; set; }

        public List<Phone> Phones { get; set; }
    }
}
