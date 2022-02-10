![Icon](https://avatars.githubusercontent.com/u/29736865?s=128&v=4)

## Hypercubus Mapper
A fast and minimalist object to object mapper for .Net.

### Usage

All the mapping is managed by the `Hypercubus.Mapping.Mapper` struct. You can use it with your favorite dependency injection framework to inject the mapper instance in your controller classes for example.

#### Configuration
```csharp
services.AddSingleton(sc =>
{
    IMapper mapper = new Hypercubus.Mapping.Mapper();

    mapper.Configure<Phone, PhoneDto>((m, s) => new PhoneDto()
    {
        Id = s.Id,
        Number = s.Number
    });

    mapper.Configure<User, UserDto>((m, s) => new UserDto()
    {
        Id = s.Id,
        UserName = s.UserName
    });

    mapper.Configure<Person, PersonDto>((m, s) => new PersonDto()
    {
        Id = s.Id,
        Name = s.Name,
        Age = s.Age,
        UserLog = m.Map<User, UserDto>(s.UserLog),
        Phones = m.Map<List<Phone>, PhoneDto[]>(s.Phones)
    });

    return mapper;
});
```
By default IEnumerables, Arrays and Lists are automatically configured for your configured types. You can disable this for a individual Type by using the overload of the `Configure` method that has a `addDefaultEnumerableTypes` parameter.

#### Usage
After injecting it in your controllers just make a simple call like this:

```csharp
List<PersonDto> personsDto = mapper.Map<IEnumerable<Person>, List<PersonDto>>(persons);
```

### Why use this library?
#### Performance and Memory efficient
Very performant and wise at memory usage by using simple structures like delegates and caching.

1,000,000 records test:

|                    Method |       Mean |    Error |    StdDev | Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------------------- |-----------:|---------:|----------:|------:|------:|------:|----------:|
|               HandWritten |   789.0 ms | 15.78 ms |  21.60 ms | 40000 | 13000 |     - |    253 MB |
|    UsingImplicitOperators |   790.3 ms | 15.58 ms |  14.58 ms | 40000 | 13000 |     - |    253 MB |
|   HypercubusInsideForEach |   653.0 ms | 11.61 ms |  15.90 ms | 34000 | 11000 |     - |    222 MB |
|     HypercubusMappingList |   724.5 ms | 26.51 ms |  71.67 ms | 34000 | 11000 |     - |    222 MB |
|    HypercubusMappingArray |   705.6 ms | 19.15 ms |  54.33 ms | 34000 | 11000 |     - |    214 MB |
|   AutoMapperInsideForEach | 1,723.2 ms | 34.33 ms |  88.01 ms | 51000 | 18000 |  1000 |    314 MB |
|     AutoMapperMappingList | 1,570.1 ms | 42.63 ms | 120.95 ms | 51000 | 18000 |  1000 |    314 MB |
|    AutoMapperMappingArray | 1,568.8 ms | 54.24 ms | 152.99 ms | 51000 | 18000 |  1000 |    305 MB |
|      HiglaboInsideForEach | 1,365.6 ms | 14.60 ms |  12.19 ms | 60000 | 16000 |     - |    375 MB |
|        HiglaboMappingList | 1,362.8 ms | 18.48 ms |  16.38 ms | 60000 | 16000 |     - |    375 MB |

###### * Legends *
  Mean      : Arithmetic mean of all measurements\
  Error     : Half of 99.9% confidence interval\
  StdDev    : Standard deviation of all measurements\
  Median    : Value separating the higher half of all measurements (50th percentile)\
  Gen 0     : GC Generation 0 collects per 1000 operations\
  Gen 1     : GC Generation 1 collects per 1000 operations\
  Gen 2     : GC Generation 2 collects per 1000 operations\
  Allocated : Allocated memory per single operation (managed only, inclusive, 1KB = 1024B)\
  1 ms      : 1 Millisecond (0.001 sec)

#### Allows Multiple Profiles

To have multiple profiles just create another mapper instance and configure its mappings.
