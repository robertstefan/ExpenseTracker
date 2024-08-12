sudo docker run --name sql_container -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=NewComplexPass123!" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest

sudo docker start sql_container