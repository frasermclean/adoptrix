param (
  $StaticWebAppName = "adoptrix-demo-frontend-swa",
  [Parameter(Mandatory = $false)]
  [string]
  $AppServiceName = "adoptrix-demo-backend-app"
)

# Get the allowed origins from the Static Web App
$swaCustomDomains = az staticwebapp show --name $StaticWebAppName --query "customDomains" | ConvertFrom-Json
$swaHostnames = az staticwebapp environment list --name $StaticWebAppName --query "[].hostname" | ConvertFrom-Json
$swaHostnames += $swaCustomDomains
$swaOrigins = $swaHostnames | ForEach-Object { "https://$_" }

# Update App Service CORS settings with the allowed origins

Write-Host "Updating CORS settings for $AppServiceName"
