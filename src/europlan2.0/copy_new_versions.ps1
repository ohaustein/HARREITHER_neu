[System.Reflection.Assembly]::LoadWithPartialName("System.Windows.Forms")

$jenkinsUrl = "http://jenkins:8080/"
$jenkinsJob = "europlan_grafisch_trunk"
$versionRootDir = "\\fsh\Daten\Share\Software\europlan\versions_ga\"

$version = Get-Content .\curversion.txt
$versionDir = $versionRootDir + "\v" + $version

if (! ([IO.Directory]::Exists($versionDir))) {
	$jenkinsUrlMsiDe = $jenkinsUrl + "view/Alle/job/" + $jenkinsJob + "/lastSuccessfulBuild/artifact/src/europlan2.0/Setup/bin/Release/de-DE/europlan30.msi"
	$jenkinsUrlMsiEn = $jenkinsUrl + "view/Alle/job/" + $jenkinsJob + "/lastSuccessfulBuild/artifact/src/europlan2.0/Setup/bin/Release/en-US/europlan30.msi"
	$jenkinsUrlExeDe = $jenkinsUrl + "view/Alle/job/" + $jenkinsJob + "/lastSuccessfulBuild/artifact/src/europlan2.0/Setup/bin/Release/latest_setup/europlan30-de.exe"
	$jenkinsUrlExeEn = $jenkinsUrl + "view/Alle/job/" + $jenkinsJob + "/lastSuccessfulBuild/artifact/src/europlan2.0/Setup/bin/Release/latest_setup/europlan30-en.exe"

	mkdir $versionDir
	$msiDe = $versionDir + "\europlan30-de.msi";
	$msiEn = $versionDir + "\europlan30-en.msi";
	$exeDe = $versionDir + "\europlan30-de.exe";
	$exeEn = $versionDir + "\europlan30-en.exe";

	# download files
	$wc = New-Object System.Net.WebClient
	$wc.DownloadFile($jenkinsUrlMsiDe, $msiDe)
	$wc.DownloadFile($jenkinsUrlMsiEn, $msiEn)
	$wc.DownloadFile($jenkinsUrlExeDe, $exeDe)
	$wc.DownloadFile($jenkinsUrlExeEn, $exeEn)
} else {
	[System.Windows.Forms.MessageBox]::Show("Das Verzeichnis für Version " + $version + " existiert bereits. Die Dateien wurden nicht kopiert", "Fehler", [System.Windows.Forms.MessageBoxButtons]::Ok, [System.Windows.Forms.MessageBoxIcon]::Error)
}