using System;
using System.Drawing;
using Backend;

namespace Frontend {
	public class Program {
		private static void Main(string[] args) {
			Console.WriteLine("Starting Bing Weather Display");
			Bitmap weatherImage = null;
			if(WebCrawler.Initialize()) {
				weatherImage = WebCrawler.GetWeather("New York City", "New York", true);
			}
			if(weatherImage != null) {
				ConsoleDrawer.Draw(weatherImage, new Point(10, 10), new Size(60, 40));
			}
		}
	}
}