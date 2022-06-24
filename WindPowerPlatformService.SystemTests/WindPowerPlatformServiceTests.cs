using NUnit.Framework;
using RestSharp;
using System;
using System.Net;
using System.Threading.Tasks;

namespace WindPowerPlatformService.SystemTests
{
    public class WindPowerPlatformServiceTests
	{
        [SetUp]
        public void Setup()
        {
        }

		[Test]
		public void GetWindpowerplatforms_DockerContainerApp_ShouldReturnPlatforms()
		{
			// Arrange
			var k8sAppUrl = "https://wpp.com/api/windpowerplatforms";
			var jsonUtf8ContentType = "application/json; charset=utf-8";
			var client = new RestClient(k8sAppUrl);
			var request = new RestRequest(Method.GET);
			client.Timeout = -1;

			var expectedPlatformNameVestas = "Vestas";
			var expectedPlatformNameSiemens = "Siemens";

			ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

			// Act
			IRestResponse response = client.Execute(request);

			// Assert
			Assert.IsNotNull(response);
			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
			Assert.IsNotNull(response.Content);
			Assert.AreEqual(jsonUtf8ContentType, response.ContentType);
			Assert.IsTrue(response.Content.Contains(expectedPlatformNameVestas) 
				&& response.Content.Contains(expectedPlatformNameSiemens));
		}

		[Test]
		public async Task CreateWindpowerplatform_DockerContainerApp_ShouldCreatePlatform()
		{
			// Arrange
			Random rnd = new Random();
			int rndNumber = rnd.Next(100);
			string dateNow = DateTime.UtcNow.ToString("yyyy-MM-dd");

			string testPlatformName = $"Test-Enercon-{dateNow}-{rndNumber}";
			string testDescription = $"German wind power company - {rndNumber}";
			string jsonBody = $"{{\"name\": \"{testPlatformName}\", \"manufacturer\": \"ENERCON GmbH\",\"description\": \"{testDescription}\"}}";

			var k8sAppUrl = "https://wpp.com/api/WindPowerPlatforms";
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
			Assert.IsTrue(response.Content.Contains(testPlatformName)
				&& response.Content.Contains(testDescription));

			CheckForNewCreatedPlatform(testPlatformName, testDescription, client);
		}

		private static void CheckForNewCreatedPlatform(string testPlatformName, string testDescription, RestClient client)
		{
			var expectedPlatformNameVestas = "Vestas";
			var request = new RestRequest(Method.GET);
			client.Timeout = -1;
			ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

			IRestResponse response = client.Execute(request);

			Assert.IsNotNull(response);
			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
			Assert.IsNotNull(response.Content);
			Assert.IsTrue(response.Content.Contains(expectedPlatformNameVestas)
				&& response.Content.Contains(testPlatformName)
				&& response.Content.Contains(testDescription));
		}
	}
}
