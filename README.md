# MichaelsPlace

How to Build and Run:
-----------


###MVC application and API:

1. Open command line in MichaelsPlace.sln directory and run `nuget.exe restore`.
2. Open solution in VS 2015.
3. Now we need to get the database in synce. Open the package managed console.
    1. Run `Update-Database -Force`.
    2. To get the test database in sync, run `Update-Database -ProjectName MichaelsPlace -StartUpProjectName MichaelsPlace.Tests -Force`
4. Now you can run the MichaelsPlace project.

###SPA application:

1. Install node and npm (https://nodejs.org/en/)
2. Open command line in the MichaelsPlace directory:
3. Run `npm install`. This will take a while.
4. Run `jspm install`. This will take a little less time.
5. Run `gulp watch`.
6. Open http://localhost:8081/app/index.html in your browser.

The SPA application is embedded in the MVC application, in directories which start with lowercase letters.

    /app - The output directory of the gulp build (equivalent to /bin). It serves as the root path of the SPA.
    /build - Build configurations
    /src - The source files for the SPA. This is where you do development.
    /test - The tests for the SPA

The SPA also owns the .json and .js files in the root of the MichaelsPlace project.



