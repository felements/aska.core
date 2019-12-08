#!/bin/bash


dotnet ef dbcontext list -p $1

dotnet ef database update -p $1