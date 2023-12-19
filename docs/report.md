---
title: _Chirp!_ Project Report
subtitle: ITU BDSA 2023 Group 21
author:
- "Emil Andreas Sondum <eson@itu.dk>"
- "Lucas Roy Guldbrandsen <lgul@itu.dk>"
- "Rafael Steffen Nguyen Jensen <rafj@itu.dk>"
- "Thøger Bro <bros@itu.dk>"
- "Tobias Dirchsen Engbo <toen@itu.dk>"
numbersections: true
---

# Design and Architecture of _Chirp!_

## Domain model

Here comes a description of our domain model.

![Illustration of the _Chirp!_ Data model as UML class diagram.](docs/images/domain_model.png)

## Architecture — In the small

## Architecture of deployed application
The following diagram shows the three parts of our deployed application.
- Client: A browser on the users machine. Sends HTTP calls to the server.
- Server: RazorPages project available at https://bdsagroup21chirprazor.azurewebsites.net/. Receives client request, and responds with HTML pages for the client to render. Communicates with database to fetch or update data.
- Database. MSSQL database hosted in Azure.

## User activities

## Sequence of functionality/calls through _Chirp!_

# Process

## Build, test, release, and deployment

## Team work

## How to make _Chirp!_ work locally
Prerequisites:
- Dotnet
- Git
- Docker Desktop

1. Clone our Chirp project from the main branch on its GitHub page at https://github.com/ITU-BDSA23-GROUP21/Chirp.  
2. If you do not have Docker Desktop installed then follow the steps on their [website](https://www.docker.com/products/docker-desktop/) to download and install Docker Desktop.  
3. Open the terminal and pull the Docker Image of Microsoft SQL Server with the command
    - Due to hardware issues on some Mac computers, you must create a Postgres database instead if you are running OSX.
`docker pull mcr.microsoft.com/mssql/server:2022-latest`.
4. Start up a container based on the image pulled from before with the command  
`docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=YourPassword123" -p 1433:1433 --name sql1 --hostname sql1 -d mcr.microsoft.com/mssql/server:2022-latest`.
    - If you are using Mac and running OSX then start a docker with the Postgres image instead.
5. In the terminal navigate to the where you have cloned the project to and navigate to the folder `Chirp/src/Chirp.Razor`.  
6. Initiate user secrets for the project with `dotnet user-secrets init`.  
7. Store the connection string to the database container in a user secret called `ConnectionString` with this command:  
`dotnet user-secrets set "ConnectionString" "Server=localhost;Database=TestDB;User Id=SA;Password=YourPassword123;TrustServerCertificate=True;"`.  
8. Now run the application with the command `dotnet run`

## How to run test suite locally

# Ethics

## License
This application uses the MIT license. For the full license agreement, see https://github.com/ITU-BDSA23-GROUP21/Chirp/blob/main/LICENSE
## LLMs, ChatGPT, CoPilot, and others
No LLMs were used in the development of this application.