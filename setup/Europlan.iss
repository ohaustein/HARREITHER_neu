#define MyAppName "Europlan"
#define MyAppVersion "3.2.1-0001"
#define MyAppPublisher "Harreither GmbH"
#define MyAppURL "https://www.harreither.com/"
#define MyAppExeName "europlan.exe"

[Setup]
AppId={{FC2D92FF-9190-4C41-8D07-5F16FED19092}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
DisableProgramGroupPage=yes
PrivilegesRequired=lowest
PrivilegesRequiredOverridesAllowed=dialog
OutputBaseFilename=Europlan {#MyAppVersion}
Compression=lzma
SolidCompression=yes
WizardStyle=modern
OutputDir=..\pub

[Languages]
Name: "en"; MessagesFile: "compiler:Default.isl"; LicenseFile: ".\Lizenz_EN.txt"
Name: "de"; MessagesFile: "compiler:Languages\German.isl"; LicenseFile: ".\Lizenz_DE.txt"
Name: "el"; MessagesFile: "compiler:Languages\Greek.isl"; LicenseFile: ".\Lizenz_EL.txt"
Name: "es"; MessagesFile: "compiler:Languages\Spanish.isl"; LicenseFile: ".\Lizenz_ES.txt"
Name: "hr"; MessagesFile: "compiler:Languages\Croatian.isl"; LicenseFile: ".\Lizenz_HR.txt"
Name: "hu"; MessagesFile: "compiler:Languages\Hungarian.isl"; LicenseFile: ".\Lizenz_HU.txt"
Name: "it"; MessagesFile: "compiler:Languages\Italian.isl"; LicenseFile: ".\Lizenz_IT.txt"
Name: "pl"; MessagesFile: "compiler:Languages\Polish.isl"; LicenseFile: ".\Lizenz_PL.txt"
Name: "sk"; MessagesFile: "compiler:Languages\Slovak.isl"; LicenseFile: ".\Lizenz_SK.txt"
Name: "sl"; MessagesFile: "compiler:Languages\Slovenian.isl"; LicenseFile: ".\Lizenz_SL.txt"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "..\src\europlan2.0\Application\bin\Release\*"; DestDir: "{app}"; Flags: ignoreversion createallsubdirs recursesubdirs

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

