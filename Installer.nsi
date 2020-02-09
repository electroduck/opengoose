SetCompressor /SOLID LZMA
RequestExecutionLevel admin
InstallDir $PROGRAMFILES\OpenGoose
InstallDirRegKey HKLM SOFTWARE\OpenGoose Location
Name "OpenGoose"
Icon "OpenGoose\Resources\Goose.ico"
OutFile "InGoose.exe"

!include MUI2.nsh
!include FileFunc.nsh

!define MUI_ICON "OpenGoose\Resources\Goose.ico"
!define MUI_ABORTWARNING
!define MUI_CUSTOMFUNCTION_GUIINIT customGUIInit
!define MUI_HEADERIMAGE
!define MUI_HEADERIMAGE_BITMAP "Install\Header.bmp"
!define MUI_WELCOMEFINISHPAGE_BITMAP "Install\Sidebar.bmp"

!define MUI_PAGE_CUSTOMFUNCTION_PRE funcCheckPassive
!insertmacro MUI_PAGE_WELCOME
!define MUI_PAGE_CUSTOMFUNCTION_PRE funcCheckPassive
!insertmacro MUI_PAGE_LICENSE "LICENSE.txt"
!define MUI_PAGE_CUSTOMFUNCTION_PRE funcCheckPassive
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!define MUI_PAGE_CUSTOMFUNCTION_PRE funcCheckPassive
!insertmacro MUI_PAGE_FINISH

!insertmacro MUI_UNPAGE_WELCOME
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_UNPAGE_FINISH

!insertmacro MUI_LANGUAGE "English"

Function funcCheckPassive
	${GetOptions} $CMDLINE "/P" $0
	StrLen $1 $0
	IntCmp $1 0 notpassive notpassive
	Abort
notpassive:
FunctionEnd

Function customGUIInit
	InitPluginsDir
	SetOutPath $EXEDIR
	
	# background / splash / bgm here
FunctionEnd

Section
	SetShellVarContext all
	SetOutPath $INSTDIR
	
	File OpenGoose\bin\Release\DeskMob.dll
	File OpenGoose\bin\Release\OpenGoose.exe
	File OpenGoose\Resources\Goose.ico
	
	CreateShortCut "$DESKTOP\OpenGoose.lnk" "$INSTDIR\OpenGoose.exe" "" "$INSTDIR\Goose.ico"
	CreateShortCut "$SMPROGRAMS\OpenGoose.lnk" "$INSTDIR\OpenGoose.exe" "" "$INSTDIR\Goose.ico"
	
	WriteUninstaller UnGoose.exe
	
	WriteRegStr HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\OpenGoose" "DisplayIcon" "$INSTDIR\Goose.ico"
	WriteRegStr HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\OpenGoose" "DisplayName" "OpenGoose"
	WriteRegStr HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\OpenGoose" "InstallLocation" "$INSTDIR"
	WriteRegStr HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\OpenGoose" "UninstallPath" "$INSTDIR\UnGoose.exe"
	WriteRegStr HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\OpenGoose" "UninstallString" '"$INSTDIR\UnGoose.exe"'
	WriteRegDWORD HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\OpenGoose" "NoModify" 1
	WriteRegDWORD HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\OpenGoose" "NoRemove" 0
	WriteRegDWORD HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\OpenGoose" "NoRepair" 1
	
	WriteRegStr HKLM "SOFTWARE\OpenGoose" "Location" "$INSTDIR"
SectionEnd
