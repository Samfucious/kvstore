dotnet restore
msbuild kvstore.sln /t:Rebuild /p:Configuration=Release /p:Platform="any cpu"
vstest.console.exe UnitTestProject1\bin\Release\netcoreapp2.1\UnitTestProject1.dll



