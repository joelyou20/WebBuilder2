#!/bin/bash

if [ $# -ne 3 ]; then
    echo "Usage: $0 clonedRepoName newRepoName gitUserName"
    exit 1
fi

clonedRepoName=$1
newRepoName=$2
gitUserName=$3

templateRepoUrl="https://github.com/$gitUserName/$clonedRepoName"
echo "Debug: $templateRepoUrl"

newRepoUrl="https://github.com/$gitUserName/$newRepoName"
echo "Debug: $newRepoUrl"

cd ~
git clone --bare $templateRepoUrl
cd $clonedRepoName.git
git push --mirror $newRepoUrl
cd ~
rm -rf $clonedRepoName.git