[System.Reflection.Assembly]::LoadWithPartialName("System.Windows.Forms")

$version = Get-Content .\curversion.txt
$versionDir = "\\eos\Share\Software\europlan\versions_ga\v" + $version
if (! ([IO.Directory]::Exists($versionDir))) {
	mkdir $versionDir
	$newDe = $versionDir + "\europlan30-de.msi";
	$newEn = $versionDir + "\europlan30-en.msi";
	copy \\eos\e$\Development\ccnet\europlan2.0_grafisch\src\europlan2.0\Setup\bin\Release\de-DE\europlan30.msi $newDe
	copy \\eos\e$\Development\ccnet\europlan2.0_grafisch\src\europlan2.0\Setup\bin\Release\en-US\europlan30.msi $newEn
	copy \\eos\e$\Development\ccnet\europlan2.0_grafisch\src\europlan2.0\Setup\bin\Release\latest_setup\europlan30-de.exe $versionDir
	copy \\eos\e$\Development\ccnet\europlan2.0_grafisch\src\europlan2.0\Setup\bin\Release\latest_setup\europlan30-en.exe $versionDir
} else {
	[System.Windows.Forms.MessageBox]::Show("Das Verzeichnis für Version " + $version + " existiert bereits. Die Dateien wurden nicht kopiert", "Fehler", [System.Windows.Forms.MessageBoxButtons]::Ok, [System.Windows.Forms.MessageBoxIcon]::Error)
}