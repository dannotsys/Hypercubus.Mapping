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
        UserLog = m.Map(s.UserLog).To<UserDto>(), // New Syntax!
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
List<PersonDto> personsDto = mapper.Map(persons).To<List<PersonDto>>();
```
If you use a single set of mapping rules, i.e. a single Mapper class, you can use a shorter and easier to write mapping extension method:

```csharp
List<PersonDto> personsDto = persons.Map().To<List<PersonDto>>();
```
This will be based on the default Mapper which is the first Mapper created in the current process and can also be changed when needed:
```csharp
Mapper.ChangeDefault(alternateMapper);
```
&nbsp;

### Why use this library?
&nbsp;

#### WYSIWYG and No Complex Configurations

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

|                     Method |       Mean |    Error |   StdDev |     Median | Ratio |      Gen 0 |      Gen 1 |     Gen 2 | Allocated |
|--------------------------- |-----------:|---------:|---------:|-----------:|------:|-----------:|-----------:|----------:|----------:|
|             'Hand Written' |   714.6 ms | 12.61 ms | 11.79 ms |   709.8 ms |  1.00 | 40000      | 13000      |         - |    244 MB |
| 'Hypercubus InsideForEach' |   434.8 ms |  2.18 ms |  1.71 ms |   434.6 ms |  0.61 | 28000      |  9000      |         - |    184 MB |
|     'Hypercubus MapToList' |   395.2 ms |  3.83 ms |  3.20 ms |   395.8 ms |  0.55 | 28000      |  9000      |         - |    184 MB |
|    'Hypercubus MapToArray' |   383.9 ms |  7.30 ms |  6.10 ms |   383.8 ms |  0.54 | 28000      |  9000      |         - |    175 MB |
| 'AutoMapper InsideForEach' | 1,538.9 ms | 14.76 ms | 12.32 ms | 1,537.6 ms |  2.15 | 51000      | 18000      | 1000      |    314 MB |
|     'AutoMapper MapToList' | 1,387.4 ms | 16.82 ms | 15.73 ms | 1,381.2 ms |  1.94 | 51000      | 18000      | 1000      |    314 MB |
|    'AutoMapper MapToArray' | 1,388.6 ms | 27.38 ms | 26.89 ms | 1,373.6 ms |  1.94 | 51000      | 18000      | 1000      |    305 MB |
|    'Mapster InsideForEach' |   691.6 ms |  6.65 ms |  5.19 ms |   691.6 ms |  0.97 | 37000      | 12000      |         - |    237 MB |
|        'Mapster MapToList' |   611.3 ms | 10.57 ms | 24.51 ms |   602.2 ms |  0.85 | 37000      | 12000      |         - |    229 MB |
|       'Mapster MapToArray' |   595.3 ms |  7.23 ms |  6.04 ms |   594.6 ms |  0.83 | 37000      | 12000      |         - |    229 MB |

###### * : Mapster can be a bit faster if NO custom adapter configuration is used for a mapping and if there is no missing mapped properties. But if your Dtos classes are usually different from your business/entity classes maybe Hypercubus.Mapping would be a better option for you.

|                     Method |     Mean |   Error |   StdDev | Ratio |      Gen 0 |      Gen 1 | Allocated |
|--------------------------- |---------:|--------:|---------:|------:|-----------:|-----------:|----------:|
|             'Hand Written' | 718.4 ms | 2.41 ms |  1.88 ms |  1.00 | 40000      | 13000      |    244 MB |
| 'Hypercubus InsideForEach' | 397.8 ms | 4.59 ms |  4.07 ms |  0.55 | 28000      |  9000      |    184 MB |
|     'Hypercubus MapToList' | 434.7 ms | 3.79 ms |  3.36 ms |  0.61 | 28000      |  9000      |    184 MB |
|    'Hypercubus MapToArray' | 374.8 ms | 5.37 ms |  4.48 ms |  0.52 | 28000      |  9000      |    175 MB |
|    'Mapster InsideForEach' | 416.4 ms | 8.07 ms | 14.97 ms |  0.60 | 29000      | 10000      |    191 MB |
|        'Mapster MapToList' | 309.8 ms | 5.87 ms |  6.28 ms |  0.43 | 29000      | 10000      |    183 MB |
|       'Mapster MapToArray' | 327.8 ms | 5.09 ms | 11.18 ms |  0.44 | 29000      | 10000      |    183 MB |

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
