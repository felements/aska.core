#!/bin/bash

version=0.1.0

while [ "$1" != "" ]; do
    case $1 in
        -v | --version )        shift
                                version=$1
                                ;;
        * )                     echo 'Wrong parameter'
                                exit 1
    esac
    shift
done



echo "Settings projects version to $version"
# https://stackoverflow.com/questions/9612090/how-to-loop-through-file-names-returned-by-find

find . -name "*.csproj" -print0 | while read -d $'\0' fname; do \
      echo "$fname" && \
      sed -i \
        -e "s/<Version>.*<\/Version>/<Version>$version<\/Version>/g" \
        -e "s/<ProjectReference.*EntityStorage\.Abstractions.*\/>/<PackageReference Include=\"Aska.Core.EntityStorage.Abstractions\" Version=\"[$version]\" \/>/g" \
        $fname && \
      cat $fname ; done