nuget install NUnit.Runners -Version 2.6.4 -OutputDirectory tools
nuget install OpenCover -Version 4.6.519 -OutputDirectory tools
nuget install coveralls.net -Version 0.412.0 -OutputDirectory tools

.\tools\OpenCover.4.6.519\tools\OpenCover.Console.exe -target:.\tools\NUnit.Runners.2.6.4\tools\nunit-console.exe -targetargs:"/nologo /noshadow .\src\ActionRpgKit\bin\Debug\ActionRpgKit.NUnit.Tests.dll" -filter:"+[ActionRpgKit]*" -register:user
.\tools\coveralls.net.0.412\tools\csmacnz.Coveralls.exe --opencover -i .\results.xml --commitId $env:APPVEYOR_REPO_COMMIT --commitBranch $env:APPVEYOR_REPO_BRANCH --commitAuthor $env:APPVEYOR_REPO_COMMIT_AUTHOR --commitEmail $env:APPVEYOR_REPO_COMMIT_AUTHOR_EMAIL --commitMessage $env:APPVEYOR_REPO_COMMIT_MESSAGE

nuget install Doxygen -Version 1.8.9.2 -OutputDirectory tools
nuget install GraphViz.NET -OutputDirectory tools



################################################################################
##### Setup this script and get the current gh-pages branch.               #####

echo "Setting up the script..."
# Exit with nonzero exit code if anything fails
set -e

# Create a clean working directory for this script.
mkdir code_docs
cd code_docs

# Get the current gh-pages branch
git clone -b gh-pages https://git@github.com/PaulSchweizer/ActionRpgKit.git
cd ActionRpgKit

##### Configure git.
# Set the push default to simple i.e. push only the current branch.
git config --global push.default simple
# Pretend to be an user called Travis CI.
git config user.name "Appveyor"
git config user.email "travis@travis-ci.org"

# Remove everything currently in the gh-pages branch.
# GitHub is smart enough to know which files have changed and which files have
# stayed the same and will only update the changed files. So the gh-pages branch
# can be safely cleaned, and it is sure that everything pushed later is the new
# documentation.
rm -rf *

# Need to create a .nojekyll file to allow filenames starting with an underscore
# to be seen on the gh-pages site. Therefore creating an empty .nojekyll file.
# Presumably this is only needed when the SHORT_NAMES option in Doxygen is set
# to NO, which it is by default. So creating the file just in case.
echo "" > .nojekyll

################################################################################
##### Generate the Doxygen code documentation and log the output.          #####
echo 'Generating Doxygen code documentation...'
# Redirect both stderr and stdout to the log file AND the console.
doxygen config_appveyor.dox 2>&1 | tee doxygen.log

################################################################################
##### Copy all files from the html directory                               #####
cp -r ./html/. ./

echo 'Uploading documentation to the gh-pages branch...'
# Add everything in this directory (the Doxygen code documentation) to the
# gh-pages branch.
# GitHub is smart enough to know which files have changed and which files have
# stayed the same and will only update the changed files.
git add --all

# Commit the added files with a title and description containing the Travis CI
# build number and the GitHub commit reference that issued this build.
git commit -m "Deploy code docs to GitHub Pages Travis build:" -m "Commit: "

# Force push to the remote gh-pages branch.
# The ouput is redirected to /dev/null to hide any sensitive credential data
# that might otherwise be exposed.
git push --force "https://${GH_REPO_TOKEN}@github.com/PaulSchweizer/ActionRpgKit.git" > /dev/null 2>&1
