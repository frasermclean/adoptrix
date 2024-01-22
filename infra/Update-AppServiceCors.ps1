param (
  $StaticWebAppName = "adoptrix-demo-frontend-swa",
  [Parameter(Mandatory = $false)]
  [string]
  $AppServiceName = "adoptrix-demo-backend-app"
)

# Get environment names
$swaEnvNames = az staticwebapp environment list --name $StaticWebAppName --query "[].name" | ConvertFrom-Json

Write-Host "Updating CORS settings for $AppServiceName"
