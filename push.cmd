@echo off
nuget push .\Acr.Settings\bin\Release\*.nupkg -Source https://www.nuget.org/api/v2/package
nuget push .\Acr.Settings\bin\Release\*.nupkg -Source https://www.myget.org/F/acr/api/v2/package
del .\Acr.Settings\bin\Release\*.nupkg
pause