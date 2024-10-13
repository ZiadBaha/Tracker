
# Tracker - ASP.NET Web API Project

## Introduction 
In today's fast-paced and interconnected world, the distress of losing valuable possessions or experiencing the disappearance of a loved one is a challenge many face. Tracker is a web application designed to address these concerns, offering a simple and accessible solution to help locate missing children and lost belongings. By providing a user-friendly platform, Tracker brings a ray of hope to individuals and families in their moments of need, making it easier to find what's most important.
_________________________________________________________
## Project Overview
Tracker is a fully-featured ASP.NET Core Web API designed for managing accounts, items, people, comments, and complaints. The project employs layered architecture, follows Domain-Driven Design principles, and supports authentication with JWT tokens. It includes role-based access control, file uploads, and an efficient commenting system, making it highly scalable and maintainable.
_________________________________________________________
## Technologies Used
- **ASP.NET Core 6**: Web API framework for building RESTful services.
- **C# Programming Language**: The backend logic of the application is written in C#.
- **Entity Framework Core**: ORM used for database operations.
- **ASP.NET MVC**: Used for building the web application's architecture.
- **ASP.NET Identity**: Provides user authentication and authorization functionality.
- **SQL Server** : Database engine for data persistence.
- **Dependency Injection**: Built-in DI for loose coupling of modules.
- **JWT Authentication**: For secure authorization.
- **AutoMapper**: For object-to-object mapping.
- **Swagger**: For API documentation.
- **Unit Testing**: XUnit or NUnit for testing the core business logic.
- **RESTful APIs**: Implemented to enable communication between different components of the application.
- **Git**: Version control for codebase management.

_________________________________________________________

## Features
### AuthenAuthentication 
- **User Registration**: Seamlessly onboard new users with a user-friendly registration process.
- **Login**: Allow users to securely log in to their accounts using their credentials.
- **Email Confirmation**: Verify user email addresses to ensure authenticity and security.
- **Password Recovery**: Enable users to recover their passwords securely in case they forget them.
- **OTP Verification**: Enhance security by implementing OTP verification for sensitive operations.
- **Password Reset**: Allow users to reset their passwords securely when needed.
- **User Management**: Registration, login, password reset, and role management.

### Project
- **Item Management**: CRUD operations on items, with image uploads.
- **Person Management**: CRUD operations for person profiles, including image uploads.
- **Commenting System**: Allows users to comment on items and persons.
- **Complaint Management**: Users can submit, view, and manage complaints.
- **Role-Based Access Control**: Admin and user roles for authorization.
- **File Uploads**: Supports uploading images for users, items, and persons.
- **JWT Authentication**: Secure authentication with tokens.
- **Error Handling**: Centralized error handling with status codes.
_________________________________________________________


## Project Structure
- **Account.Apis**: Main API project, contains controllers and startup configurations.

- **Account.Core**: Contains business logic, models, DTOs, and enums.

- **Account.Reposatory**: Implements the repository pattern for data persistence.

- **Account.Services**: Provides service layer logic and abstractions

_________________________________________________________

## Controllers Overview
### 1. AccountController
- **Handles user account operations.**

POST /api/account/register: Registers a new user.

POST /api/account/login: Authenticates the user and returns a JWT token.

POST /api/account/forgetPassword: Initiates password recovery for the user.

POST /api/account/verifyOtp: Verifies the OTP sent to the user's email.

PUT /api/account/resetPassword: Resets the user’s password using a secure token.

GET /api/account/confirm-email: Confirms a user’s email with a token.

GET /api/account/getUserInfo/{userId}: Retrieves user information by ID.

- ** **

### 2. ItemCommentsController
- **Manages comments on items.**

POST /{userId}/items/{itemId}/comments: Adds a comment to a specific item.

PUT /{userId}/items/{itemId}/comments/{commentId}: Updates a specific comment on an item.

DELETE /{userId}/items/{itemId}/comments/{commentId}: Deletes a specific comment on an item.

GET /items/{itemId}/comments: Retrieves all comments associated with a specific item.

GET /{userId}/items/comments: Retrieves all comments made by a user.

- ** **

### 3. PersonCommentsController
- **Handles comments on person profiles.**

POST /{userId}/persons/{personId}/comments: Adds a comment to a specific person profile.

PUT /{userId}/persons/{personId}/comments/{commentId}: Updates a specific comment on a person profile.

DELETE /{userId}/persons/{personId}/comments/{commentId}: Deletes a comment from a person profile.

GET /persons/{personId}/comments: Retrieves all comments associated with a specific person.

GET /{userId}/persons/comments: Retrieves all comments made by a user.

- ** **

### 4. ComplainsController
- **Manages user complaints.**

GET /api/complains: Retrieves all complaints submitted by users.

POST /api/complains: Submits a new complaint.

PUT /api/complains/{id}: Updates an existing complaint by ID.

DELETE /api/complains/{id}: Deletes a complaint by ID.

GET /api/complains/{id}: Retrieves a specific complaint by ID.

- ** **

### 5. AdminController
- **Admin-only actions.**

GET /admin/users/count: Retrieves the total count of users in the system.

GET /admin/items/count: Retrieves the total count of items in the system.

GET /admin/persons/count: Retrieves the total count of persons in the system.

GET /admin/complaints/count: Retrieves the total count of complaints in the system.

GET /admin/complaints/search: Searches for complaints by email.

DELETE /admin/users/{userId}: Deletes a user by their ID.

GET /admin/users: Retrieves a list of all users.

GET /admin/users/search: Searches users by email

- ** **

### 6. ItemsController
- **Manages item operations.**

POST /{userId}/items: Adds a new item for a user.

PUT /{userId}/items/{itemId}: Updates an existing item for a user.

DELETE /{userId}/items/{itemId}: Deletes an item for a user.

GET /{userId}/items: Retrieves all items for a specific user.

GET /items: Retrieves all items in the system.

GET /items/{id}: Retrieves a specific item by ID.

GET /items/status/{itemStatus}: Retrieves items filtered by their status.

GET /items/search: Searches items by name or unique number.

- ** **

### 7. PersonsController
- **Manages person profiles.**

POST /{userId}/persons: Adds a new person profile for a user.

PUT /{userId}/persons/{personId}: Updates an existing person profile for a user.

DELETE /{userId}/persons/{personId}: Deletes a person profile for a user.

GET /{userId}/persons: Retrieves all persons for a specific user.

GET /persons: Retrieves all persons in the system.

GET /persons/{id}: Retrieves a specific person profile by ID.

GET /persons/status/{personStatus}: Retrieves persons filtered by their status.

GET /persons/search: Searches persons by name.

- ** **

### 8. UserController
- **Handles user-specific operations.**

PUT /users/{userId}: Updates a user’s information.

DELETE /users/{userId}: Deletes a user.

GET /users/search: Searches users by email.

- ** **

### ApiBaseController
A base controller that provides common functionality for all other controllers, such as handling errors and standard responses.

_________________________________________________________

## Getting Started

To get started with using or contributing to this project, follow these steps:

1. Clone the repository to your local machine.
2. Set up your development environment with .NET SDK and SQL Server.
3. Configure the database connection string in the application settings.
4. Build and run the application locally.
5. Explore the different authentication features and functionalities.


