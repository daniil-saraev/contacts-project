# Overview
The purpose of the project was to create a scalable system, guided by Clean Architecture principles, that could support multiple clients.
Backend of the system consists of two web API services for users' identity management and contacts' data management respectively.
The core of the system consists of several libraries that define domain entites, exceptions and core interfaces.
For frontend I have created a web client and a desktop client. The project also includes unit tests.

# Core functionality
These are implemented use cases:
1. A user can create an account and can be authenticated.
2. A user can create, modify, delete and view his contacts.
    A contact entity has the following fields: ID, UserID, FirstName, MiddleName, LastName, PhoneNumber, Address, Description.
3. A user can use a web client, provided he/she is authenticated.
4. A user can use a desktop client, whether authenticated or not.
    If the user is authenticated, the desktop client works with a local filesystem and with API, providing synchronization.
    Otherwise, the desktop client only stores data locally, but still supports synchronization by tracking changes
    in case the user creates an acccount or signs in.

# Stack
1. .NET Core.
2. ASP.NET & Swagger for APIs
3. ASP.NET MVC & Razor pages for the web client.
4. WPF for the desktop client.
5. SQL Server & Entity Framework for persistence.
6. XUnit for unit testing.
7. Git for version control.

# Backend design
### Core contacts services
Interfaces for contacts services are located in Core.Contacts project. The core interface is IContactBookService. 
The project does not contain any implementation, because the main logic is implemented in Contacts.Common project, 
that Contacts.Api depends on. 
The API acts as a microservice and accepts requests defined in Core.Contacts project, handles authentication & all the CRUD operations.
A direct dependency on database is avoided with repository pattern.
Authentication process is based on JWT Tokens, wich are retrieved from Identity.Api service.

### Core identity services
Interfaces for identity services are located in Core.Identity project. The core interface is IIdentityService.
The project also does not include any implementation.
Here I decided to use Backends for Frontends pattern. The IIdentityService is intended to be used by any clients other than web clients.
It provides a simple way of authenticating with JWT tokens, which are used to access Contacts.Api service. However, 
in case of the web client it's much more convenient to leverage ASP.NET Cookie authentication functionality, but it still
needs to be able to issue JWT tokens. For that reason I put any shared functionality in Identity.Common library, that both Identity.Api and 
web client depend on. This functionality includes identity persistence and token generation.

### Core.Common
The Core.Common library mainly contains the contact entity, some base interfaces and abstract classes.

### API Gateway
The Api.Services.Gateway project provides classes, that act as wrappers around generated API clients and implement core interfaces.

# Frontend design
### Web client 
The Web.Authentication library depends on Identity.Common and implements authentication service used by Web.App.
Web.App is a simple project that mostly contains pages & views and configures core services, as IContactBookService implementation
it uses ContactBookService in Api.Services.Gateway library.

### Desktop client
Desktop.Main, which is an executable project, following MVVM pattern mainly consists of views, viewmodels and commands, and
depends on several libraries that hold the main functionality.

Desktop.Common contains interfaces and base abstract classes along with some default services implementaions like NavigationService
or JsonFileService.

Desktop.Authentication library implements the logic of authenticating a user and storing user's data, as IIdentityService implementation
uses IdentityService in Api.Services.Gateway library.

Desktop.Contacts library has the logic of managing user's cotnacts' data locally and syncing it with Cotnacts.Api service.
There are two patterns that I have used. The first pattern is State pattern: depending on whether a user is authenticated
or not a different IContactBookService implementation is used. NotAuthenticatedContactBookService is an implementation,
that works with local data only. AuthenticatedContactBookService extends NotAuthenticatedContactBookService through inheritance
and also acts as a decorator around default IContactBookService implementation in Api.Services.Gateway library, allowing
for synchronization functionality.
The second pattern I used was Unit of Work, a simple implementation that keeps a track of changes made by user.

# Running the project
For the web client to work it is necessary to run Contacts.Api, which will automatically create a database.
To use authentiation in the desktop client it is necessary to run Identity.Api, which will automatically create a databse
and also seed a default user. Default user's credentials can be found in  Core.Identity.Constants.AuthorizationConstants.
Default URLs for API services are configured in Core.Common.Constants.BaseUrls.