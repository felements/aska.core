FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS source


WORKDIR /src
# Copy csproj and restore as distinct layers
COPY ./*.sln ./
COPY ./*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ./${file%.*}/ && mv $file ./${file%.*}/; done
RUN dotnet restore

# Copy everything else and build
COPY ./ ./




FROM source AS build
ARG NUGET_PROJECT=$NUGET_PROJECT

WORKDIR /src

RUN dotnet build -c Release -o /out --no-restore ./$NUGET_PROJECT/$NUGET_PROJECT.csproj




# todo: build tests
# todo: entrypoint to execute tests