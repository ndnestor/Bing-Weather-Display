
Gui, 1: New
Gui, 1: Margin, 0, 0
Gui, 1: Add, Picture, x0 y0 w610 h589 +0x4000000 vWeatherPicture, %A_ScriptDir%\..\tmp\Bing Weather.png
GUI, 1: Show, NoActivate X200 Y200 w610 h589, Bing Weather Display
WinSet, Style, -0xC00000, ahk_exe Display Image.exe ; Removes the title bar of the window

SetTimer, UpdateImage, %1%

UpdateImage:
	GuiControl, 1:, WeatherPicture, %A_ScriptDir%\..\tmp\Bing Weather.png
return
