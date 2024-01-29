param (
  [Parameter(Mandatory = $true)][string] $ServerName,
  [Parameter(Mandatory = $true)][string] $DatabaseName,
  [Parameter(Mandatory = $true)][string] $AppName
)

# Aquire an access token for the Azure SQL Server
$accessToken = (Get-AzAccessToken -ResourceUrl https://database.windows.net).Token

$query = @"
IF NOT EXISTS(
  SELECT [name]
  FROM sys.database_principals
  WHERE [name] = '$AppName'
)
BEGIN
  CREATE USER [$AppName] FROM EXTERNAL PROVIDER
  ALTER ROLE db_datareader ADD MEMBER [$AppName]
  ALTER ROLE db_datawriter ADD MEMBER [$AppName]
  ALTER ROLE db_ddladmin ADD MEMBER [$AppName]
END
"@

Write-Host "Setting database permissions for $AppName on $ServerName/$DatabaseName"

# Execute the query
Invoke-Sqlcmd -ServerInstance "$ServerName.database.windows.net" `
  -Database $DatabaseName `
  -AccessToken $accessToken `
  -Query $query

Write-Host "Completed database permissions update." -ForegroundColor Green
