[void]
[System.Reflection.Assembly]::LoadWithPartialName("System.Windows.Forms")
$count =0;
get-pssnapin -registered | ForEach-Object {

if($_.Name -contains "GetDomains")
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

if(test-path $dllpath)
{
installutil /u $dllpath
installutil $dllpath
write-host "snap-in installed. now, you can add it to your shell instance"

}
else{
[System.Windows.Forms.MessageBox]::Show("The installutill.exe file does not exist.")
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
installutil $args[1]
installutil $args[2]
installutil $args[3]
installutil $args[4]

}



