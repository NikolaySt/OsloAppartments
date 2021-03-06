using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace StubsAI.DriverConfiguration
{
	internal class Program
	{
		private static readonly Random _random = new Random();
		private const string _targetProxyValidationUrl = "https://api.orionscloud.com/";
		private static string _driverDirectory = "";

		private static void Main(string[] args)
		{   //Loading...
			//Stubs Settings
			var SeleniumDriverBaseUserPath = "C:\\StubsAI\\Selenium\\AutomationUserData";
			var SeleniumDriverRootUserPath = "C:\\StubsAI\\Selenium";
			var SeleniumDriverDownloadPath = "C:\\StubsAI\\Downloads";
			var SeleniumDriverPath = "Drivers";

			_driverDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SeleniumDriverPath);

			var options = new ChromeOptions()
			{
				AcceptInsecureCertificates = true,
			};

			options.AddArgument(@"user-data-dir=" + $"{SeleniumDriverBaseUserPath}");

			options.AddArgument("window-size=1000,700");
			options.AddArgument("no-sandbox");
			options.AddUserProfilePreference("profile.default_content_setting_values.images", 2);

			var driver = new ChromeDriver(_driverDirectory, options);
			var baseUrl = "https://www.finn.no/realestate/lettings/search.html?area_from=37&location=0.20061&price_from=10000&price_to=15000";

			try
			{
				try
				{
					driver.Manage().Cookies.DeleteAllCookies();

					for (var i = 1; i <= 7; i++)
					{
						var url = $"{baseUrl}&page={i}";

						// NOTE Test navigation
						driver.Navigate().GoToUrl(url);

						Thread.Sleep(3000);

						var adds = driver.FindElementsByXPath("//div[@class='ads__unit__content']");
						var addresses = driver.FindElementsByXPath("//span[@class='ads__unit__content__details']");
						var prices = driver.FindElementsByXPath("//p[@class='ads__unit__content__keys']");
						var index = 0;
						foreach (var element in adds)
						{
							var address = addresses[index].Text;
							var price = prices[index].Text;
							Console.WriteLine($"{address}, {price}");
							index++;
						}
					}

					Console.WriteLine("press key to close...");

					Console.ReadLine();
				}
				finally
				{
					driver.Close();
					driver.Quit();
				}
			}
			catch { }

			Console.ReadLine();
		}
	}
}
