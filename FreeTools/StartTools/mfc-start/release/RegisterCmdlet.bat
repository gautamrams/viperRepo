@echo off
FOR /F "tokens=*" %%R IN ('powershell.exe Get-ExecutionPolicy') DO SET VAR=%%R

IF not %var% == RemoteSigned powershell.exe Set-ExecutionPolicy RemoteSigned

powershell.exe -command .\RegisterCmdlet.ps1 GetDomains.dll GetDuplicates.dll LocalUserManagement.dll SPMTool.dll SendGreetingcheck.dll
