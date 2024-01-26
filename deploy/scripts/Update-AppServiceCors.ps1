param (
  [Parameter(Mandatory = $true)][string] $ResourceGroupName
)

function Get-StaticWebAppOrigins([string]$ResourceGroupName) {
  # Get the Static Web App name
  $appName = az staticwebapp list --resource-group adoptrix-demo-rg --query "[0].name" --output tsv
  Write-Host "Detected Static Web App name: $appName"

  # Read custom domains and environment hostnames
  [string[]]$customDomains = az staticwebapp show --name $appName --query "customDomains" | ConvertFrom-Json
  [string[]]$environmentHostnames = az staticwebapp environment list --name $appName --query "[].hostname" | ConvertFrom-Json

  $origins = $($customDomains + $environmentHostnames)
  | ForEach-Object { "https://$_" }
  | Sort-Object
  | Get-Unique

  Write-Host "Currently active origins in SWA: $($origins.Length)"

  return $origins
}

function Update-AppServiceCors([string]$ResourceGroupName, [string[]]$SwaOrigins) {
  # Get the App Service name
  $appName = az webapp list --resource-group $ResourceGroupName --query "[0].name" --output tsv
  Write-Host "Detected App Service name: $appName"

  # Get the currently active origins on the App Service
  $activeOrigins = (az webapp cors show --name $appName --resource-group $ResourceGroupName | ConvertFrom-Json).allowedOrigins
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

$swaOrigins = Get-StaticWebAppOrigins -ResourceGroupName $ResourceGroupName
$operationCount = Update-AppServiceCors -ResourceGroupName $ResourceGroupName -SwaOrigins $swaOrigins

Write-Host "Completed CORS update with $operationCount operations."
