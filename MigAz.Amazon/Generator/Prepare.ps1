# v1.0.0.3
# Parameters
param (
    [Parameter(Mandatory = $true)] 
    $ResourcegroupName,

    [Parameter(Mandatory = $true)] 
    $TemplateFilePath,

    [ValidateSet("StopVirtualMachines", "DeleteVirtualMachines", "UpdateTemplateDisks")]
    [Parameter(Mandatory = $true)] 
    $Action,

    $Sufix = ""
)


Function GetDiskUrl($vmname, $lun)
{
    foreach($disk in $disks)
    {
        if ($disk.vmname -eq $vmname -and $disk.lun -eq $lun)
        {
            return $disk.url
        }
    }
}

# If action is to update template disks
If ($Action -eq "UpdateTemplateDisks")
{
    $disks = @()

    $virtualmachines = Get-AzureRmVM -ResourceGroupName $ResourcegroupName
    foreach ($virtualmachine in $virtualmachines)
    {
        $vmname = $virtualmachine.Name
        $vmname = $vmname.Substring(0,$vmname.Length-$Sufix.Length)

        $disk = New-Object System.Object
        $disk | Add-Member -type NoteProperty -name vmname -value $vmname
        $disk | Add-Member -type NoteProperty -name lun -value "OS"
        $disk | Add-Member -type NoteProperty -name url -value $virtualmachine.StorageProfile.OsDisk.Vhd.Uri
        $disks += $disk

        foreach ($datadisk in $virtualmachine.StorageProfile.DataDisks)
        {
            $disk = New-Object System.Object
            $disk | Add-Member -type NoteProperty -name vmname -value $vmname
            $disk | Add-Member -type NoteProperty -name lun -value $datadisk.Lun
            $disk | Add-Member -type NoteProperty -name url -value $datadisk.Vhd.Uri
            $disks += $disk
        }
    }

    $template = Get-Content $TemplateFilePath | ConvertFrom-Json
    for($i=0; $i -lt $template.resources.Count; $i++)
    {
        $resource = $template.resources[$i]
        if ($resource.type -eq 'Microsoft.Compute/virtualMachines')
        {
            $template.resources[$i].properties.storageProfile.osDisk.vhd.uri = GetDiskUrl -vmname $resource.name -lun 'OS'

            for($lunindex=0; $lunindex -lt $resource.properties.storageProfile.dataDisks.Count; $lunindex++)
            {
                $template.resources[$i].properties.storageProfile.dataDisks[$lunindex].vhd.uri = GetDiskUrl -vmname $resource.name -lun $lunindex.ToString()
            }
        }
    }

    $templatetext = $template | ConvertTo-Json -Depth 100
    $templatetext = $templatetext.Replace("\u0027", "'")
    $templatetext | Out-File $TemplateFilePath
}

# If action is to stop virtual machines
If ($Action -eq "StopVirtualMachines")
{
    $cmds = {
        param($resourcegroupname, $vmname)
        Select-AzureRmProfile -Path C:\temp\azureprofile.json

        Stop-AzureRmVM -ResourceGroupName $resourcegroupname -Name $vmname -Force -Verbose
    }
}

# If action is to delete virtual machines and network interfaces
If ($Action -eq "DeleteVirtualMachines")
{
    $cmds = {
        param($resourcegroupname, $vmname)
        Select-AzureRmProfile -Path C:\temp\azureprofile.json

        $vm = Get-AzureRmVM -ResourceGroupName $resourcegroupname -Name ($vmname)
        $nics = $vm.NetworkProfile.NetworkInterfaces
        Remove-AzureRmVM -ResourceGroupName $resourcegroupname -Name ($vmname) -Force -Verbose

        foreach ($nic in $nics)
        {
            $nicarray = $nic.Id.Split("/")
            $nicname = $nicarray[$nicarray.Count-1]
            Remove-AzureRmNetworkInterface -ResourceGroupName $resourcegroupname -Name $nicname -Force -Verbose
        }
    }
}

# If action is to Stop or Delete virtual machines
If ($Action -eq "DeleteVirtualMachines" -or $Action -eq "StopVirtualMachines")
{
    Save-AzureRmProfile -Path C:\temp\azureprofile.json -Force
    $jobs = @()

    $resources = Get-Content $TemplateFilePath | ConvertFrom-Json
    $virtualmachines = $resources.resources | ? type -EQ 'Microsoft.Compute/virtualMachines'
    foreach ($virtualmachine in $virtualmachines)
    {
        $jobs += Start-Job -ScriptBlock $cmds -ArgumentList $ResourcegroupName, ($virtualmachine.name+$Sufix)
    }
    do
    {
        Start-Sleep -Seconds 1
    }
    while (($jobs | ? State -EQ 'Running').Count -gt 0)
}
