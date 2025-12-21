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
The architecture of the application follows an onion structure, partitioning the src folder into three subfolders: Chirp.Core, Chirp.Infrastructure and
Chirp.Web. These represent the different layers of the program where Chirp.Core resides as the base of the program, defining the entity classes as well
as their complementing Data Transfer Objects (DTOs) without having references to neither Chirp.Infrastructure nor Chirp.Web, keeping the core independant 
from other classes. 

This core is then used by Chirp.Infrastructure which queries the database, and furthermore transfers data to contain the data retrieved and queries the database, 
giving functionality to the application.

This is used by the part of the program, Chirp.Web, which allows a user to interact with the application, applying the functionality defined in Chirp.Infrastructure
and thus contains references to both the infrastructure and core folder.

All this will be explained in detail in the following chapter, beginning at the center of the onion: The domain model

## Domain model (Therese)

![Chirp.Core Class Diagram](./diagrams/DomainModelClassDiagram.jpg)

At the lowest abstraction layer of Chirp in Chirp.Core resides a domain model, in which two POCO classes are represented: *Author* and *Cheep*:

The author class inherits from Identity.User which is a part of the ASPNETCORE.Identity standard from where we get numerous attributes such as
UserName, Email and an Id. Additionally the author class adds lists of authors like who a user is following and their followers. It also
adds lists of cheeps such as the cheeps of an author and liked and disliked cheeps, all specific attributes for our application. 

The cheep class has an Id, a text having a contraint causing it to allow a maximum length of 160 characters, an author, a timestamp, 
and two lists of authors keeping track of who has liked and disliked a post. 

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
