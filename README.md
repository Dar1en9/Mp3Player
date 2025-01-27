There're 3 working branches in this repository. 

**v1-console-app-json**: This branch stores all information in JSON files automatically. All you need to do is run the admin panel (using program arguments as "admin"), then add your tracks. After this you can switch to default panel
by running the app without any arguments.

**v2-console-app-dapper**: To use this branch, you need to connect to your database and create there a table:
```sql
   CREATE TABLE Tracks (
       Id UUID PRIMARY KEY,
       Professor VARCHAR(255) NOT NULL,
       TrackName VARCHAR(255) NOT NULL,
       AudioPath VARCHAR(255) NOT NULL
   );
```
Then you need to connect to this database in Visual Studio/Rider. After this you can use the Admin and User panels the same way as in v1-consol-app-json.

**v3-web-app-API**: To use this branch, you also need a database (database creation is similar to v2-console-app-dapper). In this version, there are no panels; just send your requests
(you can open Swagger UI in the browser).
