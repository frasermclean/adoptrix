#!/bin/bash

# Execute the configure-db.sh script
/usr/config/configure-db.sh &

# And start SQL Server
/opt/mssql/bin/sqlservr
