using System;
using System.Diagnostics;
using System.Net.Http;
using Flurl.Http;

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
			string clientId = args[1];
			string clientSecret = args[2];
			string tenantId = args[3];
			string redirectURL = args[4];
			string resourceURL = args[5];

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

			// Construct POST Request to Authorization URL
			HttpResponseMessage response = authzURL.PostUrlEncodedAsync(new
			{
				grant_type = "authorizaton_code",
				client_id = clientId,
				code = accessCode,
				redirect_uri = redirectURL,
				resource = resourceURL,
				client_secret = clientSecret,
			}).Result;

			// Make POST Request using cURL
			accessToken = response.Content.ToString();

			// Print out access token
			System.Console.WriteLine(accessToken);

			System.Console.WriteLine("Thank you for authorizing and authenticating!!!");
			System.Console.ReadLine();

			return;
		}
	}
}
