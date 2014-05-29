﻿using System;
using GigaSpaces.Core;
using GigaSpaces.Core.Executors;
using GigaSpaces.Core.Executors.Tasks;
using GigaSpaces.Core.Metadata;

namespace ManualIntegration.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var spaceProxy = GigaSpacesFactory.FindSpace("jini://*/*/manualIntegrationRunner?groups=skyler-group");

            spaceProxy.Write(new TestData() {ImportantNumber = 1, RouteId = 0});
            spaceProxy.Write(new TestData() {ImportantNumber = 2, RouteId = 0});
            spaceProxy.Write(new TestData() {ImportantNumber = 3, RouteId = 0});
            spaceProxy.Write(new TestData() {ImportantNumber = 10, RouteId = 1});
            spaceProxy.Write(new TestData() {ImportantNumber = 10, RouteId = 1});
            spaceProxy.Write(new TestData() {ImportantNumber = 10, RouteId = 1});

            ISpaceTask<long> averageTask = new AverageTask<TestData, int>(t => t.ImportantNumber);
            var result = spaceProxy.Execute(averageTask, 0);

            Console.WriteLine("-----------");
            Console.WriteLine("Result {0}.", result);
            Console.ReadKey();
        }
    }

    [SpaceClass]
    public class TestData
    {
        
        [SpaceID(AutoGenerate = true)]
        public string Id { get; set; }

        public int ImportantNumber { get; set; }

        [SpaceRouting]
        public int RouteId { get; set; }
    }
}
