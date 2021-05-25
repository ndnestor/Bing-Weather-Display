using System;
using Backend;

namespace Frontend {
	class Program {
		static void Main(string[] args) {
			Console.WriteLine("Hello World!");
			WebCrawler.GetWeather("New York City", "New York");
		}
	}
}