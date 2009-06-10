[System.Reflection.Assembly]::LoadWithPartialName("System.Windows.Forms")

$replaceVersionFiles = new-object string[] 4
$findVersionStrings = new-object string[][] 4
$replaceVersionStrings = new-object string[][] 4

$assemblyVersionFind = '\[assembly: AssemblyVersion\("${VERSION}"\)\]'
$assemblyVersionReplace = '[assembly: AssemblyVersion("${VERSION}")]'
$fileVersionFind = '\[assembly: AssemblyFileVersion\("${VERSION}"\)\]'
$fileVersionReplace = '[assembly: AssemblyFileVersion("${VERSION}")]'

# .\Application\Properties\AssemblyInfo.cs
$replaceVersionFiles[0] = ".\Application\Properties\AssemblyInfo.cs"
$findVersionStrings[0] = new-object string[] 2
$replaceVersionStrings[0] = new-object string[] 2
$findVersionStrings[0][0] = $assemblyVersionFind
$replaceVersionStrings[0][0] = $assemblyVersionReplace
$findVersionStrings[0][1] = $fileVersionFind
$replaceVersionStrings[0][1] = $fileVersionReplace

# .\AdminApplication\Properties\AssemblyInfo.cs
$replaceVersionFiles[1] = ".\AdminApplication\Properties\AssemblyInfo.cs"
$findVersionStrings[1] = new-object string[] 2
$replaceVersionStrings[1] = new-object string[] 2
$findVersionStrings[1][0] = $assemblyVersionFind
$replaceVersionStrings[1][0] = $assemblyVersionReplace
$findVersionStrings[1][1] = $fileVersionFind
$replaceVersionStrings[1][1] = $fileVersionReplace

# .\Common\Properties\AssemblyInfo.cs
$replaceVersionFiles[2] = ".\Common\Properties\AssemblyInfo.cs"
$findVersionStrings[2] = new-object string[] 2
$replaceVersionStrings[2] = new-object string[] 2
$findVersionStrings[2][0] = $assemblyVersionFind
$replaceVersionStrings[2][0] = $assemblyVersionReplace
$findVersionStrings[2][1] = $fileVersionFind
$replaceVersionStrings[2][1] = $fileVersionReplace

# .\Setup\europlan20.wxs
$replaceVersionFiles[3] = ".\Setup\europlan20.wxs"
$findVersionStrings[3] = new-object string[] 2
$replaceVersionStrings[3] = new-object string[] 2
$findVersionStrings[3][0] = '<\?define ProductVersion="${VERSION}" \?>'
$replaceVersionStrings[3][0] = '<?define ProductVersion="${VERSION}" ?>'
$findVersionStrings[3][1] = '<\?define ProductId="${GUID}" \?>'
$replaceVersionStrings[3][1] = '<?define ProductId="${GUID}" ?>'

#$findVersionStrings[3][0] = '<Product Id="${GUID}" Name="Europlan 2\.0" Language="1031" Version="${VERSION}" Manufacturer="bluesource mobile solutions gmbh" UpgradeCode="\{ca795401-2df9-4ed9-8770-f230bd4ab9dc\}">'
#$replaceVersionStrings[3][0] = '<Product Id="${GUID}" Name="Europlan 2.0" Language="1031" Version="${VERSION}" Manufacturer="bluesource mobile solutions gmbh" UpgradeCode="{ca795401-2df9-4ed9-8770-f230bd4ab9dc}">'
#$findVersionStrings[3][1] = '<UpgradeVersion OnlyDetect="no" MigrateFeatures="yes" Minimum="0\.0\.0\.0" IncludeMinimum="yes" Maximum="${VERSION}" IncludeMaximum="no" Property="OLDVERSIONFOUND" />'
#$replaceVersionStrings[3][1] = '<UpgradeVersion OnlyDetect="no" MigrateFeatures="yes" Minimum="0.0.0.0" IncludeMinimum="yes" Maximum="${VERSION}" IncludeMaximum="no" Property="OLDVERSIONFOUND" />'
#$findVersionStrings[3][2] = '<UpgradeVersion OnlyDetect="yes" Minimum="${VERSION}" IncludeMinimum="no" Property="NEWVERSIONFOUND" />'
#$replaceVersionStrings[3][2] = '<UpgradeVersion OnlyDetect="yes" Minimum="${VERSION}" IncludeMinimum="no" Property="NEWVERSIONFOUND" />'

#$updateFiles = [string]"curVersion.txt"
#$replaceVersionFiles | ForEach-Object -process {
#  $updateFiles += " "
#  $updateFiles += $_.Substring(2)
#}
#
#echo $replaceVersionFiles

#$svnOutput = svn update curVersion.txt $replaceVersionFiles
#echo $svnOutput
svn update curVersion.txt $replaceVersionFiles

if ([System.Windows.Forms.MessageBox]::Show("Bitte im Konsolenfenster überprüfen, ob das SVN Update funktioniert hat. Falls ein Problem beim Update aufgetreten ist 'Nein' drücken", "Update OK?", [System.Windows.Forms.MessageBoxButtons]::YesNo) -eq [System.Windows.Forms.DialogResult]::Yes) {

  $oldVersion = Get-Content .\curversion.txt
  $sv = $oldVersion.Split('.')
  $inc = [int]$sv[2]
  $inc++
  $sv[2] = [string]$inc
  $sv[3] = "0"
  $newVersion = $sv[0] + '.' + $sv[1] + '.' + $sv[2] + '.' + $sv[3]
  $newVersion > .\curversion.txt

  echo "Neue Version: $newVersion"
  
  $guid = [guid]::NewGuid()
  $newGuid = '{' + $guid.ToString() + '}'

  $i = [int]0
  $replaceVersionFiles | ForEach-Object -process {
    echo "Aktualisiere version in $_"
    $content = Get-Content $_
    $j = [int]0
    $findVersionStrings[$i] | ForEach-Object -process {
      $findString = $_ -replace '\$\{VERSION\}', '\d+\.\d+\.\d+\.\d+'
	  $findString = $findString -replace '\$\{GUID\}', '\{?\w{8}-\w{4}-\w{4}-\w{4}-\w{12}\}?'
	  $replaceString = $replaceVersionStrings[$i][$j]
	  $replaceString = $replaceString -replace '\$\{VERSION\}', $newVersion
	  $replaceString = $replaceString -replace '\$\{GUID\}', $newGuid
	  $content = $content -replace $findString, $replaceString
	  $j++
    }
    $content > $_
    $i++
  }
  
  if ([System.Windows.Forms.MessageBox]::Show("Sollen die modifizierten Dateien eingecheckt werden?", "Einchecken?", [System.Windows.Forms.MessageBoxButtons]::YesNo) -eq [System.Windows.Forms.DialogResult]::Yes) {
    svn commit -m $newVersion curVersion.txt $replaceVersionFiles
  }
} else {
  echo "Abgebrochen"
}