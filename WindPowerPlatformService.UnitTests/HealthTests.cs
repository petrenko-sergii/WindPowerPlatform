using System;
using System.Collections.Generic;
using NUnit.Framework;
using NSubstitute;
using WindPowerPlatformService;

namespace WindPowerPlatformService.UnitTests
{
    public class HealthTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void HealthTest1()
        {
            Assert.AreEqual(1,1);
        }      
    }
}
