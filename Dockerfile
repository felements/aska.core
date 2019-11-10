FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build

ENV NUGET_PROJECT=$NUGET_PROJECT
ENV NUGET_VERSION=$NUGET_VERSION

COPY ./docker/docker-entrypoint.sh /
RUN chmod +x /docker-entrypoint.sh

WORKDIR /src
# Copy csproj and restore as distinct layers
COPY ./src/*.sln ./
COPY ./src/*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ./${file%.*}/src/ && mv $file ./${file%.*}/; done
RUN dotnet restore

# Copy everything else and build
COPY ./src ./
RUN dotnet build --no-restore

ENTRYPOINT /docker-entrypoint.sh --project=${NUGET_PROJECT} --action=build

FROM build as pack

RUN echo "pack ${NUGET_PROJECT}"

ENTRYPOINT /docker-entrypoint.sh --project=${NUGET_PROJECT} --action=pack



# FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build-env
# ARG NUGET_PROJECT
# ARG NUGET_KEY

# ENV NUGET_PROJECT=${NUGET_PROJECT}

# VOLUME [ "/nuget" ]



# # todo - install tools


# FROM build-env AS build

# WORKDIR /source

# # Copy csproj and restore as distinct layers
# COPY ./src/*.sln ./
# COPY ./src/*/*.csproj ./
# RUN for file in $(ls *.csproj); do mkdir -p ./${file%.*}/src/ && mv $file ./${file%.*}/; done
# RUN dotnet restore

# # Copy everything else and build
# COPY . ./


# RUN echo "\n export PATH=\"$PATH:/root/.dotnet/tools\" \n" >> ~/.bash_profile
# RUN dotnet tool install -g gitversion.tool --framework netcoreapp30 --version 5.1.2
# RUN dotnet tool list -g
# RUN dotnet gitversion

# RUN dotnet build --no-restore



# FROM build AS pack
# #ARG NUGET_PROJECT

# VOLUME [ "/nuget" ]
# WORKDIR /source

# COPY --from=build /source . 

# RUN echo ">> building ...${NUGET_PROJECT}"
# RUN dotnet pack -c Release -o /nuget ${NUGET_PROJECT}/src/${NUGET_PROJECT}.csproj --no-restore --no-build



# FROM pack AS publish
# #ARG NUGET_PROJECT
# #ARG NUGET_KEY

# WORKDIR /nuget
# COPY --from=pack /nuget . 

# RUN ls -lah .
# RUN echo 'done.'
# # todo: run in loop for 1 hour (retry in ci.yml)

# # <PackageReference Include="Aska.Core.EntityStorage.Abstractions" Version="0.1.1-alpha.1" />


# ## todo
# # - tests target
# # - nuget verify target