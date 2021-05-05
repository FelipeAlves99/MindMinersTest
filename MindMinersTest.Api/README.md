# MindMinersTest


## How to run the api

 - First you gonna need the [SDK](https://dotnet.microsoft.com/download/dotnet/5.0) to run dotnet 5
   - If you want to just run the api, you can install the ASP.NET core runtime found in the same SDK's link
 - Go to the MindMinersTest.Api folder.
 - You will need to restore the nuget packages and build the solution executing the following commands
   - `dotnet restore`
   - `dotnet build`
- To run the API, just execute the following command
   - `dotnet run`

## Endpoints

This API have swagger for documentation and you can access it through here [Swagger UI](https://localhost:5001/swagger/index.html) after starting the api locally
/File has two methods, GET and POST. GET will return a list of file names found in wwwroot/uploads; POST will receive a file and an offset and return a new file with the offset added to all timecodes of that .srt file.
/File/Download is a direct endpoint for downloading any file.