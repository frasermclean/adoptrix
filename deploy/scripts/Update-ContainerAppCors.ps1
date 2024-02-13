param (
  [Parameter(Mandatory = $true)][string] $ResourceGroupName
)

function Get-StaticWebAppOrigins([string]$resourceGroup) {
  $app = Get-AzStaticWebApp -ResourceGroupName $resourceGroup

  Write-Host "Detected Static Web App: $($app.Name)"
  $builds = Get-AzStaticWebAppBuild -ResourceGroupName $resourceGroup -Name $app.Name

  # Calculate origins from default hostname, custom domains and build environment hostnames
  $origins = @($app.DefaultHostname, $app.CustomDomain, ($builds | ForEach-Object { $_.Hostname }))
  | ForEach-Object { "https://$_" }
  | Sort-Object
  | Get-Unique

  Write-Host "Calculated $($origins.Length) origins that should be allowed: $($origins -join ', ')"

  return $origins
}

function Update-ContainerAppCors([string]$resourceGroup, [string[]]$origins) {
  $app = Get-AzContainerApp -ResourceGroupName $resourceGroup
  Write-Host "Detected container app: $($app.Name)"

  foreach ($origin in $origins) {
    if ($app.Configuration.CorPolicyAllowedOrigin -notcontains $origin) {
      Write-Host "Will add origin: $origin"
      $app.Configuration.CorPolicyAllowedOrigin += $origin
      $updateRequired = $true
    }
  }

  if ($updateRequired) {
    Write-Host "Updating container app CORS policy"
    Update-AzContainerApp -ResourceGroupName $resourceGroup -Name $app.Name -Configuration $app.Configuration
  }
  else {
    Write-Host "No update required - Container App CORS settings are up to date." -ForegroundColor Green
  }
}

$swaOrigins = Get-StaticWebAppOrigins $ResourceGroupName
Update-ContainerAppCors $ResourceGroupName $swaOrigins
