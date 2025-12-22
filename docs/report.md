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
The following section describes how a user navigates through the Chirp application
from registering a new account to navigating the different timelines, interacting with other users and finally deleting an existing account.

### 1. Start up
Upon opening the website the user is presented with the Public Timeline. Here the user can view Cheeps from other authors but is unable to interact with them. 
Via the buttons in the banner the user is then presented with two options.
* Login
* Register

Should the user choose the login option they are prompted to login with an existing account.
If however the user chooses register they are prompted for a Username and Email both of which must be unique within the system. Additionally the user must create a password that meets the following security criteria:
* Minimum 6 characters.
* At least one uppercase and one lowercase letter.
* At least one number.
* At least one special character.

Once on the login page the user also has a third option
* Login with github

This allows the user to login via their github account.

### 2. Timelines
Once logged in, the user is redirected to the public timeline. Here Cheeps from all users are displayed along with their respective likes and dislikes.
From here the user can perform the following actions.

* Write and publish their own Cheeps.
* Like or dislike Cheeps.
* Follow other authors.

The user can switch to their private timeline. This view filters the content to only show.
*  The user's own Cheeps.
*  Cheeps from authors the user is following.

If the user decides to unfollow an author while on their own private timeline the page refreshes and that author's Cheeps immediately disappear from the private timeline.

Clicking on another author's username in either the public or private timeline will link directly to their timeline showing all their cheeps.
### 3. About me.
The user can access the about Me page from the banner at any time. This section displays the user's profile information
* Username
* Email
* List of followed accounts.

At the bottom of this page the user finds the forget me button.
Once pressed The user's Name and Email are anonymized in the database effectively deleting their account,
the user is then immediately logged out and the user will be unable to log back in with the anonymized credentials.

All cheeps from the deleted account will also be invisible and inaccessible for all remaining users.
### 4. Logout
Finally a user can choose to perform a standard logout. This returns them to the initial unauthenticated state on the Public Timeline, where they can choose to log in again or register a new account.


## Sequence of functionality/calls trough _Chirp!_ (Emilie)

# Process

## Build, test, release, and deployment (Therese og Milja)


## Team work (Emilie og Milja)

## How to make _Chirp!_ work locally (Morten)

## How to run test suite locally (Morten)

# Ethics

## License (Joakim)

## LLMs, ChatGPT, CoPilot, and others (Joakim)
