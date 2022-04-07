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
        private Mock<IWindPowerPlatformRepo> mockRepository;
        private Mock<IMapper> mockMapper;
        private Mock<ICommandDataClient> commandDataClient;
        private Mock<IMessageBusClient> messageBusClient;

        [SetUp]
        public void Setup()
        {
            mockRepository = new Mock<IWindPowerPlatformRepo>();
            mockMapper = new Mock<IMapper>();
            commandDataClient = new Mock<ICommandDataClient>();
            messageBusClient = new Mock<IMessageBusClient>();
        }

        [Test]
        public void GetPlatformById_WithValidPlatformId_GetCorrectPlatform()
        {
            // Arrange
            mockRepository.Setup(x => x.GetPlatformById(It.IsAny<int>()))
                .Returns(
                    new WindPowerPlatform
                    {
                        Id = 1,
                        Name = "PlatformA",
                        Manufacturer = "ManufacturerA",
                        Description = "DescriptionA"
                    }
                );

            mockMapper.Setup(m => m.Map<WindPowerPlatformReadDto>(It.IsAny<WindPowerPlatform>()))
                .Returns(GetTestWindPowerPlatformReadDto());

            var controller = new WindPowerPlatformsController(
                                mockRepository.Object,
                                mockMapper.Object,
                                commandDataClient.Object,
                                messageBusClient.Object);

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
                                mockRepository.Object,
                                mockMapper.Object,
                                commandDataClient.Object,
                                messageBusClient.Object);

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
                                mockRepository.Object,
                                mockMapper.Object,
                                commandDataClient.Object,
                                messageBusClient.Object);

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
            IEnumerable<WindPowerPlatformReadDto> data = GetTestWindPowerPlatformReadDtos();

            mockMapper.Setup(m => m.Map<IEnumerable<WindPowerPlatformReadDto>>(It.IsAny<IEnumerable<WindPowerPlatform>>()))
                .Returns(data);

            var controller = new WindPowerPlatformsController(
                                mockRepository.Object,
                                mockMapper.Object,
                                commandDataClient.Object,
                                messageBusClient.Object);

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
