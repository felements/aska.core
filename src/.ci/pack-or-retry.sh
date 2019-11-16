#!/bin/bash

project=
output=
attempts=5
delay=15

while [ "$1" != "" ]; do
    case $1 in
        -p | --project )        shift
                                project=$1
                                ;;
        -o | --output )         shift
                                output=$1
                                ;;
        -n | --attempts )       shift
                                attempts=$1
                                ;;
        -d | --delay )          shift
                                delay=$1
                                ;;
        * )                     echo 'Wrong parameter'
                                exit 1
    esac
    shift
done



echo "Packing the project $project to $output"
echo " - max attempts: $attempts"
echo " - delay:        ${delay}s"

n=0
until [ $n -ge $attempts ] ; do 
   dotnet pack -c Release -o $output $project && break  
   n=$[$n+1]
   echo "Attempt #$n/$attempts. Retry in ${delay}s..."
   sleep ${delay}s
   echo 'Retrying ...' ; done

