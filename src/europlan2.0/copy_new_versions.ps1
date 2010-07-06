[System.Reflection.Assembly]::LoadWithPartialName("System.Windows.Forms")

$version = Get-Content .\curversion.txt
$versionDir = "\\eos\Share\Software\europlan\versions_ga\v" + $version
if (! ([IO.Directory]::Exists($versionDir))) {
	mkdir $versionDir
	$newDe = $versionDir + "\europlan20ga-de.msi";
	$newEn = $versionDir + "\europlan20ga-en.msi";
	copy \\eos\e$\Development\ccnet\europlan2.0_grafisch\src\europlan2.0\Setup\bin\Release\de-DE\europlan20ga.msi $newDe
	copy \\eos\e$\Development\ccnet\europlan2.0_grafisch\src\europlan2.0\Setup\bin\Release\en-US\europlan20ga.msi $newEn
	copy \\eos\e$\Development\ccnet\europlan2.0_grafisch\src\europlan2.0\Setup\bin\Release\latest_setup\europlan20ga-de.exe $versionDir
	copy \\eos\e$\Development\ccnet\europlan2.0_grafisch\src\europlan2.0\Setup\bin\Release\latest_setup\europlan20ga-en.exe $versionDir
} else {
	[System.Windows.Forms.MessageBox]::Show("Das Verzeichnis für Version " + $version + " existiert bereits. Die Dateien wurden nicht kopiert", "Fehler", [System.Windows.Forms.MessageBoxButtons]::Ok, [System.Windows.Forms.MessageBoxIcon]::Error)
}