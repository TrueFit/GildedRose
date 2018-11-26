#!/bin/bash 
appDir=$1

function deployPlatform() {
    BLUE='\033[1;34m'
    NC='\033[0m' # No Color
    sqlPackage=/C/Program\ Files\ \(x86\)/Microsoft\ Visual\ Studio/2017/BuildTools/Common7/IDE/Extensions/Microsoft/SQLDB/DAC/150/SqlPackage.exe

    printf "${BLUE}Publishing Platform Database${NC}\n"
    #appDir=$(cygpath -aw ./../../)

    "$sqlPackage" /Action:Publish\
        /OverwriteFiles:True\
        /SourceFile:"$appDir\\Database\\Platform\\bin\\Debug\\GildedRose.Platform.dacpac" \
        /Profile:"$appDir\\Database\\Platform\\GildedRose.Platform.publish.xml"\
        /TargetConnectionString:"server=(localdb)\\MSSQLLocalDB;database=GildedRose.Platform;Integrated Security=True;" \
        /Variables:Environment=Local

    printf "${BLUE}Published Platform Database${NC}\n"
}

function deployMembership() {
    BLUE='\033[1;34m'
    NC='\033[0m' # No Color
    sqlPackage=/C/Program\ Files\ \(x86\)/Microsoft\ Visual\ Studio/2017/BuildTools/Common7/IDE/Extensions/Microsoft/SQLDB/DAC/150/SqlPackage.exe
    
    printf "${BLUE}Publishing Membership Database${NC}\n"
    #appDir=$(cygpath -aw ./../../)

    "$sqlPackage" /Action:Publish\
        /OverwriteFiles:True\
        /SourceFile:"$appDir\\Database\\Membership\\bin\\Debug\\GildedRose.Membership.dacpac" \
        /Profile:"$appDir\\Database\\Membership\\GildedRose.Membership.publish.xml"\
        /TargetConnectionString:"server=(localdb)\\MSSQLLocalDB;database=GildedRose.Membership;Integrated Security=True;" \
        /Variables:Environment=Local

    printf "${BLUE}Published Membership Database${NC}\n"
}

while :
do
    echo "Platform"
    echo "Membership"   
    echo "Exit"

    read -p "Select database deploy option:`echo $'\n> '`" -r
    echo
    response="${REPLY,,}"

    if [ $response == "platform" ]; then
        deployPlatform
    elif [ $response == "membership" ]; then
        deployMembership       
    elif [ $response == "exit" ] || [ $response == "quit" ]; then
        exit 0
    else
        echo "App $response not found"
    fi
done