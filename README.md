# Bing-Weather-Display

## The Goal

The goal was to make a application that displays the weather given a city and state. I more focused on the backend of the software that takes a screenshot of the Bing weather panel and saves the resulting image. This could be hooked up to any frontend (admittedly one preferably nicer than the one I have) since the backend is partitioned from the rest of the program to allow for that.

## Technologies

The backend of it is a C# class library. The frontend is a C# console application that uses AutoHotKey for GUI. .NET Core must be installed to use the backend library. The program is targeted towards the Windows platform using net5.0.

## Launch

To build this project, you first need to clone this repo. Then compile the .NET solution (using something like Visual Studio). Next download AutoHotkey from here. After finishing the setup process, compile the AHK scripts given into the directory [bin directory]\AHK where [bin directory] is the directory of the compiled .NET solution executable file. AHK scripts can be compiled by right clicking them in File Explorer and selecting the compile option from the context menu. Finally create a folder called tmp and a file called settings.json in [bin directory].

You can run the program from command line using Frontend.exe "City Name" "State Name" updateInterval or create the settings.json file structure in place of command line arguments. The settings.json file structure is as follows:

```
{
	"city name": "Miami",
	"state name": "Florida",
	"update weather interval": 10000
}
```
You can replace the sample settings as you see fit although I would refrain from using an update interval value less than 5000 for program stability.

## Project Status

Likely complete. Although I may integrated some weather service with Winux: a private project being developed as a rework of my public Active Overlay and Project Winux projects.
