using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
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
		// Firefox input argument(s). Note that the "--headless" argument alone will cause an exception
		private const string Arguments = "";

		// Call this before using any other methods in this class (returns false if it fails)
		public static bool Initialize() {
			try {
				Console.WriteLine("Starting web crawler...");

				// Create driver service
				FirefoxDriverService driverService = FirefoxDriverService.CreateDefaultService();
				driverService.HideCommandPromptWindow = true;

				// Set options
				FirefoxOptions options = new();
				options.AddArguments(Arguments);
				_webDriver = new FirefoxDriver(driverService, options);
				_webDriver.Manage().Window.Minimize();

				// Hide window
				HideWindow("Mozilla Firefox");

				Console.WriteLine("Web crawler initialized");
				return true;
			} catch(Exception e) {
				Console.WriteLine("Initialization failed with FATAL exception: " + e + "\n\nMake sure gecko driver is added to PATH");
				CloseBrowser();
				return false;
			}
		}

		// Runs an AHK script that hides the window. A substitute for running headless
		private static void HideWindow(string windowTitle) {
			Process hideWindowAhk = new();
			hideWindowAhk.StartInfo.FileName = Path.Combine(GetCurrentPath(), @"AHK\Hide Window.exe");
			hideWindowAhk.StartInfo.Arguments = "\"" + windowTitle + "\"";
			hideWindowAhk.Start();
		}

		// Returns the path to the folder containing this process
		public static string GetCurrentPath() {
			return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
		}
		
		// Returns an image showing the weather forecast (returns null if it fails)
		public static Bitmap GetWeather(string city, string state, bool saveResult=true) {
			try {
				// Navigate to the weather site
				_webDriver.Navigate().GoToUrl(GetWeatherUrl(city, state));
				IWebElement loadedElement = WaitForPage();
				if(loadedElement == null) {
					CloseBrowser();
					return null;
				}

				// Hide the browser
				HideWindow(_webDriver.Title + " — Mozilla Firefox");

				// Take screenshot of page
				byte[] screenshotByteArray = ((ITakesScreenshot)_webDriver).GetScreenshot().AsByteArray;
				Bitmap screenshot = new(new MemoryStream(screenshotByteArray));

				// Crop the screenshot
				Rectangle cropArea =
					new(loadedElement.Location.X, loadedElement.Location.Y, loadedElement.Size.Width, loadedElement.Size
						.Height - 50);
				screenshot = screenshot.Clone(cropArea, screenshot.PixelFormat);

				if(saveResult) {
					screenshot.Save(Path.Combine(GetCurrentPath(), @"tmp\Bing Weather.png"), System.Drawing.Imaging.ImageFormat.Png);

				}

				Console.WriteLine("Succesfully took a screenshot of the Bing weather panel");

				return screenshot;
			} catch(Exception e) {
				Console.WriteLine("Get weather failed with exception: " + e);

				// Close the browser
				CloseBrowser();

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
			} catch(Exception e) {
				Console.WriteLine(e);
				return null;
			}
		}

		// Closes the Selenium browser
		public static void CloseBrowser() {
			_webDriver.Close();
			_webDriver.Quit();
		}
	}
}