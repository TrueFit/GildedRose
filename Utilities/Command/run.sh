#!/bin/bash 
appDir=$1

function runApi() {
    cd ../../Web/Api/GildedRose.Api
    start cmd.exe //C dotnet watch -v run
}

while :
do
    echo "API"
    echo "Web"
    echo "Exit"

    read -p "Select run option:`echo $'\n> '`" -r
    echo
    response="${REPLY,,}"

    if [ $response == "api" ]; then
        runApi   
    elif [ $response == "exit" ] || [ $response == "quit" ]; then
        exit 0
    else
        echo "App $response not found"
    fi
done