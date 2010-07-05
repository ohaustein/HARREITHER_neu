[System.Reflection.Assembly]::LoadWithPartialName("System.Windows.Forms")

$version = Get-Content .\curversion.txt
$versionDir = "\\eos\Share\Software\europlan\versions_\v" + $version
if (! ([IO.Directory]::Exists($versionDir))) {
	mkdir $versionDir
	$newDe = $versionDir + "\europlan20-de.msi";
	$newEn = $versionDir + "\europlan20-en.msi";
	copy \\eos\e$\Development\ccnet\europlan2.0\src\europlan2.0_grafisch\Setup\bin\Release\de-DE\europlan20.msi $newDe
	copy \\eos\e$\Development\ccnet\europlan2.0\src\europlan2.0_grafisch\Setup\bin\Release\en-US\europlan20.msi $newEn
	copy \\eos\e$\Development\ccnet\europlan2.0\src\europlan2.0_grafisch\Setup\bin\Release\latest_setup\europlan20-de.exe $versionDir
	copy \\eos\e$\Development\ccnet\europlan2.0\src\europlan2.0_grafisch\Setup\bin\Release\latest_setup\europlan20-en.exe $versionDir
} else {
	[System.Windows.Forms.MessageBox]::Show("Das Verzeichnis für Version " + $version + " existiert bereits. Die Dateien wurden nicht kopiert", "Fehler", [System.Windows.Forms.MessageBoxButtons]::Ok, [System.Windows.Forms.MessageBoxIcon]::Error)
}