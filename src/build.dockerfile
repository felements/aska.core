FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS source


WORKDIR /src

ENV EF_TOOL_VERSION 3.1.0

RUN dotnet tool install --global dotnet-ef --version $EF_TOOL_VERSION     

ENV PATH="/root/.dotnet/tools:${PATH}"

RUN dotnet ef --version

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


FROM source AS test
ARG NUGET_PROJECT=$NUGET_PROJECT
ENV NUGET_PROJECT $NUGET_PROJECT
ENV DB_HOST $DB_HOST
WORKDIR /src

COPY ./.ci/ef-migrate.sh .

ENTRYPOINT echo 'Applying DB changes...' \
 && ./ef-migrate.sh ./$NUGET_PROJECT/$NUGET_PROJECT.csproj\
 && echo "Testing ${NUGET_PROJECT}..." \
 && dotnet test ./$NUGET_PROJECT/$NUGET_PROJECT.csproj

# todo: build tests
# todo: entrypoint to execute tests