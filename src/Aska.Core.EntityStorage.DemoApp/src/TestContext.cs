//using System;
//using System.Diagnostics;
//using Aska.Core.EntityStorage.Abstractions;
//using Aska.Core.EntityStorage.Abstractions.Extensions;
//using Aska.Core.EntityStorage.Ef.MariaDb;
//
//namespace Aska.Core.EntityStorage.DemoApp
//{
//    public class TestContext : AutoDiscoveryMariaDbContext
//    {
//        public TestContext() : base(
//            new StaticConnectionStringProvider<AutoDiscoveryMariaDbContext>(),
//            new TypeDiscoveryProvider<AutoDiscoveryMariaDbContext>())
//        {
//            Console.WriteLine("Test");
//        }
//        
//        public TestContext(IConnectionStringProvider<AutoDiscoveryMariaDbContext> connectionStringProvider, ITypeDiscoveryProvider<AutoDiscoveryMariaDbContext> typeDiscoveryProvider) : base(connectionStringProvider, typeDiscoveryProvider)
//        {
//        }
//    }
//}