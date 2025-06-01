# Use the official .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container
WORKDIR /src

# Copy the solution file and the project file(s)
COPY ["book-and-dock-backend.sln", "./"]
# Copy the project file(s) to their respective directories
COPY ["Backend/Backend.csproj", "Backend/"] 

# Restore dependencies
RUN dotnet restore "book-and-dock-backend.sln"

# Copy the rest of the application files into the correct project directory
COPY . .

# Change to the project directory to publish
WORKDIR /src/Backend

# Publish the application
RUN dotnet publish "Backend.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the official .NET runtime image for running the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

# Set the working directory inside the container for the final image
WORKDIR /app

# Copy the published application from the build stage
COPY --from=build /app/publish .
# Copy .env file to the app directory
COPY .env /app

# Expose the port the application runs on
EXPOSE ${PORT}
# Set environment variables for the application
ENV IS_DOCKER_CONTAINER=TRUE

# Set the entry point for the container
ENTRYPOINT ["dotnet", "Backend.dll"]
