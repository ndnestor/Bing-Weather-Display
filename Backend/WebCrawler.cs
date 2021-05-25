using System;
using System.Drawing;
using System.IO;
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
		private const string LoadedCssIndicator = "div[class='b_antiTopBleed b_antiSideBleed b_antiBottomBleed']";

		// Call this before using any other methods in this class
		public static void Initialize() {
			_webDriver = new FirefoxDriver();
		}
		
		// Saves an image showing the weather forecast
		public static Bitmap GetWeather(string city, string state) {
			try {
				// Navigate to the weather site
				_webDriver.Navigate().GoToUrl(GetWeatherUrl(city, state));
				IWebElement loadedElement = WaitForPage();
				if(loadedElement == null) {
					return null;
				}

				// Take screenshot of page
				byte[] screenshotByteArray = ((ITakesScreenshot)_webDriver).GetScreenshot().AsByteArray;
				Bitmap screenshot = new(new MemoryStream(screenshotByteArray));

				// Crop the screenshot
				Rectangle cropArea =
					new(loadedElement.Location.X, loadedElement.Location.Y, loadedElement.Size.Width, loadedElement.Size
						.Height);
				screenshot = screenshot.Clone(cropArea, screenshot.PixelFormat);
				screenshot.Save(@"C:\tmp\test.png", System.Drawing.Imaging.ImageFormat.Png);

				// Close the browser
				_webDriver.Close();
				_webDriver.Quit();

				return screenshot;
			} catch(Exception e) {
				Console.WriteLine("Get weather failed with exception: " + e);
				return null;
			}
		}

		// Turns the client's search query into a Bing search query
		private static string GetWeatherUrl(string city, string state) {
			return "https://www.bing.com/search?form=MOZLBR&pc=MOZI&q=weather+in+" + city + "+" + state;
		}
		
		// Stops thread until weather page is loaded (returns false if could not load in time)
		private static IWebElement WaitForPage() {
			try {
				WebDriverWait driverWait = new(_webDriver, TimeSpan.FromSeconds(TimeoutTime));
				driverWait.Until(driver => driver.FindElement(By.CssSelector(LoadedCssIndicator)));
				return _webDriver.FindElement(By.CssSelector(LoadedCssIndicator));
			} catch(NoSuchElementException e) {
				Console.WriteLine(e);
				return null;
			}
		}
	}
}