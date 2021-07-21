@echo off
IF %1.==. GOTO No1
IF %2.==. GOTO No1

set PACKAGE_VERSION=%1
set API_KEY=%2

dotnet build FullTextSearchQuery/FullTextSearchQuery.csproj -c Release
echo --- package built

dotnet nuget push "FullTextSearchQuery/bin/Release/SkuVault.FullTextSearchQuery."%PACKAGE_VERSION%".nupkg"  --api-key %API_KEY% --source "https://nuget.pkg.github.com/skuvault/index.json"
echo --- package published

PAUSE
GOTO End1

:No1
	echo Enter package version as the first parameter and GitHub personal access token (PAT) as the second parameter.
	pause
GOTO End1

:End1