; Wait 10 seconds for the Upload window to appear

  Local $hWnd = WinWait("[CLASS:#32770]","",10)

; Set input focus to the edit control of Upload window using the handle returned by WinWait

  ControlFocus($hWnd,"","Edit1")

  Sleep(2000)

; Set the File name text on the Edit field

  ControlSetText($hWnd, "", "Edit1", "C:\Users\longvu\Pictures\BDD_FW.png")

  Sleep(2000)

; Click on the Open button

  ControlClick($hWnd, "","Button1");