# WebAPINetCore.API (API Project for Teams and Players)
This API project allows interaction with a database containing information about Teams and Players. It was built following the tutorial from Microsoft Learn.

## Getting Started
These instructions will help you set up the project on your local development environment.

### Prerequisites
Before you start, make sure you have installed:

* Visual Studio 2019, Visual Studio Code, or Visual Studio for Mac

* .NET Core SDK 6.0 or later

### Installing
1. Clone the repository to your local machine:

`git clone https://github.com/santace85/WebAPINetCore.API.git`

2. Open the solution file APITeamsAndPlayers.sln in your preferred development environment.

3. Restore the NuGet packages by right-clicking the solution and choosing "Restore NuGet Packages".

4. Build the solution.

5. Run the project by pressing F5 or using the Run menu.

## Endpoints
The API provides the following endpoints to interact with the database of Teams and Players:

### Teams
* Create a Team: POST http://localhost:5000/Teams

* Query for Teams: GET http://localhost:5000/Teams

* Query by ID: GET http://localhost:5000/Teams/{id}

* Query All Teams: GET http://localhost:5000/Teams

* Query All Teams ordered by Name: GET http://localhost:5000/Teams?orderBy=name

* Query All Teams ordered by Location: GET http://localhost:5000/Teams?orderBy=location

* Add or Remove a Player from a Team: PUT http://localhost:5000/Teams/{id}/player/{playerId}?teamUpdateType=0


### Players
* Create a Player: POST http://localhost:5000/Players

* Query for Players: GET http://localhost:5000/Players

* Query by ID: GET http://localhost:5000/Players/{id}

* Query All Players: GET http://localhost:5000/Players

* Query All Players matching a given Last Name: GET http://localhost:5000/Players?lastName={lastName}

* Query All Players matching a given ID: GET http://localhost:5000/Players?id={id}

* Query for all Players on a Team: GET http://localhost:5000/Players?teamName={teamName}



## Built With
* .NET Core

* Entity Framework Core

## Author
* Henry Santacruz
