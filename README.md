Last change was Jan 1st 2022
Code surfaced April 12th 2024

As part of my internship, I built (with the help of a senior developer) a concept for a facial recognition kiosk using Azure’s API services. It had a simple workflow where a user would sign in with their face, and if unrecognized it would prompt the user to register an account to train their face against. If recognized a text-to-speech message would play to welcome the user. Each login attempt using face ID trained and improved the API’s recognition of that user.

This app was built using Blazor WASM for the front end and for the back end, I used .NET Web API, Entity Framework, and SQL Server. For authentication, I used JWT tokens and utilized the Microsoft Identity package.

During this, I focused on implementing SOLID principles, the main one being Single Responsibility. One way I addressed this was by using MediatR to separate my business logic into microservices, that way my REST API controllers that use the microservices were decoupled. Another way was by ensuring the front-end sections of the app were consolidated into components using Blazor. Dependency Inversion was also another principle I worked on by using dependency injection (which was done by using .NET’s built-in package), which makes the codebase much cleaner.
