$version = Get-Content .\curversion.txt
$versionDir = "\\eos\Share\Software\europlan\versions\v" + $version
mkdir $versionDir
$newDe = $versionDir + "\europlan20-de.msi";
$newEn = $versionDir + "\europlan20-en.msi";
copy \\eos\e$\Development\ccnet\europlan2.0\trunk\src\europlan2.0\Setup\bin\Release\de-DE\europlan20.msi $newDe
copy \\eos\e$\Development\ccnet\europlan2.0\trunk\src\europlan2.0\Setup\bin\Release\en-US\europlan20.msi $newEn
copy \\eos\e$\Development\ccnet\europlan2.0\trunk\src\europlan2.0\Setup\bin\Release\latest_setup\europlan20-de.exe $versionDir
copy \\eos\e$\Development\ccnet\europlan2.0\trunk\src\europlan2.0\Setup\bin\Release\latest_setup\europlan20-en.exe $versionDir