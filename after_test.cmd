nuget install NUnit.Runners -Version 2.6.4 -OutputDirectory tools
nuget install OpenCover -Version 4.6.519 -OutputDirectory tools
nuget install coveralls.net -Version 0.412.0 -OutputDirectory tools

.\tools\OpenCover.4.6.519\tools\OpenCover.Console.exe -target:.\tools\NUnit.Runners.2.6.4\tools\nunit-console.exe -targetargs:"/nologo /noshadow .\src\ActionRpgKit\bin\Debug\ActionRpgKit.NUnit.Tests.dll" -filter:"+[ActionRpgKit]*" -register:user
.\tools\coveralls.net.0.412\tools\csmacnz.Coveralls.exe --opencover -i .\results.xml --commitId $env:APPVEYOR_REPO_COMMIT --commitBranch $env:APPVEYOR_REPO_BRANCH --commitAuthor $env:APPVEYOR_REPO_COMMIT_AUTHOR --commitEmail $env:APPVEYOR_REPO_COMMIT_AUTHOR_EMAIL --commitMessage $env:APPVEYOR_REPO_COMMIT_MESSAGE

nuget install Doxygen -Version 1.8.9.2 -OutputDirectory tools
nuget install GraphViz.NET -OutputDirectory tools

rem Create a clean working directory for this script.
MD doc
cd doc

rem Get the current gh-pages branch
git clone -b gh-pages https://git@github.com/PaulSchweizer/ActionRpgKit.git
cd ActionRpgKit

rem Configure git.
rem Set the push default to simple i.e. push only the current branch.
git config --global push.default simple
rem Pretend to be an user called Travis CI.
git config user.name "Appveyor"
git config user.email "travis@travis-ci.org"

rem Remove everything currently in the gh-pages branch.
rem GitHub is smart enough to know which files have changed and which files have
rem stayed the same and will only update the changed files. So the gh-pages branch
rem can be safely cleaned, and it is sure that everything pushed later is the new
rem documentation.
rem del /q .\*
rem for /d %%x in (.\*) do @rd /s /q "%%x"

rem Need to create a .nojekyll file to allow filenames starting with an underscore
rem to be seen on the gh-pages site. Therefore creating an empty .nojekyll file.
rem Presumably this is only needed when the SHORT_NAMES option in Doxygen is set
rem to NO, which it is by default. So creating the file just in case.
copy NUL .nojekyll

rem Generate the Doxygen code documentation and log the output.
echo 'Generating Doxygen code documentation...'
rem Redirect both stderr and stdout to the log file AND the console.
dir C:\projects\actionrpgkit\ActionRpgKit
dir

C:\projects\actionrpgkit\ActionRpgKit\tools\Doxygen.1.8.9.2\tools\doxygen.exe C:\projects\actionrpgkit\ActionRpgKit\config_appveyor.dox

xcopy C:\projects\actionrpgkit\doc\ActionRpgKit\html C:\projects\actionrpgkit\doc\ActionRpgKit /s /y

echo 'Uploading documentation to the gh-pages branch...'

git status

git add --all

git commit -m "Deploy code docs to GitHub Pages Appveyor build:" -m "Commit: "
git push --force "https://%GH_REPO_TOKEN%@github.com/PaulSchweizer/ActionRpgKit.git"
rem > /dev/null 2>&1
