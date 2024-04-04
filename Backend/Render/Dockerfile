# Use Ubuntu as the base for the custom .NET environment
FROM ubuntu:20.04 AS base

# Install .NET dependencies, NFS support, wget, and other essential tools
RUN apt-get update && apt-get install -y \
    software-properties-common \
    nfs-common \
    wget \
    && rm -rf /var/lib/apt/lists/*

# Install Microsoft package signing key and repository
RUN wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb \
    && dpkg -i packages-microsoft-prod.deb \
    && rm packages-microsoft-prod.deb

# Install .NET SDK and Runtime
RUN apt-get update && \
    apt-get install -y apt-transport-https && \
    apt-get update && \
    apt-get install -y dotnet-sdk-8.0 dotnet-runtime-8.0

ENV APP_UID=1000

USER $APP_UID

WORKDIR /app

EXPOSE 8080
EXPOSE 8081

RUN mkdir -p /app/data

# Copy the rest of your application's code and follow the same build steps as before
COPY . /app

# Assuming you have a similar structure for the build and publish stages,
# you can follow the same approach as in your original Dockerfile.
# Remember to adjust paths and commands as needed for your application.

# Final setup
COPY Upload/entrypoint.sh /entrypoint.sh
USER root
RUN chmod +x /entrypoint.sh && chown $APP_UID /app/data
#USER $APP_UID
ENTRYPOINT ["/bin/sh", "/entrypoint.sh"]
