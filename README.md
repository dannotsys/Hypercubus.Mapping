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

|                     Method |       Mean |    Error |   StdDev | Ratio |      Gen 0 |      Gen 1 | Allocated |
|--------------------------- |-----------:|---------:|---------:|------:|-----------:|-----------:|----------:|
|             'Hand Written' |   704.9 ms |  5.76 ms |  4.50 ms |  1.00 | 40000      | 13000      |    244 MB |
| 'Hypercubus InsideForEach' |   375.0 ms |  5.65 ms |  4.72 ms |  0.53 | 24000      |  8000      |    161 MB |
|     'Hypercubus MapToList' |   351.2 ms |  7.02 ms |  7.21 ms |  0.50 | 24000      |  8000      |    153 MB |
|    'Hypercubus MapToArray' |   356.7 ms |  6.98 ms |  6.85 ms |  0.51 | 24000      |  8000      |    153 MB |
| 'AutoMapper InsideForEach' | 1,531.2 ms | 30.50 ms | 51.80 ms |  2.19 | 51000      | 18000      |    314 MB |
|     'AutoMapper MapToList' | 1,366.9 ms | 14.08 ms | 12.48 ms |  1.94 | 51000      | 18000      |    314 MB |
|    'AutoMapper MapToArray' | 1,356.7 ms | 17.83 ms | 15.80 ms |  1.93 | 51000      | 18000      |    305 MB |
|   'Mapster* InsideForEach' |   671.5 ms |  5.68 ms |  4.74 ms |  0.95 | 37000      | 12000      |    237 MB |
|       'Mapster* MapToList' |   592.0 ms |  7.08 ms |  5.92 ms |  0.84 | 37000      | 12000      |    229 MB |
|      'Mapster* MapToArray' |   586.1 ms |  5.77 ms |  4.51 ms |  0.83 | 37000      | 12000      |    229 MB |

###### * : Mapster can be a bit faster if NO custom adapter configuration is used for a mapping and if there is no missing mapped properties. But if your Dtos classes are usually different from your business/entity classes then Hypercubus.Mapping would be the right choice for you.

|                     Method |     Mean |    Error |   StdDev |   Median | Ratio |      Gen 0 |      Gen 1 | Allocated |
|--------------------------- |---------:|---------:|---------:|---------:|------:|-----------:|-----------:|----------:|
|             'Hand Written' | 705.7 ms | 11.25 ms | 10.52 ms | 703.7 ms |  1.00 | 40000      | 13000      |    244 MB |
| 'Hypercubus InsideForEach' | 360.8 ms |  7.14 ms | 10.91 ms | 361.7 ms |  0.51 | 24000      |  8000      |    161 MB |
|     'Hypercubus MapToList' | 339.5 ms |  6.67 ms |  6.55 ms | 338.2 ms |  0.48 | 24000      |  8000      |    153 MB |
|    'Hypercubus MapToArray' | 349.9 ms |  2.99 ms |  2.94 ms | 350.4 ms |  0.50 | 24000      |  8000      |    153 MB |
|    'Mapster InsideForEach' | 407.2 ms |  6.01 ms |  4.69 ms | 405.7 ms |  0.58 | 29000      | 10000      |    191 MB |
|        'Mapster MapToList' | 318.4 ms |  6.32 ms | 12.77 ms | 324.2 ms |  0.43 | 29000      | 10000      |    183 MB |
|       'Mapster MapToArray' | 323.6 ms |  6.40 ms | 11.37 ms | 326.2 ms |  0.45 | 29000      | 10000      |    183 MB |

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
Hypercubus.Mapping 0.1.14\
Mapster 7.2.0

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1466 (21H1/May2021Update)\
Intel Core i5-8250U CPU 1.60GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores\
.NET SDK=6.0.100 
