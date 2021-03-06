# To run the web app :

1. Restore bower packages.
2. Restore .NET Core dependencies.
3. Create an empty database (ex. *OnlineStore*) 
4. Change the connection string in **src/OnlineStore/appsettings.json** to match your newly created databaase.
5. Run `Update-Database` in the Package Manager Console.
6. Run your project either through Visual Studio or execute `dotnet run` in the **src/OnlineStore** directory.

# To run the tests :

> **Note:**
> You might want to install [TestDriven.NET](http://testdriven.net/). I'm not sure if Visual Studio straight out of the box would be able to run Fixie tests.

1. Restore .NET Core dependencies.
2. Create a copy of the database you created for the web project or initialize a new one following steps 2,3,4,5 in the instructions above.
3. Change the connection string in the **tests/OnlineStore.IntegrationTests/appsettings.json** to match your database.
4. Run the tests using the Visual Studio Test Explorer or by executing `dotnet test` in the project directory.
