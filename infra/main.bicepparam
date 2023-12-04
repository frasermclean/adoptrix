using 'main.bicep'

param workload = 'adoptrix'
param category = 'demo'

param b2cAuthClientId = '91556c30-ad29-4d36-a3df-29701eaba8a9'
param b2cAuthAudience = '2cb24eb4-3363-42fb-81ca-e69ff34ac201'

param adminGroupName = 'Adoptrix Demo Administrators'
param adminGroupObjectId = '0356480d-b5dc-440f-a5d3-e7b3f2169c40'

param appSettingsSqidsAlphabet = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789'
param appSettingsSqidsMinLength = 8
