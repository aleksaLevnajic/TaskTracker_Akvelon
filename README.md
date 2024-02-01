# TaskTracker
Task Tracker is Web API application that allows users to create Projects and specific Tasks, manipulate with them if they are authorized(list, delete and update specific tasks or projects).
Technologies used for crateing Task Tracker app are: ASP.NET Core 8, Entity Framework Core 8(Code-first approach), MS SQL Server 2018.
Three-level project architecture is used, along side Repository Design Pattern(with UnitOfWork implemented).
Reposiotry pattern is used to isolate Data Layer from the rest of the application and by doing that we achive more testable and modular project. We use Unit of work to avoid to many injections of different repositories in our controllers and with that making our code more readable.
