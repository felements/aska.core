#!/bin/bash

# requirements
# - sudo apt install jq
# - docker pull rg.nl-ams.scw.cloud/askaone/gitversion:latest

# usage
# > ./getver.sh -t FullSemVer

type=

while [ "$1" != "" ]; do
    case $1 in
        -t | --type )           shift
                                type=$1
                                ;;
        * )                     echo 'Wrong parameter'
                                exit 1
    esac
    shift
done

docker run -it -v /home/frame/projects/aska.core:/src rg.nl-ams.scw.cloud/askaone/gitversion:latest \
| sed -e 's/.*{.*/{/g' -e 's/.*}.*/}/g' -e 's/.*=.*/ /g' | jq ".$type"