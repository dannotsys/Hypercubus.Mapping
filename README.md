![Icon](https://raw.githubusercontent.com/dannotsys/Hypercubus.Mapping/main/.github/images/Hypercubus_icon.png)

## Hypercubus Mapper
A fast and minimalist object to object mapper for .Net.\
&nbsp;

### Usage

All the configured mapping is managed by the `Hypercubus.Mapping.Mapper` class. You can use it with your favorite dependency injection framework to inject the mapper instance in your Asp.Net controller classes.\
&nbsp;

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
IEnumerable, Array, and List configurations are automatically added to your configured types by default. You can disable this for a individual Type by using the overload of the `Configure` method that has a `addDefaultEnumerableTypes` parameter.\
&nbsp;

#### Mapper Usage
After injecting it in your controllers just make a simple call like this:

```csharp
List<PersonDto> personsDto = mapper.Map<IEnumerable<Person>, List<PersonDto>>(persons);
```
If you use a single set of mappings profiles, i.e. a single Mapper class, you can use a shorter and easier to write mapping extension method:

```csharp
List<PersonDto> personsDto = persons.Maps().To<List<PersonDto>>();
```
These will be based on the default Mapper which is the first Mapper created in the current process and can also be changed when needed:
```csharp
Mapper.ChangeDefault(alternateMapper);
```
&nbsp;

### Why use this library?
&nbsp;

#### No Complex Configurations and Library Bachelor Classes Needed

Keep it Simple, Silly :wink:\
&nbsp;

#### Similar or even Faster than Traditional Handwritten Code

Why write mappings code by hand every time when you can make this process standard and reuse mapping rules by adopting a mapper library? And with a big plus that makes your code run FASTER.

With very simple POCO classes, Hypercubus Mapper runs typically in a time equivalent to handwriting and rarely adds a maximum of 9% time overhead. Still, when classes become more complex - with other classes referenced - it can save up to 46% mapping processing time.\
&nbsp;

#### Pure .NET Standard 1.6 Code with No Dependencies

This library does not use Reflection.Emit package so it can be used in Xamarin, Mono, and UWP projects without a problem.\
&nbsp;

#### Allows Multiple Profiles

To add multiple profiles just create another Mapper instance and configure its mappings.\
&nbsp;

#### Performance and Memory efficiency
Great performance and smart memory usage by using simple structures like delegates and caching techniques.

#### 1,000,000 Person objects test with Phones list and User info:

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

###### * : Mapster can be faster if NO custom adapter configuration is used for a mapping. But if your Dtos classes are usually different from your business/entity classes maybe Hypercubus.Mapping would be a better option for you.

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

###### * Test Configuration *

AutoMapper 11.0.1\
Hypercubus.Mapping 0.1.14\
Mapster 7.2.0

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1466 (21H1/May2021Update)\
Intel Core i5-8250U CPU 1.60GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores\
.NET SDK=6.0.100 
