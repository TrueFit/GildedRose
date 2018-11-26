#!/bin/bash 
appDir=$(cygpath -aw ./../../)

while :
do
    echo "BUILD"
    echo "DEPLOY"
    echo "RUN"
    echo "Exit"

    read -p "Select option:`echo $'\n> '`" -r
    echo
    response="${REPLY,,}"

    if [ $response == "build" ]; then
        clear
        ./build.sh $appDir
    elif [ $response == "run" ]; then
        clear
        ./run.sh $appDir
    elif [ $response == "deploy" ]; then
        clear
        ./deploy.sh $appDir
    elif [ $response == "exit" ] || [ $response == "quit" ]; then
        exit 0                
    else
        echo "App $response not found"
    fi
done