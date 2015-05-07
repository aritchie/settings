@echo off
del *.nupkg
nuget pack Acr.Settings.nuspec
nuget pack Acr.MvvmCross.Plugins.Settings.nuspec
pause