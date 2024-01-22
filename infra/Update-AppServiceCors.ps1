param (
  [Parameter(Mandatory = $true)][string] $StaticWebAppName,
  [Parameter(Mandatory = $true)][string] $AppServiceName,
  [Parameter(Mandatory = $true)][string] $ResourceGroupName
)

function Get-StaticWebAppOrigins([string]$AppName, [string]$ResourceGroupName) {
  # Read custom domains and environment hostnames
  [string[]]$customDomains = az staticwebapp show --name $StaticWebAppName --query "customDomains" | ConvertFrom-Json
  [string[]]$environmentHostnames = az staticwebapp environment list --name $AppName --query "[].hostname" | ConvertFrom-Json

  # Merge and return
  return $($customDomains + $environmentHostnames)
  | ForEach-Object { "https://$_" }
  | Sort-Object
  | Get-Unique
}

function Update-AppServiceCors([string]$AppName, [string[]]$SwaOrigins, [string]$ResourceGroupName) {
  # Get the currently active origins on the App Service
  $activeOrigins = (az webapp cors show --name $AppName --resource-group $ResourceGroupName | ConvertFrom-Json).allowedOrigins
  $originsToAdd = $SwaOrigins | Where-Object { $activeOrigins -notcontains $_ }
  $originsToRemove = $activeOrigins | Where-Object { $SwaOrigins -notcontains $_ }
  $operationCount = 0

  $originsToAdd | ForEach-Object {
    Write-Host "Adding CORS origin: $_"
    az webapp cors add --name $AppName --resource-group $ResourceGroupName --allowed-origins $_ --output none
    $operationCount++
  }

  $originsToRemove | ForEach-Object {
    Write-Host "Removing CORS origin: $_"
    az webapp cors remove --name $AppName --resource-group $ResourceGroupName --allowed-origins $_ --output none
    $operationCount++
  }

  return $operationCount
}

$swaOrigins = Get-StaticWebAppOrigins -AppName $StaticWebAppName -ResourceGroupName $ResourceGroupName
$operationCount = Update-AppServiceCors -AppName $AppServiceName -SwaOrigins $swaOrigins -ResourceGroupName $ResourceGroupName

Write-Host "Completed CORS update with $operationCount operations."
