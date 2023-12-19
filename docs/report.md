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
We use two GitHub Actions workflows, one for testing, and one for release / deployment. The **test workflow** is triggered on any push to the main branch, and has to complete successfully before any pull request to main can be completed. Together with our branch policy that forbids pushing directly to main, this means that any code that reaches main has passed the tests.

The **deployment workflow** is triggered by pushing a tag to the main branch, that matches the regex `v*.*.*`. This workflow builds the application, publishes it to GitHub, and deploys it to Azure. Database schema synchronization is not included here, as it is performed during startup of the application, when it runs a new version.

## Team work

# Unresolved tasks
- [217](https://github.com/ITU-BDSA23-GROUP21/Chirp/issues/217): We were not able to get our e2e UI tests running in our GitHub Actions workflow yet. This should be fixed, so they will be integrated into our automatic testing, and we can be certain that all code in the main branch has passed the tests.
- [208](https://github.com/ITU-BDSA23-GROUP21/Chirp/issues/208): When anonymizing a user after they click "Forget about me", we do not delete the user entry in our Azure AD B2C. Ideally the user should also be deleted there, but it was not considered a high priority to implement at this point.
- [196](https://github.com/ITU-BDSA23-GROUP21/Chirp/issues/196): Every time the user interacts with the page, the page is reloaded. I.e. when a user follows another user, or likes a cheep, the page is reloaded, and they lose their position on the page. This is not a great user experience, but as the system is still usable, it was considered a higher priority to fully implement the other features.

<!-- - [44](https://github.com/ITU-BDSA23-GROUP21/Chirp/issues/44): Workflow stuff. Should be closed as will not be done?
- [204](https://github.com/ITU-BDSA23-GROUP21/Chirp/issues/204): Page numbers. Could be added?
- [114](https://github.com/ITU-BDSA23-GROUP21/Chirp/issues/114): Unit tests. This can't really be left here?
- [211](https://github.com/ITU-BDSA23-GROUP21/Chirp/issues/211): In code documentation. Should be closed before hand-in. -->


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

<!-- Should we write some considerations of how the packages we use impacts our choice of license?
     And have we confirmed that MIT license is OK with all the packages we added later on? -->
## LLMs, ChatGPT, CoPilot, and others
No LLMs were used in the development of this application.