param (
  [Parameter(Mandatory = $true)][string] $ResourceGroupName
)

function ConvertTo-FlattenedArray {
  $input | ForEach-Object {
    if ($_ -is [array]) {
      $_ | ConvertTo-FlattenedArray
    }
    else {
      $_
    }
  }
}

function Get-StaticWebAppOrigins([string]$resourceGroup) {
  $app = Get-AzStaticWebApp -ResourceGroupName $resourceGroup

  Write-Host "Detected Static Web App: $($app.Name)"
  $environmentHostnames = Get-AzStaticWebAppBuild -ResourceGroupName $resourceGroup -Name $app.Name | ForEach-Object { $_.Hostname }

  # Calculate origins from default hostname, custom domains and build environment hostnames
  $origins = @($app.DefaultHostname, $app.CustomDomain, $environmentHostnames)
  | ConvertTo-FlattenedArray
  | ForEach-Object { "https://$_" }
  | Sort-Object
  | Get-Unique

  Write-Host "Calculated $($origins.Length) origins that should be allowed"

  return $origins
}

function Update-ContainerAppCors([string]$resourceGroup, [string[]]$origins) {
  $app = Get-AzContainerApp -ResourceGroupName $resourceGroup
  Write-Host "Detected container app: $($app.Name)"

  # Add origins that are not yet allowed
  foreach ($origin in $origins) {
    if ($app.Configuration.CorPolicyAllowedOrigin -notcontains $origin) {
      Write-Host "Will add origin: $origin"
      $updateRequired = $true
    }
  }

  # Remove origins that are no longer required
  foreach ($origin in $app.Configuration.CorPolicyAllowedOrigin) {
    if ($origins -notcontains $origin) {
      Write-Host "Will remove origin: $origin" -ForegroundColor Yellow
      $updateRequired = $true
    }
  }

  # Update container app if required
  if ($updateRequired) {
    $app.Configuration.CorPolicyAllowedOrigin = $origins
    Write-Host "Updating container app CORS policy with $($origins.Length) origins."
    Update-AzContainerApp -ResourceGroupName $resourceGroup -Name $app.Name -Configuration $app.Configuration
  }
  else {
    Write-Host "No update required - Container App CORS settings are up to date." -ForegroundColor Green
  }
}

$swaOrigins = Get-StaticWebAppOrigins $ResourceGroupName
Update-ContainerAppCors $ResourceGroupName $swaOrigins
