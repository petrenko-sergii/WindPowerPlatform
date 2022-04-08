using System;
using System.Linq;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using WindPowerPlatformService.Data;
using WindPowerPlatformService.Models;

namespace WindPowerPlatformService.UnitTests
{
    [TestFixture]
    public class WindPowerPlatformRepoTests
    {
        private AppDbContext _appDbContextMock;
        private IWindPowerPlatformRepo _windPowerPlatformRepo;

        [SetUp]
        public void Setup()
        {
            _appDbContextMock = GetDatabaseContext();
            _windPowerPlatformRepo = new WindPowerPlatformRepo(_appDbContextMock);
        }

        [TearDown]
        public void DisposeTest()
        {
            _appDbContextMock = null;
            _windPowerPlatformRepo = null;
        }

        [Test]
        public void GetAllPlatforms_ShouldReturnAllPlatforms()
        {
            // Arrange
            int expectedPlatformCount = 5;
            string expectedPlatform1Name = "Platform1";
            string expectedPlatform1Manufacturer = "Manufacturer1";
            string expectedPlatform1Description = "Description1";

            // Act
            var platforms = _windPowerPlatformRepo.GetAllPlatforms();
            var platform1 = platforms.FirstOrDefault();

            // Assert
            Assert.AreEqual(expectedPlatformCount, platforms.Count());
            Assert.AreEqual(expectedPlatform1Name, platform1.Name);
            Assert.AreEqual(expectedPlatform1Manufacturer, platform1.Manufacturer);
            Assert.AreEqual(expectedPlatform1Description, platform1.Description);
        }

        [Test]
        public void GetPlatformById_WithValidPlatformId_ShouldReturnPlatform()
        {
            // Arrange
            int platformId = 4;
            string expectedPlatform4Name = "Platform4";
            string expectedPlatform4Manufacturer = "Manufacturer4";
            string expectedPlatform4Description = "Description4";

            // Act
            var platform = _windPowerPlatformRepo.GetPlatformById(platformId);

            // Assert
            Assert.IsNotNull(platform);
            Assert.AreEqual(expectedPlatform4Name, platform.Name);
            Assert.AreEqual(expectedPlatform4Manufacturer, platform.Manufacturer);
            Assert.AreEqual(expectedPlatform4Description, platform.Description);
        }

        [Test]
        public void GetPlatformById_WithNotValidPlatformId_ShouldReturnNull()
        {
            // Arrange
            int notValidPlatformId = 0;

            // Act
            var platform = _windPowerPlatformRepo.GetPlatformById(notValidPlatformId);

            // Assert
            Assert.IsNull(platform);
        }

        [Test]
        public void CreatePlatform_WithNullableModel_ShouldReturnException()
        {
            // Arrange
            WindPowerPlatform platform = null;

            // Act & Assert
            Assert.Throws(typeof(ArgumentNullException), () => _windPowerPlatformRepo.CreatePlatform(platform));
        }

        [Test]
        public void CreatePlatform_WithValidModel_ShouldCreatePlatform()
        {
            // Arrange
            var platformToCreate = new WindPowerPlatform()
            {
                Name = "NewPlatform",
                Manufacturer = "ManufacturerA",
                Description = "DescriptionA"
            };

            var existedPlatformCount = _windPowerPlatformRepo.GetAllPlatforms().Count();

            // Act
            _windPowerPlatformRepo.CreatePlatform(platformToCreate);
            _windPowerPlatformRepo.SaveChanges();            
            var platformListWithNewItem = _windPowerPlatformRepo.GetAllPlatforms();

            // Assert
            Assert.AreEqual(existedPlatformCount + 1, platformListWithNewItem.Count());
        }

        private AppDbContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var dbContext = new AppDbContext(options);

            dbContext.Database.EnsureCreated();
            PopulateTestData(dbContext);

            return dbContext;
        }

        private static void PopulateTestData(AppDbContext dbContext)
        {
            if (dbContext.WindPowerPlatforms.Count() == 0)
            {
                for (int i = 1; i <= 5; i++)
                {
                    dbContext.WindPowerPlatforms.Add(
                        new WindPowerPlatform
                        {
                            Name = $"Platform{i}",
                            Manufacturer = $"Manufacturer{i}",
                            Description = $"Description{i}"
                        });

                    dbContext.SaveChanges();
                }
            }
        }
    }
}
