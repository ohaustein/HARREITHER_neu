Get-ChildItem -Filter *.resx -Recurse |
     ForEach-Object { (Get-Content $_.FullName) -replace
     "CTRL","Strg" | Set-Content -path $_.FullName }