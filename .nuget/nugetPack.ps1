$root = (split-path -parent $MyInvocation.MyCommand.Definition) + '\..'
$nugetPath = "$root\.nuget"
$version = [System.Reflection.Assembly]::LoadFile("$root\Simple\bin\Release\Simple.dll").GetName().Version
$versionStr = "{0}.{1}.{2}" -f ($version.Major, $version.Minor, $version.Build)

Write-Host "Setting .nuspec version tag to $versionStr"

$content = (Get-Content $nugetPath\Simple.nuspec)
$content = $content -replace '\$version\$',$versionStr

$content | Out-File $nugetPath\Simple.generated.nuspec

& $nugetPath\nuget.exe pack $nugetPath\Simple.generated.nuspec