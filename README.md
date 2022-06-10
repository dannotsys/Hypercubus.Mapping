![Icon](https://raw.githubusercontent.com/dannotsys/Hypercubus.Mapping/main/.github/images/Hypercubus_icon.png)

## Hypercubus Mapper
A fast and minimalist object to object mapper for .Net.\
&nbsp;

### How to start

Add the library to your project using Nuget Package Manager or with the command below:

```bash
dotnet add package Hypercubus.Mapping
```
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

#### Validate your Mappings Rules at Compile Time

Fully automated mapping might seem useful, but it can become a problem when properties are changed or renamed and things stop working and the team doesn't immediately know why. Hypercubus.Mapping always enforces the use of explicit mappings that avoid this kind of situation.\
&nbsp;

#### Similar or even Faster than Traditional Handwritten Code

Why write mappings code by hand every time when you can make this process standard and reuse mapping rules by adopting a mapper library? And with a big plus that makes your code run FASTER.

With very simple POCO classes, Hypercubus Mapper runs typically in a time equivalent to handwriting and rarely adds a maximum of 9% time overhead. Still, when classes become more complex - with other classes referenced - it can save up to 46% mapping processing time.\
&nbsp;

#### Pure .NET Standard 2.0 Code with No Dependencies

This library does not use Reflection.Emit package so it can be used in Xamarin, Mono, and UWP projects without a problem.\
&nbsp;

#### Allows Multiple Profiles

To add multiple profiles just create another Mapper instance and configure its mappings.\
&nbsp;

#### Performance and Memory efficiency
Great performance and low memory footprint using lightweight c# features like delegates and structs plus some nice caching techniques.

#### 1,000,000 Person objects test with a array of Phones and User info properties:

|                        Method |       Mean |    Error |   StdDev |     Median | Ratio |      Gen 0 |      Gen 1 |     Gen 2 | Allocated |
|------------------------------ |-----------:|---------:|---------:|-----------:|------:|-----------:|-----------:|----------:|----------:|
|          'Usual Hand Written' |   568.6 ms | 15.46 ms | 45.58 ms |   537.6 ms |  1.00 | 39000      | 13000      |         - |    244 MB |
|    'Hypercubus InsideForEach' |   355.9 ms |  7.01 ms |  6.55 ms |   355.7 ms |  0.67 | 28000      |  9000      |         - |    184 MB |
|        'Hypercubus MapToList' |   312.2 ms |  6.02 ms |  5.33 ms |   311.0 ms |  0.59 | 28000      |  9000      |         - |    175 MB |
|       'Hypercubus MapToArray' |   312.9 ms |  5.53 ms |  4.31 ms |   312.0 ms |  0.59 | 28000      |  9000      |         - |    175 MB |
| 'ExpressMapper InsideForEach' | 1,125.3 ms | 26.43 ms | 77.52 ms | 1,144.8 ms |  1.99 | 49000      | 17000      | 1000      |    306 MB |
|     'ExpressMapper MapToList' |   782.5 ms | 15.60 ms | 19.73 ms |   783.5 ms |  1.40 | 43000      | 15000      | 1000      |    275 MB |
|    'ExpressMapper MapToArray' |   807.6 ms | 16.02 ms | 32.36 ms |   809.6 ms |  1.44 | 43000      | 15000      | 1000      |    275 MB |
|    'AutoMapper InsideForEach' | 1,279.9 ms | 20.04 ms | 17.76 ms | 1,282.8 ms |  2.40 | 50000      | 17000      | 1000      |    314 MB |
|        'AutoMapper MapToList' | 1,148.5 ms | 18.88 ms | 17.66 ms | 1,142.0 ms |  2.15 | 50000      | 17000      | 1000      |    314 MB |
|       'AutoMapper MapToArray' | 1,120.8 ms | 15.17 ms | 14.19 ms | 1,121.3 ms |  2.10 | 50000      | 17000      | 1000      |    305 MB |
|       'Mapster InsideForEach' |   565.1 ms | 10.13 ms | 13.87 ms |   557.3 ms |  1.00 | 37000      | 12000      |         - |    237 MB |
|           'Mapster MapToList' |   523.2 ms | 10.29 ms | 17.20 ms |   516.7 ms |  0.91 | 37000      | 12000      |         - |    229 MB |
|          'Mapster MapToArray' |   523.3 ms |  7.49 ms |  7.69 ms |   520.6 ms |  0.97 | 37000      | 12000      |         - |    229 MB |

###### * : Mapster can be a little bit faster if NO custom adapter configuration is used for a mapping but Hypercubus.Mapping keeps a more stable and predictable performance for any type of configuration.

|                     Method |     Mean |   Error |   StdDev |   Median | Ratio |      Gen 0 |      Gen 1 |     Gen 2 | Allocated |
|--------------------------- |---------:|--------:|---------:|---------:|------:|-----------:|-----------:|----------:|----------:|
|       'Usual Hand Written' | 549.0 ms | 6.57 ms |  5.13 ms | 549.8 ms |  1.00 | 39000      | 13000      |         - |    244 MB |
| 'Hypercubus InsideForEach' | 362.3 ms | 8.62 ms | 25.43 ms | 347.3 ms |  0.72 | 28000      |  9000      |         - |    184 MB |
|     'Hypercubus MapToList' | 321.2 ms | 5.34 ms |  4.73 ms | 322.3 ms |  0.59 | 28000      |  9000      |         - |    175 MB |
|    'Hypercubus MapToArray' | 313.7 ms | 4.59 ms |  3.59 ms | 313.9 ms |  0.57 | 28000      |  9000      |         - |    175 MB |
|    'Mapster InsideForEach' | 352.4 ms | 5.34 ms |  4.46 ms | 352.7 ms |  0.64 | 30000      | 10000      | 1000      |    191 MB |
|        'Mapster MapToList' | 307.9 ms | 6.01 ms |  5.90 ms | 307.2 ms |  0.56 | 29000      | 10000      |         - |    183 MB |
|       'Mapster MapToArray' | 299.7 ms | 3.59 ms |  3.00 ms | 299.4 ms |  0.55 | 29000      | 10000      |         - |    183 MB |

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
Hypercubus.Mapping 0.5.2\
Mapster 7.2.0

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1706 (21H1/May2021Update)
Intel Core i5-8250U CPU 1.60GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET SDK=6.0.100
  [Host]     : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT
  DefaultJob : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT
