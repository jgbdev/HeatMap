#
# InstallHeatMap.ps1
#
#Requires -RunAsAdministrator

Add-Type -AssemblyName System.IO.Compression.FileSystem

if (Test-Path .\HeatMapService.zip) {
	Remove-Item .\HeatMapService.zip -Force
}
if (Test-Path .\HeatMapService) {
	Remove-Item .\HeatMapService -Force -Recurse
}

wget https://github.com/jgbdev/HeatMap/raw/master/Releases/HeatMap_ServiceLatest.zip -OutFile .\HeatMapService.zip

function Unzip
{
    param([string]$zipfile, [string]$outpath)

    [System.IO.Compression.ZipFile]::ExtractToDirectory($zipfile, $outpath)
}

Unzip ((Get-Item -Path ".\" -Verbose).FullName + "\HeatMapService.zip") ((Get-Item -Path ".\" -Verbose).FullName + "\HeatMapService")

If(Test-Path "C:\Windows\Microsoft.NET\Framework64\v4.0.30319") {
	& "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\InstallUtil.exe" -u ".\HeatMapService\HeatMap_Service.exe"
	& "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\InstallUtil.exe" ".\HeatMapService\HeatMap_Service.exe"
}
ElseIf(Test-Path "C:\Windows\Microsoft.NET\Framework\v4.0.30319") {
	& "C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe" -u ".\HeatMapService\HeatMap_Service.exe"
	& "C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe" ".\HeatMapService\HeatMap_Service.exe"
}
Else {
	"InstallUtil not found! Please use the manual install instructions."
}