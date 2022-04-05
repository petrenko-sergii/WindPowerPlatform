using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NSubstitute;
using Microsoft.EntityFrameworkCore;
using WindPowerPlatformService;
using WindPowerPlatformService.Data;
using WindPowerPlatformService.Models;

namespace WindPowerPlatformService.UnitTests
{
    [TestFixture]
    public class WindPowerPlatformRepoTests
    {
        //private IWindPowerPlatformRepo _windPowerPlatformRepo;
        //TODO
       
        private static readonly WindPowerPlatform[] WindPowerPlatformTestData =
        {            
            new WindPowerPlatform {
                Name = "Name1",
                Manufacturer = "M1",
                Description = "D1"
            },
            new WindPowerPlatform {
                Name = "Name2",
                Manufacturer = "M2",
                Description = "D2"
            }
        };

        [SetUp]
        public void Setup()
        {          
            
        }

        [Test]
        public void WindPowerPlatformRepoTest1()
        {
            //TODO
            Assert.AreEqual(2,2);
        }
    }
}