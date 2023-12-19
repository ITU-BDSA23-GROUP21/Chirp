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

1. Clone the project from the main branch on https://github.com/ITU-BDSA23-GROUP21/Chirp.
1. Create an MSSQL database. The program is only tested using Docker Desktop, but a local SQL server installation *should* work as well.
    - Due to hardware issues on some Mac computers, you must create a Postgres database instead if you are running OSX.
1. In the user secrets for the Chirp.Razor project, add the connection string to the database. They key for the secret must be *ConnectionString*
1. Run the Chirp.Razor project using the Dotnet CLI 

## How to run test suite locally

# Ethics

## License
This application uses the MIT license. For the full license agreement, see https://github.com/ITU-BDSA23-GROUP21/Chirp/blob/main/LICENSE
## LLMs, ChatGPT, CoPilot, and others
No LLMs were used in the development of this application.