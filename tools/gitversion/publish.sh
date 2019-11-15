#!/bin/bash

docker build -t gitversion:latest .

docker login rg.nl-ams.scw.cloud/askaone -u nologin -p $SCW_SECRET_TOKEN

docker tag gitversion:latest rg.nl-ams.scw.cloud/askaone/gitversion:latest
docker push rg.nl-ams.scw.cloud/askaone/gitversion:latest