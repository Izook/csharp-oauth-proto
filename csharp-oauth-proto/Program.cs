using System;
using System.Diagnostics;
using System.Net.Http;
using Flurl.Http;
using RestSharp;

namespace csharp_oauth_proto
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length < 5)
			{
				System.Console.WriteLine("Insufficient arguments provided. Five Arguments expected.");
				return;
			}

			// Retrieve command line arguments
			string clientId = args[0];
			string clientSecret = args[1];
			string tenantId = args[2];
			string redirectURL = args[3];
			string resourceURL = args[4];

			// Set OAuth 2.0 URL Endpoints
			string accessURL = "https://login.microsoftonline.com/" + tenantId + "/oauth2/authorize?client_id=" + clientId + "&response_type=code&redirect_uri=" + redirectURL + "&response_mode=query&resource=" + resourceURL;
			string authzURL = "https://login.microsoftonline.com/" + tenantId + "/oauth2/token";

			// Access Code
			string accessCode = "";
			string accessToken = "";

			// Open ie at Authorization URL
			ProcessStartInfo info = new ProcessStartInfo(@"C:/Program Files/Internet Explorer/iexplore.exe");
			info.Arguments = accessURL;
			Process.Start(info);

			// Await Access Code
			System.Console.WriteLine("Please enter the access code once you have logged in: ");
			accessCode = Console.ReadLine();
			System.Console.WriteLine("Thank you!");

            // Make POST request to get Access Token
            var client = new RestClient(authzURL);
            var request = new RestRequest(Method.POST);
            request.AddHeader("Postman-Token", "5144ba81-d1c1-4c97-aee5-351b8ba20cea");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("undefined", "grant_type=authorization_code&client_id="+ clientId + "&code=" + accessCode + "&redirect_uri=" + redirectURL + "&resource=" + resourceURL + "&client_secret=" +  clientSecret, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            // Print out access token
            System.Console.WriteLine(response.Content);

			System.Console.WriteLine("Thank you for authorizing and authenticating!!!");
			System.Console.ReadLine();

			return;
		}
	}
}
