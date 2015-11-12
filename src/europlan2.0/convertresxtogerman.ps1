Get-ChildItem -Filter *.resx -Recurse |
     ForEach-Object { (Get-Content $_.FullName) -replace
     "CTRL","STRG" | Set-Content -path $_.FullName }