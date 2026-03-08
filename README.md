# Customer Management API

## Repository Structure

```
/
├── client/   # React (Vite) frontend
└── server/   # .NET 10 backend
    ├── server/         # API project
    └── server.Tests/   # Test project
```

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/get-started)
- [Node.js & npm](https://nodejs.org)

## Setup

### 1. Start the MSSQL Database

Run the following command to start a SQL Server instance in Docker:

```bash
docker run -e 'ACCEPT_EULA=Y' \
           -e 'SA_PASSWORD=YourStrong!Passw0rd' \
           -p 1401:1433 \
           --name sqlserver \
           -d mcr.microsoft.com/mssql/server:2022-latest
```

The connection string in `appsettings.json` is already configured for this container. If you prefer to use a different database setup, update the `DefaultConnection` value in `appsettings.json` accordingly:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "your-connection-string-here"
  }
}
```

### 2. Configure User Secrets

This project requires a Postit API key stored in user secrets. Run the following command from the `server/` directory:

```bash
dotnet user-secrets set "PostitApiKey" "postit.lt-examplekey"
```

Replace `postit.lt-examplekey` with your actual API key if you have one.

### 3. Run the Backend

From the `server/server/` directory, run:

```bash
dotnet run
```

Database migrations are applied automatically on startup.

### 4. Run the Frontend

From the `client/` directory, run:

```bash
npm install
npm run dev
```

## Running Tests

From the `server/` directory, run:

```bash
dotnet test
```
