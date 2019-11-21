using System;
using System.Diagnostics;
using RestSharp;
using Newtonsoft.Json;

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
			string accessCode;
			string accessToken;

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
			request.AddParameter("grant_type", "authorization_code", ParameterType.GetOrPost);
			request.AddParameter("client_id", clientId, ParameterType.GetOrPost);
			request.AddParameter("code", accessCode, ParameterType.GetOrPost);
			request.AddParameter("redirect_uri", redirectURL, ParameterType.GetOrPost);
			request.AddParameter("resource", resourceURL, ParameterType.GetOrPost);
			request.AddParameter("client_secret", clientSecret, ParameterType.GetOrPost);

			IRestResponse result = client.Execute(request);

			// Deserialize and Parse JSON Response
			Response response = JsonConvert.DeserializeObject<Response>(result.Content);

			// Print Access and Refresh Token
			System.Console.WriteLine("Access Token: " + response.access_token);
			System.Console.WriteLine("Refresh Token: " + response.refresh_token);

			// End Program
			System.Console.WriteLine("Thank you for authorizing and authenticating!!!");
			System.Console.ReadLine();

			return;
		}
	}

	class Response
	{
		public string token_type;
		public string scope;
		public string expires_in;
		public string ext_expires_in;
		public string expires_on;
		public string not_before;
		public string resource;
		public string access_token;
		public string refresh_token;
	}
}
