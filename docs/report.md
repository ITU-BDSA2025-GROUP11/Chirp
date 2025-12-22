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
The decision to adopt the MIT License for the project was based on two primary factors:

* The scope of the application
* The educational nature of the project

With these two factors in mind, the simplicity of the MIT license was ideal for a project of relatively small scope as it is easy to implement and future developers only needs to read a small paragraph to fully understand the parameters of the license.

The most significant consideration however was the project’s educational context. As it is unlikely parts of Chirp would be used in a high-stakes commercial environment it was prioritized to take future students of the course into consideration. The MIT license ensures that future students can study, modify, and take inspiration from our implementation without fear of legal repercussions.


## LLMs, ChatGPT, CoPilot, and others (Joakim)
From the outset of the project the decision was made to keep the use of Large language models (LLMs) to a minimum during the development of the Chrip application.
Should the need for AI assistance arise during development the group would handle it with transparency both internally
and by crediting the LLM as co-author in the given part of the code. 
This approach however, proved challenging in practice as ChatGPT does not have an associated GitHub account. As an alternative solution, the LLM was credited in writing within Git commit messages whenever its assistance was relevant.

The use of AI would turn out to be mostly unnecessary, however in some instances the development would reach a bottleneck and the group decided to consult ChatGPT. 
This was done during the creation of the chripDbContextFactory and the initial implementation of identity package.

Troubles also arose during the deployment of the application to Azure where LMMs were consulted but ultimately yielded no tangible solutions so here it was discarded.

When consulted, the LLM were used as a substitute for a teaching assistant providing guidance and explanations rather than generating concrete code implementations.

LLMs would also prove useful for interpreting and understanding large and complex error messages produced by JetBrains Rider, particularly during migration related issues encountered in the first half of the development process.
