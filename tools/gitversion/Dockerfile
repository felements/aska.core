FROM mcr.microsoft.com/dotnet/core/sdk:3.0  as build-env

# https://stackoverflow.com/questions/51977474/install-dotnet-core-tool-dockerfile - dotnet tools in docker 
RUN dotnet tool install -g gitversion.tool
ENV PATH="${PATH}:/root/.dotnet/tools"

# https://dev.to/jake/using-libcurl3-and-libcurl4-on-ubuntu-1804-bionic-184g - libcurl3
COPY ./lib/* /usr/lib/
ENV LD_PRELOAD="/usr/lib/libcurl.so.3 /usr/lib/libssl.so.1.0.0 /usr/lib/libcrypto.so.1.0.0"

WORKDIR /src
VOLUME /src
# todo - mount .git and gitversion.yml as volume for context

ENTRYPOINT [ "dotnet", "gitversion" ]
#RUN  dotnet gitversion