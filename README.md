# SQLSERVER LNM README

This is the SQl Server LNM exe to use it you must have a config file with the same name in the very same directory with relevant details of the server to connect to Database.



    1. the first field e.g <add key="Dbname" value="sms_in"/>
        * the key is the DB name you are trying to connect to 
        * the value is the name of the db table  to connect e.g. sms_in 
    2. the second e.g <add key="server" value="localhost"/> the value here is the server name.
    3. the third e.g. <add key="UID" value="root"/> the value here is the username login to the server.
    4. the forth e.g. <add key="PWD" value=""/> the value should be the password for login to the server.

this is a sample of the config file.

``` XML
<configuration>
  <appSettings>
    <add key="DB" value="sms_in"/>
    <add key="server" value="localhost"/>
    <add key="UID" value="root"/>
    <add key="PWD" value=""/>
  </appSettings>
</configuration> 
```

the name of the config **MUST EXACTLY** be like the name of the exe i.e sqlsrvr.exe.config 

You can use the config provided and edit it.
