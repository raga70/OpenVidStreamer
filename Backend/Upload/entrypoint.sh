#!/bin/sh

# Mount the NFS share
mount -t nfs $NFS_SERVER:$NFS_PATH /app/data

# Check if mount was successful
if [ $? -ne 0 ]; then
    echo "Failed to mount NFS share"
    exit 1
fi

# Start the .NET application
exec dotnet Upload.dll
