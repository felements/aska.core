#!/bin/bash

# https://stackoverflow.com/questions/192249/how-do-i-parse-command-line-arguments-in-bash
for i in "$@"
do
case $i in
    -p=*|--project=*)
    PROJECT="${i#*=}"
    shift 
    ;;
    -a=*|--action=*)
    ACTION="${i#*=}"
    shift 
    ;;
    -v=*|--version=*)
    VERSION="${i#*=}"
    shift 
    ;;
    #--default)
    #DEFAULT=YES
    #shift # past argument with no value
    #;;
    *)
          # unknown option
    ;;
esac
done

echo ">> ASKA core tools build script"
echo ''
echo "PROJECT    = ${PROJECT}"
echo "ACTION     = ${ACTION}"
echo "VERSION    = ${VERSION}"

