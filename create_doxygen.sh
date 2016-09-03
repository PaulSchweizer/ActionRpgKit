#!/bin/bash

# Publish to the "gh-pages" branch
COMMIT_AUTHOR_EMAIL="paulschweizer@gmx.net"
GH_REPO="@github.com/PaulSchweizer/ActionRpgKit.git"

SOURCE_BRANCH="master"
TARGET_BRANCH="gh-pages"

rm -rf ./doc || exit 0;
mkdir ./doc;

FULL_REPO="https://$GH_TOKEN$GH_REPO"

# Create doxygen
doxyen config.dox

cd ./doc/html
git init
git config user.name "rmflight-travis"
git config user.email "travis"
# cp ../categoryCompare/inst/doc/categoryCompare_vignette.html index.html

git add .
git commit -m "deployed to github pages"
git push --force --quiet $FULL_REPO master:gh-pages
