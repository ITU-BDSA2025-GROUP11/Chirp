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

The SQLite database file is regenerated on every deployment, this means that user data and cheep data is only persistent within a deployment. 
When a new feature is merged into, main resulting in a new deployment, all users and cheeps not specified in the DbInitializer-file are lost.

As part of the deployment specifications on Azure, there is specified a startup-command.
As the program would simply not be run by the Web App otherwise.


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
