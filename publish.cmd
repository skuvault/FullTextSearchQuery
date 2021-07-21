@echo off
IF %1.==. GOTO No1
set API_KEY=%2

cd FullTextSearchQuery
dotnet clean FullTextSearchQuery.csproj -c Release
dotnet build FullTextSearchQuery.csproj -c Release
echo --- package built

cd bin/Release
dotnet nuget push *.nupkg  --api-key %API_KEY% --source "https://nuget.pkg.github.com/skuvault/index.json"
echo --- package published

PAUSE
GOTO End1

:No1
	echo Enter GitHub personal access token (PAT) as a parameter.
	pause
GOTO End1

:End1