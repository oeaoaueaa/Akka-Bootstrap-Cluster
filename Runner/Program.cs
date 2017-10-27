using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            //CurrentDirectory: C:\Repos\Misc\Akka-Bootstrap-Cluster\Runner
            //Akka-Bootstrap-Cluster\Akka.Bootstrap.Cluster.Node1\bin\Debug\netcoreapp2.0\Akka.Bootstrap.Cluster.Node1.dll

            var solutionRoot = Environment.CurrentDirectory.Replace("\\Runner", string.Empty);
            var producerPath =
                $@"{solutionRoot}\Akka.Bootstrap.Cluster.Node2\bin\Debug\netcoreapp2.0\Akka.Bootstrap.Cluster.Node2.dll";
            var consumerPath =
                $@"{solutionRoot}\Akka.Bootstrap.Cluster.Node1\bin\Debug\netcoreapp2.0\Akka.Bootstrap.Cluster.Node1.dll";

            var producerCount = 4;
            var consumerCount = 4;

            var producerProcesses = Enumerable.Range(1, producerCount).Select(i => Process.Start($"dotnet",  $"\"{producerPath}\" {i}")).ToList();
            var consumerProcesses = Enumerable.Range(1, consumerCount).Select(i => Process.Start($"dotnet", $"\"{consumerPath}\" {i}")).ToList();

            Console.ReadLine();
            producerProcesses.ForEach(p => p.Kill());
            consumerProcesses.ForEach(p => p.Kill());
        }

    }
}
