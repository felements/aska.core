#!/bin/bash

# requirements
# - sudo apt install jq
# - docker pull rg.nl-ams.scw.cloud/askaone/gitversion:latest

# usage
# > ./getver.sh -t FullSemVer

type=
url=
branch=
sha=
user=
password=
image=rg.nl-ams.scw.cloud/askaone/gitversion:latest

while [ "$1" != "" ]; do
    case $1 in
        -t | --type )           shift
                                type=$1
                                ;;
        --url )                 shift
                                url=$1
                                ;;
        --user )                shift
                                user=$1
                                ;;
        --password )            shift
                                password=$1
                                ;;
        --branch )              shift
                                branch=$1
                                ;;
        --sha )              shift
                                sha=$1
                                ;;                     
        * )                     echo 'Wrong parameter'
                                exit 1
    esac
    shift
done

docker pull $image > /dev/null

docker run --tmpfs /git $image /url $url /b $branch /c $sha /dynamicRepoLocation /git /u $user /p $password \
| sed -e 's/.*{.*/{/g' -e 's/.*}.*/}/g' -e 's/.*=.*/ /g' | jq ".$type"