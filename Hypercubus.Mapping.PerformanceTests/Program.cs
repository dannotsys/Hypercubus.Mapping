using BenchmarkDotNet.Running;
using System;

namespace Hypercubus.Mapping.PerformanceTests
{
    internal class Program
    {
        static void Main(string[] args)
        {
#if RELEASE
            var summary = BenchmarkRunner.Run<Benchmark>();
#else
            var benchmark = new Benchmark();
            benchmark.Setup();

            benchmark.HypercubusMapperWithArray();
            
            benchmark.MapsterMapperWithArray();

            benchmark.AutoMapperMapperWithArray();

            benchmark.ExpressMapperMapperWithArray();
#endif
            Console.ReadKey();
        }
    }
}
