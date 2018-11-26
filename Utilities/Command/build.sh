#!/bin/bash 
appDir=$1
 
function buildApi() {
    cd ../../Web/Api/GildedRose.Api
    start cmd.exe //K dotnet build -c $1 -v normal
}

function buildMembershipDacpac() {
    msBuild=/c/Program\ Files\ \(x86\)/Microsoft\ Visual\ Studio/2017/BuildTools/MSBuild/15.0/Bin/MSBuild.exe
    #appDir=$(cygpath -aw ./../../)
    prjPath="$(cygpath -aw $appDir/Database/Membership/GildedRose.Membership.sqlproj)"
    echo "Building Membership database"
    start cmd.exe //K "$msBuild" $prjPath //t:Build //p:VisualStudioVersion=15.0 //p:Configuration=Debug //v:detailed
}

function buildPlatformDacpac() {
    msBuild=/c/Program\ Files\ \(x86\)/Microsoft\ Visual\ Studio/2017/BuildTools/MSBuild/15.0/Bin/MSBuild.exe
    #appDir=$(cygpath -aw ./../../)    
    prjPath="$(cygpath -aw $appDir/Database/Platform/GildedRose.Platform.sqlproj)"
    echo "Building Platform database"
    start cmd.exe //K "$msBuild" $prjPath //t:Build //p:VisualStudioVersion=15.0 //p:Configuration=Debug //v:detailed
}
 
while :
do
    echo "API"
    echo "API-RELEASE"
    echo "Membership-Dacpac"
    echo "Platform-Dacpac"
    echo "Web"
    echo "Exit"
    read -p "Select build option:`echo $'\n> '`" -r
    echo
    response="${REPLY,,}"

    if [ $response == "api" ]; then
        buildApi Debug
    elif [ $response == "api-release" ]; then
        buildApi Release
    elif [ $response == "platform-dacpac" ]; then
        buildPlatformDacpac
    elif [ $response == "membership-dacpac" ]; then
        buildMembershipDacpac
    elif [ $response == "exit" ] || [ $response == "quit" ]; then
        exit 0
    else
        echo "App $response not found"
    fi
done