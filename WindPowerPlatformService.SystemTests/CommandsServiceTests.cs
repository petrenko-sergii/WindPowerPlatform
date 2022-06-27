using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WindPowerPlatformService.SystemTests.Stubs;

namespace WindPowerPlatformService.SystemTests
{
    public class CommandsServiceTests
	{
        [SetUp]
        public void Setup()
        {
        }

		[Test]
		public void GetWindpowerplatforms_CommandsSvcDockerContainerApp_ShouldReturnPlatforms()
		{
			// Arrange
			var k8sAppUrl = "https://wpp.com/api/c/windpowerplatforms";
			var jsonUtf8ContentType = "application/json; charset=utf-8";
			var client = new RestClient(k8sAppUrl);
			var request = new RestRequest(Method.GET);
			client.Timeout = -1;

			var expectedPlatformNameNordex = "Nordex";
			var expectedPlatformNameEnercon = "Enercon";

			ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

			// Act
			IRestResponse response = client.Execute(request);

			// Assert
			Assert.IsNotNull(response);
			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
			Assert.IsNotNull(response.Content);
			Assert.AreEqual(jsonUtf8ContentType, response.ContentType);
			Assert.IsTrue(response.Content.Contains(expectedPlatformNameNordex) 
				&& response.Content.Contains(expectedPlatformNameEnercon));
		}

		[Test]
		public async Task CreateCommandForPlatform_DockerContainerApp_ShouldCreateCommand()
		{
			// Arrange
			Random rnd = new Random();
			int rndNumber = rnd.Next(100);
			string dateNow = DateTime.UtcNow.ToString("yyyy-MM-dd");

			string testCommandName = $"Start-{dateNow}-{rndNumber}";
			string testCommandDescr = $"Start to produce electricity: test - {rndNumber}";
			var testPlatformId = 1;
			string jsonBody = $"{{\"name\": \"{testCommandName}\",\"description\": \"{testCommandDescr}\"}}";
			
			var k8sAppUrl = $"https://wpp.com/api/c/windpowerplatforms/{testPlatformId}/commands";
			var client = new RestClient(k8sAppUrl);
			var request = new RestRequest(Method.POST);
			request.AddHeader("Content-Type", "application/json");
			request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);
			client.Timeout = -1;
			ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

			// Act
			IRestResponse response = await Task.FromResult(client.Execute(request));

			// Assert
			Assert.IsNotNull(response);
			Assert.AreEqual(ResponseStatus.Completed, response.ResponseStatus);
			Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
			Assert.IsNotNull(response.Content);
			Assert.IsTrue(response.Content.Contains(testCommandName)
				&& response.Content.Contains(testCommandDescr));

			CheckForNewCreatedCommandForPlatform(testCommandName, testCommandDescr, testPlatformId, client);
		}

		private static void CheckForNewCreatedCommandForPlatform(string testPlatformName, string testDescription, int platformId, RestClient client)
		{
			// Arrange to check
			var expectedCommand = new CommandReadDtoStub { 
									Name = testPlatformName, 
									Description = testDescription, 
									PlatformId = platformId 
								};

			var request = new RestRequest(Method.GET);
			client.Timeout = -1;
			ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

			// Act to check
			IRestResponse response = client.Execute(request);

			// Assert to check
			Assert.IsNotNull(response);
			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
			Assert.IsNotNull(response.Content);

			IEnumerable<CommandReadDtoStub> commandsForPlatform = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<CommandReadDtoStub>>(response.Content);

			Assert.IsNotNull(commandsForPlatform);
			Assert.AreNotEqual(0, commandsForPlatform.Count());
			Assert.IsTrue(commandsForPlatform.Any(x => x.Name == expectedCommand.Name)
				&& commandsForPlatform.Any(x => x.Description == expectedCommand.Description)
				&& commandsForPlatform.Any(x => x.PlatformId == expectedCommand.PlatformId));
		}
	}
}
