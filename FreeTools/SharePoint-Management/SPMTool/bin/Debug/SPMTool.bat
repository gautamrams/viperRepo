@echo off
FOR /F "tokens=*" %%R IN ('powershell.exe Get-ExecutionPolicy') DO SET VAR=%%R
echo "%var%"

IF not %var% == RemoteSigned powershell.exe Set-ExecutionPolicy RemoteSigned

powershell.exe -command .\SPMTool.ps1 SPMTool.dll
exit