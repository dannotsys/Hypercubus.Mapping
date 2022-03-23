using Hypercubus.Mapping.PerformanceTests.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hypercubus.Mapping.PerformanceTests.Dto
{
    public class PersonDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public UserDto User { get; set; }

        public IList<PhoneDto> Phones { get; set; }

        public static implicit operator PersonDto(Person person)
        {
            return new PersonDto()
            {
                Name = person.Name,
                Age = person.Age,
                Id = person.Id,
                User = person,
                Phones = person.Phones.Select(p => (PhoneDto)p).ToArray(),
            };
        }
    }
}
