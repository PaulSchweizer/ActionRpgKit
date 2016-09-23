nuget install NUnit.Runners -Version 2.6.4 -OutputDirectory tools
nuget install OpenCover -Version 4.6.519 -OutputDirectory tools
nuget install coveralls.net -Version 0.412.0 -OutputDirectory tools
 
"/tools/OpenCover.4.6.519/tools/OpenCover.Console.exe" -target:.\tools\NUnit.Runners.2.6.4\tools\nunit-console.exe "-targetargs:""ActionRpgKit/bin/Release/ActionRpgKit.dll"" /noshadow" -filter:"+[ActionRpgKit*]*" -register:user

"/tools\coveralls.net.0.412\tools\csmacnz.Coveralls.exe" --opencover -i .\results.xml 
