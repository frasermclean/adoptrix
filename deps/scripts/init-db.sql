-- Create the database if it doesn't already exist
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = N'adoptrix')
BEGIN
  CREATE DATABASE adoptrix;
END;
GO
