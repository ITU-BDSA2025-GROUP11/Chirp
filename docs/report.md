---
title: _Chirp!_ Project Report
subtitle: ITU BDSA 2025 Group `<11>`
author:
- "Emilie Bliddal Ravn Larsen <ebll@itu.dk>"
- "Joakim-David Stauning Prewett <jpre@itu.dk>"
- "Milja Noomi Hildigunnsdottir <mhil@itu>"
- "Morten Holtorp Erlandsen <merl@itu>"
- "Therese Schultz-Nielsen <thsch@itu>"
numbersections: true
---

**Planlagt aflevering d. 30 december**

# Design and Architecture of _Chirp!_
![[DomainModelClassDiagram.jpg]]
The architecture of the application follows an onion structure, partitioning the src folder into three subfolder: Chirp.Core, Chirp.Infrastructure and
Chirp.Web. These represent the different layers of the program where Chirp.Core resides as the base of the program, defining the entity classes as well
as as containing Data Transfer Objects (DTOs) without having references to neither Chirp.Infrastructure nor Chirp.Web, keeping the core independant 
from other classes. 

This core is used by Chirp.Infrastructure which uses the entity classes to create migrations when creating the database and **(læs op på dette)**
Furthermore it uses DTOs when transferring data to contain the data retrieved and queries the database, giving functionality to the application.

This is used by the final layer, Chirp.Web which allows a user to interact with the application, applying the functionality defined in Chirp.Infrastructure
and thus it has references to both the infrastructure and web folder.

All this will be explained in detail in the this chapter, beginning in the center of the onion: The domain model

## Domain model (Therese)

At the lowest abstraction layer of Chirp in Chirp.Core resides a domain model, in which two POCO classes are represented: *Author* and *Cheep*:

the author class inherits from Identity.User which is a part of the ASPNETCORE.Identity standard from where we get numerous attributes such as
UserName and Email. Additionally the author class adds lists of authors like who a user is following and their followers. It also
adds lists of cheeps such as the cheeps of an author and liked and disliked cheeps, all specific attributes for our application.

## Architecture — In the small (Morten)
![OnionArchitectureDiagram.png](diagrams/OnionArchitectureDiagram.png)

Above figure illustrates the onion architecture which the Chirp! Codebase is built upon. Dependencies flow exclusively inward, ensuring loosely coupled layers with inverted control. The application is structured into the three following layers:

- Chirp.Core: This layer is the innermost layer of Chirp. Containing the Domain Model and the Data transfer Objects. Chirp.Core has no external dependencies.
- Chirp.Infrastructure: Manages data persistence and retrieval, this data access interface is implemented using Entity Framework Core. This layer also encapsulates application logic. It manages data flow between the user interface and the repositories. 
- Chirp.Web: The outermost layer constraining the razor pages for user interaction.

This structure ensures the application is loosely coupled, maintainable and testable


## Architecture of deployed application (Milja)

## User activities (Joakim)

## Sequence of functionality/calls trough _Chirp!_ (Emilie)

# Process

## Build, test, release, and deployment (Therese og Milja)


## Team work (Emilie og Milja)

## How to make _Chirp!_ work locally (Morten)
Git must be installed, as a prerequisit for the following steps.
Once installed, the following command can be run, to clone the repository.
```
git clone https://github.com/ITU-BDSA2025-GROUP11/Chirp.git
```
After cloning the repository, user-secrets must be set up to authorize third-party login.
Locate the freshly cloned repository on your device and navigate to the ```src/Chirp.Web/``` directory.

Before setting up user-secrets. Ensure dotnet-ef is installed. This step can be skipped, if ef is already configured.
```
dotnet tool install --global dotnet-ef
```
From the ```/Chirp.Web/``` directory, run the following commands to setup the user-secrets.
```
dotnet user-secrets init
```
```
dotnet user-secrets set "authentication:github:clientId" "Ov23ctwQATqvMi87GmtN"
```
```
- dotnet user-secrets set "authentication:github:clientSecret" "e04c4c4cf97ccb87946a3a0e9ee9080ac0528995"
```
To confirm thats user-secrets has been setup successfully, the fowllowing command can be run from the Chirp.Web directory.
```
dotnet user-secrets list
```
Which should output both the authentication:github:clientsecret and authentication:github:clientID

With all the prerequisit for running the program locally done, the program can be run from the chirp.Web 
directory in the terminal, using the following command
```
dotnet run program.cs
```

## How to run test suite locally (Morten)
This chapter assumes you have followed the previous chapter and setup user-secrets and the required dependencies.

Tests should be run from the project rooty ```/Chirp/```. However First, build the test project to generate the installation script. 
Run the following command from the root directory:
```
dotnet build
```

Playwright is required to successfully run the End2End-Tests. To install Playwright, run the following command from the ```
/Chirp/``` directory
```
pwsh test/WebTest/bin/Debug/net8.0/playwright.ps1 install
```
The website msut be running locally, for the playwrigt test to successfully run. 
Run the project from ```src/Chirp.Web/``` with the command ```dotnet run```

Having completed the prerequisite for the test to run, we can finally execute the tests.
Run the test from the ```/Chirp/``` directory, using the following command:
```
dotnet test --no-build
```

This should run the enitre test-suite, consisting of unit tests, Intergration tests and End2End tests.

# Ethics

## License (Joakim)

## LLMs, ChatGPT, CoPilot, and others (Joakim)
