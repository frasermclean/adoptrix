using 'main.bicep'

param workload = 'adoptrix'
param category = 'demo'

param b2cAuthClientId = '91556c30-ad29-4d36-a3df-29701eaba8a9'
param b2cAuthAudience = '2cb24eb4-3363-42fb-81ca-e69ff34ac201'

param sqlAdminGroupName = 'Adoptrix Demo SQL Server Administrators'
param sqlAdminGroupObjectId = 'e7c9edf8-8969-43d1-91d1-cced4230b4b6'

param appSettingsSqidsAlphabet = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789'
param appSettingsSqidsMinLength = 8
