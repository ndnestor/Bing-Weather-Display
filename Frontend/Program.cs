using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Timers;
using Backend;
using Newtonsoft.Json.Linq;

namespace Frontend {
	public class Program {
			
		private static readonly Timer UpdateWeatherTimer = new();

		private static string cityName;
		private static string stateName;
		private static double updateWeatherInterval;

		private static Process displayResultAhk;

		private static ConsoleEventDelegate consoleEventDelegate;
		private const int ConsoleCloseEvent = 2;

		// Used to set a "On Console Exit" callback
		private delegate bool ConsoleEventDelegate(int eventType);
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);

		private static void Main(string[] args) {
			Console.WriteLine("Starting Bing Weather Display");

			// Initialize variables
			if(args.Length == 3) {
				cityName = args[0];
				stateName = args[1];
				updateWeatherInterval = double.Parse(args[2]); // In milliseconds
			} else {
				Console.WriteLine("Three arguments were not given. Falling back to settings.json");
				JObject settings = JObject.Parse(File.ReadAllText(Path.Combine(WebCrawler.GetCurrentPath(), "settings.json")));
				cityName = settings["city name"].Value<string>();
				stateName = settings["state name"].Value<string>();
				updateWeatherInterval = settings["update weather interval"].Value<int>();
			}

			// Set up timer
			if(cityName.Length > 1 && stateName.Length > 1 && updateWeatherInterval > 0) {
				/* NOTE: These if statements are intentionally separated
				 * Initializing the web crawler takes a relatively long time
				 * So it is prefered to only do it when it is certain that all other
				 * running conditions are met */

				if(WebCrawler.Initialize()) {
					UpdateImage(null, null);

					UpdateWeatherTimer.Interval = updateWeatherInterval;
					UpdateWeatherTimer.AutoReset = true;
					UpdateWeatherTimer.Elapsed += UpdateImage;
					UpdateWeatherTimer.Start();

					Console.WriteLine("Press enter to exit");
					Console.ReadLine();
					OnConsoleEvent(ConsoleCloseEvent);
                }
			} else {
				Console.WriteLine("Invalid arguments given. Make sure to surround multi-word arguments with \" such as \"New York City\"");
            }
		}

		// Runs an AHK script that shows the weather image
		private static void DisplayResult() {
			if(displayResultAhk == null) {
				displayResultAhk = new();
				displayResultAhk.StartInfo.FileName = Path.Combine(WebCrawler.GetCurrentPath(), @"AHK\Display Image.exe");
				displayResultAhk.StartInfo.Arguments = updateWeatherInterval.ToString();
				displayResultAhk.Start();
            }
		}

		// Calles GetWeather() and DisplayResult()
		private static void UpdateImage(object o, ElapsedEventArgs e) {
			if(consoleEventDelegate == null) {
				consoleEventDelegate = new(OnConsoleEvent);
				SetConsoleCtrlHandler(consoleEventDelegate, true);
            }
			WebCrawler.GetWeather(cityName, stateName);
			DisplayResult();
        }

		// Do some process cleanup before exiting
		private static bool OnConsoleEvent(int eventType) {
			if(eventType == ConsoleCloseEvent) {
				Console.WriteLine("Cleaning up processes...");
				WebCrawler.CloseBrowser();
				if(displayResultAhk != null) {
					displayResultAhk.Kill();
				}
				Console.WriteLine("Finished");
            }
			return false;
        }
	}
}