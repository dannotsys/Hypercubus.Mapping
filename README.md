![Icon](https://avatars.githubusercontent.com/u/29736865?s=128&v=4)

## Hypercubus Mapper
A fast and minimalist object to object mapper for .Net.

### Usage

All the mapping is managed by the `Hypercubus.Mapping.Mapper` class. You can use it with your favorite dependency injection framework to inject the mapper instance in your controller classes for example.

#### Configuration
```csharp
services.AddSingleton(sc =>
{
    var mapper = new Hypercubus.Mapping.Mapper();

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

#### Pure .NET Standard 1.6 Code with No Dependencies

This library does not use Reflection.Emit package so it can be used in Xamarin, Mono and UWP projects with no problem.


#### Allows Multiple Profiles

To add multiple profiles just create another mapper instance and configure its mappings.


#### Performance and Memory efficient
Very performant and wise at memory usage by using simple structures like delegates and caching.

#### 1,000,000 records test:

|                     Method |       Mean |    Error |   StdDev | Ratio |      Gen 0 |      Gen 1 |     Gen 2 | Allocated |
|--------------------------- |-----------:|---------:|---------:|------:|-----------:|-----------:|----------:|----------:|
|             'Hand Written' |   768.8 ms |  7.23 ms |  6.41 ms |  1.00 | 40000      | 13000      |         - |    244 MB |
| 'Hypercubus InsideForEach' |   493.6 ms |  5.01 ms |  4.44 ms |  0.64 | 28000      |  9000      |         - |    184 MB |
|     'Hypercubus MapToList' |   451.2 ms |  8.38 ms | 23.07 ms |  0.62 | 28000      |  9000      |         - |    184 MB |
|    'Hypercubus MapToArray' |   417.6 ms |  5.68 ms |  4.75 ms |  0.54 | 28000      |  9000      |         - |    175 MB |
| 'AutoMapper InsideForEach' | 1,648.2 ms | 20.49 ms | 19.17 ms |  2.14 | 51000      | 18000      | 1000      |    314 MB |
|     'AutoMapper MapToList' | 1,478.3 ms | 15.44 ms | 14.45 ms |  1.92 | 51000      | 18000      | 1000      |    314 MB |
|    'AutoMapper MapToArray' | 1,457.6 ms | 10.26 ms |  8.01 ms |  1.90 | 51000      | 18000      | 1000      |    305 MB |
|   'Mapster* InsideForEach' |   764.9 ms |  6.16 ms |  5.14 ms |  0.99 | 37000      | 12000      |         - |    237 MB |
|       'Mapster* MapToList' |   648.4 ms |  8.02 ms |  7.11 ms |  0.84 | 37000      | 12000      |         - |    229 MB |
|      'Mapster* MapToArray' |   645.7 ms |  7.33 ms |  6.12 ms |  0.84 | 37000      | 12000      |         - |    229 MB |

###### * : Mapster can be faster if no custom adapter configuration is used. If your Dtos classes are usually identical to your business/entity classes then Mapster would be a better option

|                     Method |       Mean |    Error |    StdDev | Ratio |      Gen 0 |      Gen 1 |     Gen 2 | Allocated |
|--------------------------- |-----------:|---------:|----------:|------:|-----------:|-----------:|----------:|----------:|
|             'Hand Written' |   782.8 ms | 15.15 ms |  20.22 ms |  1.00 | 40000      | 13000      |         - |    244 MB |
| 'Hypercubus InsideForEach' |   496.4 ms |  8.64 ms |   7.66 ms |  0.64 | 28000      |  9000      |         - |    184 MB |
|     'Hypercubus MapToList' |   441.5 ms |  7.60 ms |   6.74 ms |  0.57 | 28000      |  9000      |         - |    184 MB |
|    'Hypercubus MapToArray' |   420.2 ms |  6.93 ms |  12.85 ms |  0.54 | 28000      |  9000      |         - |    175 MB |
|    'Mapster InsideForEach' |   461.5 ms |  9.22 ms |  26.15 ms |  0.59 | 29000      | 10000      |         - |    191 MB |
|        'Mapster MapToList' |   357.8 ms |  6.77 ms |  10.93 ms |  0.46 | 29000      | 10000      |         - |    183 MB |
|       'Mapster MapToArray' |   346.0 ms |  6.79 ms |  12.76 ms |  0.44 | 29000      | 10000      |         - |    183 MB |

###### * Legends *
 Mean      : Arithmetic mean of all measurements\
  Error     : Half of 99.9% confidence interval\
  StdDev    : Standard deviation of all measurements\
  Median    : Value separating the higher half of all measurements (50th percentile)\
  Ratio     : Mean of the ratio distribution ([Current]/[Baseline])\
  RatioSD   : Standard deviation of the ratio distribution ([Current]/[Baseline])\
  Gen 0     : GC Generation 0 collects per 1000 operations\
  Gen 1     : GC Generation 1 collects per 1000 operations\
  Gen 2     : GC Generation 2 collects per 1000 operations\
  Allocated : Allocated memory per single operation (managed only, inclusive, 1KB = 1024B)\
  1 ms      : 1 Millisecond (0.001 sec)
