nuget install NUnit.Runners -Version 2.6.4 -OutputDirectory tools
nuget install OpenCover -Version 4.6.519 -OutputDirectory tools
nuget install coveralls.net -Version 0.412.0 -OutputDirectory tools

.\tools\OpenCover.4.6.519\tools\OpenCover.Console.exe -target:.\tools\NUnit.Runners.2.6.4\tools\nunit-console.exe -targetargs:"/nologo /noshadow .\src\ActionRpgKit\bin\Debug\ActionRpgKit.NUnit.Tests.dll" -filter:"+[ActionRpgKit]*" -register:user
.\tools\coveralls.net.0.412\tools\csmacnz.Coveralls.exe --opencover -i .\results.xml --commitId $env:APPVEYOR_REPO_COMMIT --commitBranch $env:APPVEYOR_REPO_BRANCH --commitAuthor $env:APPVEYOR_REPO_COMMIT_AUTHOR --commitEmail $env:APPVEYOR_REPO_COMMIT_AUTHOR_EMAIL --commitMessage $env:APPVEYOR_REPO_COMMIT_MESSAGE

nuget install Doxygen -Version 1.8.9.2 -OutputDirectory tools
nuget install GraphViz.NET -OutputDirectory tools


MD code_docs
cd code_docs

git clone -b gh-pages https://git@github.com/PaulSchweizer/ActionRpgKit.git
cd ActionRpgKit

git config --global push.default simple
git config user.name "Appveyor"
git config user.email "travis@travis-ci.org"

echo 'Generating Doxygen code documentation...'
C:\projects\actionrpgkit\tools\doxygen\bin\doxygen.exe config_appveyor.dox

xcopy /s /q /y .\html\. .\

echo 'Uploading documentation to the gh-pages branch...'

git add --all

git commit -m "Deploy code docs to GitHub Pages Appveyor build:" -m "Commit: "
git push --force "https://${GH_REPO_TOKEN}@github.com/PaulSchweizer/ActionRpgKit.git" > /dev/null 2>&1
