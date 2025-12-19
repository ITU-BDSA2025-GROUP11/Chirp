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
as as containing Data Transfer Objects (DTOs) which are all used in the infrastructure layer for the functionality of the program, but without having references
to netiher Chirp.Infrastructure nor Chirp.Web, keeping the core independant from other classes. 

This core is used by Chirp.Infrastructure which uses the entity classes to create migrations when creating the database and **(læs op på dette)** 

## Domain model (Therese)

At the "bottom layer" of Chirp in Chirp.Core resides a domain model, in which two POCO classes are represented: *Author* and *Cheep*:

the author class inherits from Identity.User which is a part of the ASPNETCORE.Identity standard from where we get numerous attributes such as
UserName and Email. Additionally the author class adds lists of authors like who a user is following and their followers. It also
adds lists of cheeps such as the cheeps of an author and liked and disliked cheeps, all specific attributes for our application.


## Architecture — In the small (Morten)

## Architecture of deployed application (Milja)

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
