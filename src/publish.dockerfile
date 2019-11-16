FROM mcr.microsoft.com/dotnet/core/sdk:3.0 as build-env



FROM build-env AS source
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ./*.sln ./
COPY ./*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ./${file%.*}/ && mv $file ./${file%.*}/; done

# Copy everything else and build
COPY ./ ./




FROM source AS pack

ARG NUGET_PROJECT=$NUGET_PROJECT
ARG NUGET_VERSION=$NUGET_VERSION

WORKDIR /src

RUN echo "Packing NUGET $NUGET_PROJECT:$NUGET_VERSION"

# 1. change version & abstractions dependencies
RUN ./.ci/set-version.sh --version $NUGET_VERSION

# 2. append build info into description - sha, information version
# todo

# 3. build and pack
RUN ./.ci/pack-or-retry.sh --attempts 60 --delay 60 --output /nuget --project ./${NUGET_PROJECT}/${NUGET_PROJECT}.csproj



FROM build-env AS publish
ARG NUGET_PROJECT=$NUGET_PROJECT
ARG NUGET_VERSION=$NUGET_VERSION
ARG NUGET_TOKEN=$NUGET_TOKEN

WORKDIR /nuget
COPY --from=pack /nuget .

RUN echo "Publishing NUGET $NUGET_PROJECT:$NUGET_VERSION" \
 && ls -lah

RUN dotnet nuget push $NUGET_PROJECT.$NUGET_VERSION.nupkg -k $NUGET_TOKEN -s https://api.nuget.org/v3/index.json