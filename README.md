Currently a bit stuck on testing. will push soon. I got a bit stuck with xUnit. I know it's overtime but I will try to have it done on Monday night along with the logging.

But for now, I have a somewhat untested, semi-proofed version of the code that works fine logically.

dependencies

dotnet list package
Project 'rokkit' has the following package references

[net8.0]:

Top-level Package

Microsoft.NET.Test.Sdk         17.10.0     

Moq                            4.20.70    

xunit                          2.8.1       

xunit.runner.visualstudio      2.8.1     



How to run.

Just pull master to VSCode and let it rip with Ctrl+F5

(If you want to be old school there is the standard:

`dotnet build rokkit.csproj`

`dotnet run`
)

If you feel like configuring some testing input you can find it under Program.cs (It will not be required once xUnit is working properly)
