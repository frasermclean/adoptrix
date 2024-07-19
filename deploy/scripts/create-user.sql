CREATE USER [adoptrix-demo-api-ca] FROM EXTERNAL PROVIDER;
ALTER ROLE db_datareader ADD MEMBER [adoptrix-demo-api-ca];
ALTER ROLE db_datawriter ADD MEMBER [adoptrix-demo-api-ca];
ALTER ROLE db_ddladmin ADD MEMBER [adoptrix-demo-api-ca];
GO
