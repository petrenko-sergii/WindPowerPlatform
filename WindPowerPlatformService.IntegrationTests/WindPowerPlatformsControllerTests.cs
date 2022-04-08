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
using System.Collections.Generic;

namespace WindPowerPlatformService.IntegrationTests
{
    [TestFixture]
    public class WindPowerPlatformsControllerTests
    {
        private Mock<IWindPowerPlatformRepo> _mockRepository;
        private Mock<IMapper> _mockMapper;
        private Mock<ICommandDataClient> _commandDataClient;
        private Mock<IMessageBusClient> _messageBusClient;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<IWindPowerPlatformRepo>();
            _mockMapper = new Mock<IMapper>();
            _commandDataClient = new Mock<ICommandDataClient>();
            _messageBusClient = new Mock<IMessageBusClient>();
        }

        [Test]
        public void GetPlatformById_WithValidPlatformId_GetCorrectPlatform()
        {
            // Arrange
            _mockRepository.Setup(x => x.GetPlatformById(It.IsAny<int>()))
                .Returns(
                    new WindPowerPlatform
                    {
                        Id = 1,
                        Name = "PlatformA",
                        Manufacturer = "ManufacturerA",
                        Description = "DescriptionA"
                    }
                );

            _mockMapper.Setup(m => m.Map<WindPowerPlatformReadDto>(It.IsAny<WindPowerPlatform>()))
                .Returns(GetTestWindPowerPlatformReadDto());

            var controller = new WindPowerPlatformsController(
                                _mockRepository.Object,
                                _mockMapper.Object,
                                _commandDataClient.Object,
                                _messageBusClient.Object);

            string expectedPlatformName = "PlatformA";
            string expectedPlatformDescription = "DescriptionA";

            // Act
            var response = controller.GetPlatformById(1);
            var resultPlatform = (response.Result as OkObjectResult).Value as WindPowerPlatformReadDto;

            // Assert
            Assert.IsNotNull(resultPlatform);
            Assert.AreEqual(expectedPlatformName, resultPlatform.Name);
            Assert.AreEqual(expectedPlatformDescription, resultPlatform.Description);
        }

        [Test]
        public void GetPlatformById_WithInvalidPlatformId_ShouldReturnNull()
        {
            // Arrange
            var invalidPlatformId = 5;

            var controller = new WindPowerPlatformsController(
                                _mockRepository.Object,
                                _mockMapper.Object,
                                _commandDataClient.Object,
                                _messageBusClient.Object);

            // Act
            var response = controller.GetPlatformById(invalidPlatformId);
            var resultPlatform = response.Result as NotFoundObjectResult;

            // Assert
            Assert.IsNull(resultPlatform);
        }

        [Test]
        public void GetPlatformById_WithInvalidPlatformId_ShouldReturnNotFoundResult()
        {
            // Arrange
            var invalidPlatformId = 5;

            var controller = new WindPowerPlatformsController(
                                _mockRepository.Object,
                                _mockMapper.Object,
                                _commandDataClient.Object,
                                _messageBusClient.Object);

            // Act
            var response = controller.GetPlatformById(invalidPlatformId);
            var resultPlatform = response.Result.ToString();

            // Assert
            Assert.IsNotNull(resultPlatform);
            Assert.IsTrue(resultPlatform.Contains(HttpStatusCode.NotFound.ToString()));
        }

        [Test]
        public void GetWindPowerPlatforms_ShouldReturnPlatforms()
        {
            // Arrange
            IEnumerable<WindPowerPlatformReadDto> data = GetTestWindPowerPlatformReadDtos();

            _mockMapper.Setup(m => m.Map<IEnumerable<WindPowerPlatformReadDto>>(It.IsAny<IEnumerable<WindPowerPlatform>>()))
                .Returns(data);

            var controller = new WindPowerPlatformsController(
                                _mockRepository.Object,
                                _mockMapper.Object,
                                _commandDataClient.Object,
                                _messageBusClient.Object);

            // Act
            var response = controller.GetWindPowerPlatforms();
            var resultPlatforms = (response.Result as OkObjectResult).Value as IEnumerable<WindPowerPlatformReadDto>;

            // Assert
            Assert.IsNotNull(resultPlatforms);
            Assert.AreEqual(2, resultPlatforms.Count());
        }

        private static WindPowerPlatformReadDto GetTestWindPowerPlatformReadDto()
        {
            return new WindPowerPlatformReadDto
            {
                Id = 1,
                Name = "PlatformA",
                Manufacturer = "ManufacturerA",
                Description = "DescriptionA"
            };
        }

        private static IEnumerable<WindPowerPlatformReadDto> GetTestWindPowerPlatformReadDtos()
        {
            return new List<WindPowerPlatformReadDto>
            {
                GetTestWindPowerPlatformReadDto(),
                new WindPowerPlatformReadDto {
                    Id = 2,
                    Name = "PlatformB",
                    Manufacturer = "ManufacturerB",
                    Description = "DescriptionB"
                }
            }.AsEnumerable();
        }
    }
}
