#!/bin/sh
docker run --rm -it -e "SPACES_KEY=$1" -e "SPACES_SECRET=$2" -v $(pwd):/app/ -w /app mcr.microsoft.com/dotnet/sdk:5.0 dotnet run -- "$3" "$4" "$5"
