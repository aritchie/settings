@echo off
del *.nupkg
nuget pack Acr.Settings.nuspec
rem nuget pack Acr.MvvmCross.Plugins.Settings.nuspec
pause