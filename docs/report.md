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

The domain model in Chirp.Core consists of two primary entities: Author and Cheep. 
The Author class extends the standard ASP.NET Core Identity IdentityUser class.

This inheritance provides built-in security attributes such as:
- UserName
- Email
- GUID-based Id

while adhering to industry standards for authentication.

Additionally, the Author class adds collections of authors whom a user is following and their followers. 
It also adds collections of cheeps posted by an author, 
as well as liked and disliked cheeps—all specific attributes for our application. 
This behavior represents a one-to-many relationship between an author and their posts, 
as well as other cheeps they have liked or disliked.

The Cheep class represents the fundamental unit of communication within the system. 
The entity contains information about the Cheep itself and the author, such as:
- CheepId: An identifier unique to the cheep.
- Text: The content of the cheep, limited to 160 characters.
- Author: The Author entity that created the cheep; this acts as a foreign key.
- Timestamp: The time at which the cheep was created.

Much like the Author entity, the Cheep entity also contains collections of authors 
who have liked and disliked the Cheep—all specific attributes for our application.


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
