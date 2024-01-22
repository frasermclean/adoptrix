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

$origins = Get-StaticWebAppOrigins -AppName $StaticWebAppName -ResourceGroupName $ResourceGroupName
Write-Host "Detected $($origins.Count) origins to enable"

Write-Host "Updating CORS settings for $AppServiceName"
