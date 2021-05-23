using System;
using System.Drawing;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace Backend {
	public static class WebCrawler {

		// Driver for using Firefox through Selenium
		private static IWebDriver _webDriver = new FirefoxDriver();
		// How long selenium should try to connect to a website before throwing a timeout exception
		private const int TimeoutTime = 10; // In seconds
		// The HTML class used to determine whether a page is loaded or not
		private const string LoadedClassIndicator = "wtr_heroXpBkg heroHeight";
		// The pixel area where weather data is displayed on the weather site
		private static readonly Rectangle CropArea = new(100, 100, 100, 100);

		// Call this before using any other methods in this class
		public static void Initialize() {
			_webDriver = new FirefoxDriver();
		}
		
		// Saves an image showing the weather forecast
		public static bool GetWeather(string city, string state) {
			
			// Navigate to the weather site
			_webDriver.Navigate().GoToUrl(GetWeatherUrl(city, state));
			bool pageLoaded = WaitForPage();
			if(!pageLoaded) {
				return false;
			}
			
			// Take screenshot of page
			Screenshot screenshot = ((ITakesScreenshot) _webDriver).GetScreenshot();
			
			// Crop the screenshot
			
			return true;
		}

		// Turns the client's search query into a Bing search query
		private static string GetWeatherUrl(string city, string state) {
			return "https://www.bing.com/search?form=MOZLBR&pc=MOZI&q=weather+in+" + city + "+" + state;
		}
		
		// Stops thread until weather page is loaded (returns false if could not load in time)
		private static bool WaitForPage() {
			try {
				WebDriverWait driverWait = new(_webDriver, TimeSpan.FromSeconds(TimeoutTime));
				driverWait.Until(driver => driver.FindElement(By.ClassName(LoadedClassIndicator)));
				return true;
			} catch(NoSuchElementException e) {
				Console.WriteLine("ERR: Could not load website");
				return false;
			}
		}
	}
}