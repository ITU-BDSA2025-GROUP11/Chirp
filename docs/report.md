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

## Domain model (Therese)

Here comes a description of our domain model.

## Architecture — In the small (Morten)

## Architecture of deployed application (Milja)

The deployed application is hosted on Azure Web Services. 
The application is deployed from GitHub via an auto-generated workflow file.

There were a lot of issues with getting Azure to run the deployed application, this was solved by adding
a startup command on Azure. Without this the Web App did not run the program.

The startup command is simply `dotnet Chirp.Web.dll`.

Secondly because the application relies on an SQLite database-file, which is not "sent" to Azure as part of the deployment,
the database file should be created if the program is run without an existing file.
This code was added in Chirp.Web.Program.cs.
As a result of this the SQLite database file is regenerated on every deployment, this means that user data and cheep data is only persistent within a deployment. 
When a new feature is merged into, main resulting in a new deployment, all users and cheeps not specified in the DbInitializer-file are lost.


## User activities (Joakim)

## Sequence of functionality/calls trough _Chirp!_ (Emilie)

# Process

## Build, test, release, and deployment (Therese og Milja)

## Team work (Emilie og Milja)

## How to make _Chirp!_ work locally (Morten)

## How to run test suite locally (Morten)

# Ethics

## License (Joakim)

## LLMs, ChatGPT, CoPilot, and others (Joakim)
