#!/bin/bash

docker-compose build
if [ $? -ne 0 ]; then
    echo "Build failed. Exiting."
    exit 1
fi

BUILD_NUMBER=$RANDOM
echo BUILD_NUMBER=$BUILD_NUMBER
docker tag book-and-dock-backend-app maxcool101/book-and-dock-backend-app:amd64_$BUILD_NUMBER

docker push maxcool101/book-and-dock-backend-app:amd64_$BUILD_NUMBER
if [ $? -ne 0 ]; then
    echo "Push failed. Exiting."
    exit 1
fi