#!/bin/bash

echo "Prepare DB context for tests"

dotnet ef dbcontext list

dotnet ef database update