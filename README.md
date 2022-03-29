![Icon](https://raw.githubusercontent.com/dannotsys/Hypercubus.Mapping/main/.github/images/Hypercubus_icon.png)

## Hypercubus Mapper
A fast and minimalist object to object mapper for .Net.\
&nbsp;

### How to start

Add the library to your project using Nuget Package Manager or with the command below:

```bash
dotnet add package Hypercubus.Mapping
```

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

#### Validate your Mappings Rules at Compile Time

Fully automated mapping might seem useful, but it can become a problem when properties are changed or renamed and things stop working and the team doesn't immediately know why. Hypercubus.Mapping always enforces the use of explicit mappings that avoid this kind of situation.\
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
Great performance and low memory footprint using lightweight c# features like delegates and structs plus some nice caching techniques.

#### 1,000,000 Person objects test with a array of Phones and User info properties:

|                        Method |       Mean |    Error |   StdDev | Ratio | RatioSD |      Gen 0 |      Gen 1 |     Gen 2 | Allocated |
|------------------------------ |-----------:|---------:|---------:|------:|--------:|-----------:|-----------:|----------:|----------:|
|          'Usual Hand Written' |   656.1 ms |  1.78 ms |  1.49 ms |  1.00 |    0.00 | 39000      | 13000      |         - |    244 MB |
|    'Hypercubus InsideForEach' |   373.0 ms |  7.41 ms | 12.58 ms |  0.58 |    0.02 | 28000      |  9000      |         - |    184 MB |
|        'Hypercubus MapToList' |   346.9 ms |  6.05 ms |  5.05 ms |  0.53 |    0.01 | 28000      |  9000      |         - |    175 MB |
|       'Hypercubus MapToArray' |   343.0 ms |  6.64 ms | 10.54 ms |  0.52 |    0.02 | 28000      |  9000      |         - |    175 MB |
| 'ExpressMapper InsideForEach' | 1,115.4 ms | 12.26 ms | 11.47 ms |  1.70 |    0.02 | 49000      | 17000      | 1000      |    306 MB |
|     'ExpressMapper MapToList' |   730.7 ms | 14.32 ms | 18.11 ms |  1.11 |    0.03 | 43000      | 15000      | 1000      |    275 MB |
|    'ExpressMapper MapToArray' |   722.9 ms | 14.35 ms | 20.11 ms |  1.11 |    0.03 | 43000      | 15000      | 1000      |    275 MB |
|    'AutoMapper InsideForEach' | 1,760.6 ms | 13.74 ms | 11.47 ms |  2.68 |    0.02 | 57000      | 20000      | 1000      |    352 MB |
|        'AutoMapper MapToList' | 1,619.8 ms | 12.26 ms | 10.87 ms |  2.47 |    0.01 | 57000      | 20000      | 1000      |    352 MB |
|       'AutoMapper MapToArray' | 1,608.5 ms | 17.69 ms | 16.55 ms |  2.45 |    0.03 | 57000      | 20000      | 1000      |    343 MB |
|       'Mapster InsideForEach' |   734.4 ms |  6.02 ms |  5.03 ms |  1.12 |    0.01 | 37000      | 12000      |         - |    237 MB |
|           'Mapster MapToList' |   569.7 ms |  4.79 ms |  4.00 ms |  0.87 |    0.00 | 37000      | 12000      |         - |    229 MB |
|          'Mapster MapToArray' |   570.9 ms |  2.27 ms |  2.01 ms |  0.87 |    0.00 | 37000      | 12000      |         - |    229 MB |

###### * : Mapster can be a bit faster if NO custom adapter configuration is used for a mapping but Hypercubus.Mapping keeps a more stable and predictable performance for any type of configuration.

|                     Method |     Mean |    Error |   StdDev |   Median | Ratio |      Gen 0 |      Gen 1 | Allocated |
|--------------------------- |---------:|---------:|---------:|---------:|------:|-----------:|-----------:|----------:|
|       'Usual Hand Written' | 699.3 ms | 13.96 ms | 24.81 ms | 688.4 ms |  1.00 | 40000      | 13000      |    244 MB |
| 'Hypercubus InsideForEach' | 370.6 ms |  4.74 ms |  4.87 ms | 370.7 ms |  0.52 | 28000      |  9000      |    184 MB |
|     'Hypercubus MapToList' | 353.5 ms |  2.92 ms |  2.44 ms | 354.6 ms |  0.50 | 28000      |  9000      |    175 MB |
|    'Hypercubus MapToArray' | 346.9 ms |  4.16 ms |  3.25 ms | 347.7 ms |  0.49 | 28000      |  9000      |    175 MB |
|    'Mapster InsideForEach' | 402.7 ms |  7.13 ms |  9.02 ms | 400.1 ms |  0.57 | 29000      | 10000      |    191 MB |
|        'Mapster MapToList' | 297.7 ms |  5.93 ms |  6.09 ms | 297.4 ms |  0.42 | 29000      | 10000      |    183 MB |
|       'Mapster MapToArray' | 324.5 ms |  6.39 ms | 10.50 ms | 325.2 ms |  0.46 | 29000      | 10000      |    183 MB |

###### * Legends *
 Mean      : Arithmetic mean of all measurements\
  Error     : Half of 99.9% confidence interval\
  StdDev    : Standard deviation of all measurements\
  Median    : Value separating the higher half of all measurements (50th percentile)\
  Ratio     : Mean of the ratio distribution ([Current]/[Baseline])\
  Gen 0     : GC Generation 0 collects per 1000 operations\
  Gen 1     : GC Generation 1 collects per 1000 operations\
  Gen 2     : GC Generation 2 collects per 1000 operations\
  Allocated : Allocated memory per single operation (managed only, inclusive, 1KB = 1024B)\
  1 ms      : 1 Millisecond (0.001 sec)

###### * Test Configuration *

AutoMapper 11.0.1\
Hypercubus.Mapping 0.2.7\
Mapster 7.2.0

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1466 (21H1/May2021Update)\
Intel Core i5-8250U CPU 1.60GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores\
.NET SDK=6.0.100 
