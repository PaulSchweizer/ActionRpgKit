nuget install NUnit.Runners -Version 2.6.4 -OutputDirectory tools
nuget install OpenCover -Version 4.6.519 -OutputDirectory tools
nuget install coveralls.net -Version 0.412.0 -OutputDirectory tools
 
.\tools\OpenCover.4.6.519\tools\OpenCover.Console.exe -target:.\tools\NUnit.Runners.2.6.4\tools\nunit-console.exe -targetargs:"/nologo /noshadow .\ActionRpgKit\bin\Debug\ActionRpgKit.dll" -filter:"+[ActionRpgKit]*" -register:user
.\tools\coveralls.net.0.412\tools\csmacnz.Coveralls.exe --opencover -i .\results.xml --commitId $env:APPVEYOR_REPO_COMMIT --commitBranch $env:APPVEYOR_REPO_BRANCH --commitAuthor $env:APPVEYOR_REPO_COMMIT_AUTHOR --commitEmail $env:APPVEYOR_REPO_COMMIT_AUTHOR_EMAIL --commitMessage $env:APPVEYOR_REPO_COMMIT_MESSAGE 

nuget install Doxygen -Version 1.8.9.2 -OutputDirectory tools
.\generateDocumentationAndDeploy.sh
