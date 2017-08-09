[void]
[System.Reflection.Assembly]::LoadWithPartialName("System.Windows.Forms")
$count =0;
get-pssnapin -registered | ForEach-Object {

if($_.Name -contains "ManageLocalUsers")
{
$count += 1

}


}
if($count -ne 1)
{

function installutil
{
if($args[0] -ne $null)
{
$dllpath = $args[0]
$processorInfo = "$env:windir\syswow64\WindowsPowerShell\v1.0"

if(test-path $processorInfo)
{
	set-alias installutil $env:windir\Microsoft.NET\Framework64\v2.0.50727\installutil
	Write-Host "64-bit Machine"
}
else
{
	set-alias installutil $env:windir\Microsoft.NET\Framework\v2.0.50727\installutil
}

if(test-path $dllPath)
{
installutil /u $dllpath
installutil $dllpath
write-host "snap-in installed. now, you can add it to your shell instance"

}
else{
[System.Windows.Forms.MessageBox]::Show("The file does not exist")
break;
}
}
else
{

[System.Windows.Forms.MessageBox]::Show("The classLibrary file not given as input")
break;
}
}
installutil $args[0]
}
Add-Pssnapin "ManageLocalUsers" -ErrorAction SilentlyContinue
cls
Manage-LocalUsers
