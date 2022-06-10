using BenchmarkDotNet.Attributes;
using Hypercubus.Mapping.PerformanceTests.Dto;
using Hypercubus.Mapping.PerformanceTests.Entity;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hypercubus.Mapping.PerformanceTests
{
    [MemoryDiagnoser]
    public class Benchmark
    {
        List<Person> persons;
        Hypercubus.Mapping.Mapper _hypercubusMapper;
        AutoMapper.IMapper _autoMapperMapper;
        MapsterMapper.Mapper _mapsterMapper;

        [GlobalSetup]
        public void Setup()
        {
            #region HypercubusMapper

            _hypercubusMapper = new Hypercubus.Mapping.Mapper();

            _hypercubusMapper.Configure<Person, UserDto>((m, s) => new UserDto()
            {
                UserName = "Dan Lima"
            });

            _hypercubusMapper.Configure<Phone, PhoneDto>((m, s) => new PhoneDto()
            {
                Number = s.Number
            });

            _hypercubusMapper.Configure<Person, PersonDto>((m, s) => new PersonDto()
            {
                Name = s.Name,
                Age = s.Age,
                Id = s.Id,
                User = s.Map().To<UserDto>(),
                Phones = s.Phones.Map().To<PhoneDto[]>(),
            });

            #endregion

            #region ExpressMapper

            ExpressMapper.Mapper.Register<Person, UserDto>()
                .Member(dest => dest.UserName,
                            src => "Dan Lima");

            ExpressMapper.Mapper.Register<Person, PersonDto>()
                .Member(dest => dest.User,
                            src => src)
                ;

            #endregion

            #region AutoMapperMapper

            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Phone, PhoneDto>()
                    .ConstructUsing((s) => new PhoneDto()
                    {
                        Number = s.Number
                    });

                cfg.CreateMap<Person, UserDto>()
                    .ConstructUsing((s) => new UserDto()
                    {
                        UserName = "Dan Lima"
                    });

                cfg.CreateMap<Person, PersonDto>()
                    .ConstructUsing((s, c) => new PersonDto()
                    {
                        Name = s.Name,
                        Age = s.Age,
                        Id = s.Id,
                        User = c.Mapper.Map<Person, UserDto>(s),
                        Phones = c.Mapper.Map<List<Phone>, PhoneDto[]>(s.Phones)
                    });
            });

            _autoMapperMapper = config.CreateMapper();

            #endregion

            #region MapsterMapper

            var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;

            typeAdapterConfig
                .NewConfig<Person, UserDto>()
                .Map(dest => dest.UserName,
                    src => "Dan Lima");

            typeAdapterConfig
                .NewConfig<Person, PersonDto>()
                .Map(dest => dest.User,
                    src => src.Adapt<UserDto>());

            _mapsterMapper = new MapsterMapper.Mapper(typeAdapterConfig);

            #endregion

            var personsAux = new List<Person>();
            Random rnd = new Random();

            for (int i = 0; i < 1000000; i++)
            {
                personsAux.Add(new Person()
                {
                    Name = "User" + i.ToString(),
                    Age = Math.Abs(rnd.Next(1, 99)),
                    Id = Guid.NewGuid(),
                    Phones = new List<Phone> ( new[] { new Phone() { Number = (2100000000L + Math.Abs(rnd.Next(10000000, 99999999))) }, new Phone() { Number = (2100000000L + Math.Abs(rnd.Next(10000000, 99999999))) } } )
                });
            }

            persons = personsAux;
        }
        
        [Benchmark(Description = "Usual Hand Written", Baseline = true)]
        public void DirectAttrib()
        {
            PersonDto[] personsDto = persons.Select(person => new PersonDto()
            {
                Name = person.Name,
                Age = person.Age,
                Id = person.Id,
                User = new UserDto()
                {
                    UserName = "Dan Lima"
                },
                Phones = person.Phones.Select(p => new PhoneDto() { Number = p.Number }).ToArray()
            }).ToArray();
        }
        /*
        [Benchmark(Description = "Implicit Operator")]
        public void DirectOperator()
        {
            PersonDto[] personsDto = persons.Select(person => (PersonDto)person).ToArray();

            personsDto = null;
        }
       */
        [Benchmark(Description = "Hypercubus InsideForEach")]
        public void HypercubusMapper()
        {
            List<PersonDto> personsDto = new List<PersonDto>();

            foreach (var person in persons)
            {
                personsDto.Add(_hypercubusMapper.Map<Person, PersonDto>(person));
            }

            personsDto.Clear();
        }
        
        [Benchmark(Description = "Hypercubus MapToList")]
        public void HypercubusMapperWithList()
        {
            IList<PersonDto> personsDto = _hypercubusMapper.Map<List<Person>, List<PersonDto>>(persons);

            personsDto.Clear();
        }
        
        [Benchmark(Description = "Hypercubus MapToArray")]
        public void HypercubusMapperWithArray()
        {
            IList<PersonDto> personsDto = _hypercubusMapper.Map<List<Person>, PersonDto[]>(persons);
        }

        /* 
        [Benchmark(Description = "Hypercubus MapToArrayFromQueryable")]
        public void HypercubusMapperWithArray2()
        {
            PersonDto[] personsDto = _hypercubusMapper.Map<IEnumerable<Person>, PersonDto[]>(persons.AsQueryable<Person>());

            personsDto = null;
        }
         */

        [Benchmark(Description = "ExpressMapper InsideForEach")]
        public void ExpressMapperMapper()
        {
            List<PersonDto> personsDto = new List<PersonDto>();

            foreach (var person in persons)
            {
                personsDto.Add(ExpressMapper.Mapper.Map<Person, PersonDto>(person));
            }

            personsDto.Clear();
        }

        [Benchmark(Description = "ExpressMapper MapToList")]
        public void ExpressMapperMapperWithList()
        {
            IList<PersonDto> personsDto = ExpressMapper.Mapper.Map<List<Person>, List<PersonDto>>(persons);

            personsDto.Clear();
        }

        [Benchmark(Description = "ExpressMapper MapToArray")]
        public void ExpressMapperMapperWithArray()
        {
            IList<PersonDto> personsDto = ExpressMapper.Mapper.Map<List<Person>, PersonDto[]>(persons);
        }

        [Benchmark(Description = "AutoMapper InsideForEach")]
        public void AutoMapperMapper()
        {
            List<PersonDto> personsDto = new List<PersonDto>();

            foreach (var person in persons)
            {
                personsDto.Add(_autoMapperMapper.Map<Person, PersonDto>(person));
            }

            personsDto.Clear();
        }
        
        [Benchmark(Description = "AutoMapper MapToList")]
        public void AutoMapperMapperWithList()
        {
            List<PersonDto> personsDto = _autoMapperMapper.Map<IEnumerable<Person>, List<PersonDto>>(persons);

            personsDto.Clear();
        }

        [Benchmark(Description = "AutoMapper MapToArray")]
        public void AutoMapperMapperWithArray()
        {
            PersonDto[] personsDto = _autoMapperMapper.Map<IEnumerable<Person>, PersonDto[]>(persons);
        }
          

        [Benchmark(Description = "Mapster InsideForEach")]
        public void MapsterMapper()
        {
            List<PersonDto> personsDto = new List<PersonDto>();

            foreach (var person in persons)
            {
                personsDto.Add(person.Adapt<PersonDto>());
            }

            personsDto.Clear();
        }
        
        [Benchmark(Description = "Mapster MapToList")]
        public void MapsterMapperWithList()
        {
            IList<PersonDto> personsDto = persons.Adapt<List<PersonDto>>();

            personsDto.Clear();
        }
        
        [Benchmark(Description = "Mapster MapToArray")]
        public void MapsterMapperWithArray()
        {
            IList<PersonDto> personsDto = persons.Adapt<PersonDto[]>();
        }

    }
}
