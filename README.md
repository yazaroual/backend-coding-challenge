# backend-coding-challenge

Backend coding challenge for Mimo.

The requirements are explained [here](/doc/backend-challenge-req.md)

While doing this challenge my goals were to :
1. Solve the challenge by correctly implementing the requirements
2. Provide a sens of how I would design the whole api if it was real work. It means DI, Erro handling, logs and unit tests.
3. Implement the API as if it will be used by a client. It means providing enough details on swagger.
4. Not going too far. This work will allow to open the discussions on various design subjects.

I understand that the position you need to fill involves working on CI/CD. I could have added some github actions to Build, Test on branches and mock a deployment when merged on main but that would probably not add more value to this demo. Still, I added a basic Build & Test on all pushes. I am whiling to discuss how I would have done it for a real project.

I hope you will enjoy the read !

## Progress :
- [X] Database
- [X] API
- [X] Tests
- [X] Documentation (API & Database)

## Start the project

1. Clone repo
2. Apply migrations `dotnet ef database update`
3. Run the project

A user was hard coded on the database. You can use User Id `1` for the endpoints.

## Database

Find below the database diagramm :
![Database diagram](/doc/db-diag.png)

## Additional features

* An error handling middleware was implemented to prevent unhandled errors from bubling to the client
* Unit tests are done in-memory

## Remarks

I was not sure if the endpoint to share lessons progress was expecting a list or a unique lesson. In theory we can only finish one lesson at once but if there is some kind of offline mode we could change the endpoint to receive all completed lessons when the user is back online.

Thanks !