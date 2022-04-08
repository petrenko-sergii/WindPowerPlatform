using System.Linq;
using NUnit.Framework;
using WindPowerPlatformService.Data;
using WindPowerPlatformService.Models;
using WindPowerPlatformService.Controllers;
using Moq;
using AutoMapper;
using WindPowerPlatformService.SyncDataServices.Http;
using WindPowerPlatformService.AsyncDataServices;
using Microsoft.AspNetCore.Mvc;
using WindPowerPlatformService.Dtos;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;

namespace WindPowerPlatformService.IntegrationTests
{
    [TestFixture]
    public class WindPowerPlatformsControllerAsyncTests
    {
        private AppDbContext _appDbContextMock;
        private Mock<IMapper> _mockMapper;
        private Mock<ICommandDataClient> _commandDataClient;
        private Mock<IMessageBusClient> _messageBusClient;
        private IWindPowerPlatformRepo _windPowerPlatformRepo;

        [SetUp]
        public void Setup()
        {
            _appDbContextMock = GetDatabaseContext();
            _windPowerPlatformRepo = new WindPowerPlatformRepo(_appDbContextMock);
            _mockMapper = new Mock<IMapper>();
            _commandDataClient = new Mock<ICommandDataClient>();
            _messageBusClient = new Mock<IMessageBusClient>();
        }

        [TearDown]
        public void DisposeTest()
        {
            _appDbContextMock = null;
            _windPowerPlatformRepo = null;
        }

        [Test]
        public async Task CreatePlatform_WithValidInputModel_ShouldReturnNewPlatform()
        {
            // Arrange
            var newPlatformId = 5;
            var newPlatformName = "NewPlatform";

            var platformCreateDto = new WindPowerPlatformCreateDto()
            {
                Name = "NewPlatform",
                Manufacturer = "Manufacturer",
                Description = "Description"
            };

            var controller = new WindPowerPlatformsController(
                                _windPowerPlatformRepo,
                                _mockMapper.Object,
                                _commandDataClient.Object,
                                _messageBusClient.Object);

            _mockMapper.Setup(m => m.Map<WindPowerPlatform>(It.IsAny<WindPowerPlatformCreateDto>()))
               .Returns(GetTestWindPowerPlatform());

            _mockMapper.Setup(m => m.Map<WindPowerPlatformReadDto>(It.IsAny<WindPowerPlatform>()))
               .Returns(GetNewWindPowerPlatformReadDto());

            _mockMapper.Setup(m => m.Map<WindPowerPlatformPublishedDto>(It.IsAny<WindPowerPlatformReadDto>()))
               .Returns(GetWindPowerPlatformPublishedDto());

            // Act
            var response = await controller.CreatePlatform(platformCreateDto);
            var responseResult = (response.Result as CreatedAtRouteResult);
            var resultPlatform = responseResult.Value as WindPowerPlatformReadDto;

            // Assert
            Assert.IsNotNull(resultPlatform);
            Assert.AreEqual(((int)HttpStatusCode.Created), responseResult.StatusCode);
            Assert.AreEqual(newPlatformId, resultPlatform.Id);
            Assert.AreEqual(newPlatformName, resultPlatform.Name);
        }

        private static WindPowerPlatform GetTestWindPowerPlatform()
        {
            return new WindPowerPlatform
            {
                Name = "NewPlatform",
                Manufacturer = "NewManufacturer",
                Description = "NewDescription"
            };
        }

        private static WindPowerPlatformReadDto GetNewWindPowerPlatformReadDto()
        {
            return new WindPowerPlatformReadDto
            {
                Id = 5,
                Name = "NewPlatform",
                Manufacturer = "NewManufacturer",
                Description = "NewDescription"
            };
        }

        private static WindPowerPlatformPublishedDto GetWindPowerPlatformPublishedDto()
        {
            return new WindPowerPlatformPublishedDto
            {
                Name = "NewPlatformPublishedDto",
                Manufacturer = "Manufacturer",
                Description = "Description",
                Event = "Event"
            };
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
                for (int i = 1; i <= 4; i++)
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
