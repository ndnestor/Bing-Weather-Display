
updateImageInterval := %1%

Gui, New
Gui, Margin, 0, 0
;Gui, Add, Picture, x0 y0 w648 h443 +0x4000000 vweatherPicture, %A_ScriptDir%\..\tmp\Bing Weather.png
Gui, Add, Picture, x0 y0 w610 h589 +0x4000000 vweatherPicture, %A_ScriptDir%\..\tmp\Bing Weather.png
GUI, Show, NoActivate X200 Y200 w610 h589, Bing Weather Display
WinSet, Style, -0xC00000, ahk_exe Display Image.exe ; Removes the title bar of the window

SetTimer, UpdateImage, %1%

UpdateImage:
	GuiControl,, weatherPicture, %A_ScriptDir%\..\tmp\Bing Weather.png
	MsgBox, %1%
return
