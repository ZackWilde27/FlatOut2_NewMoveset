# Set Working Directory
Split-Path $MyInvocation.MyCommand.Path | Push-Location
[Environment]::CurrentDirectory = $PWD

Remove-Item "$env:RELOADEDIIMODS/FlatOut2_NewMoveset/*" -Force -Recurse
dotnet publish "./FlatOut2_NewMoveset.csproj" -c Release -o "$env:RELOADEDIIMODS/FlatOut2_NewMoveset" /p:OutputPath="./bin/Release" /p:ReloadedILLink="true"

# Restore Working Directory
Pop-Location