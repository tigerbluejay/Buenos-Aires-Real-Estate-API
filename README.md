# Buenos-Aires-Real-Estate-API

The Buenos Aires Real Estate API is a RESTful web service built to manage, query, and serve real estate property data within the Buenos Aires region. Designed with scalability and clean architecture in mind, it provides a robust backend foundation suitable for integration with front-end applications or data-driven services. This project is ideal for developers looking to understand or contribute to modern API development using the latest .NET ecosystem tools.The Buenos Aires Real Estate API is a RESTful web service built to manage, query, and serve real estate property data within the Buenos Aires region. Designed with scalability and clean architecture in mind, it provides a robust backend foundation suitable for integration with front-end applications or data-driven services. This project is ideal for developers looking to understand or contribute to modern API development using the latest .NET ecosystem tools.

## Project Structure

The API Service Solution is comprised of four core projects: The API which houses core functionality, a Data Project which handles data access, a Models Project which contains the Models and DTOs for the Solution and a Utilities Project with ancilliary code to assist. 

### BuenosAiresRealEstate.API

#### Controllers

The API Project houses the two core sections of the Project. One is the Controllers, which are defined in different version folders. Version 2 of the ApartmentComplexController serves to show how you can work with in memory data. But the core controllers are the ApartmentComplexController and the ApartmentUnitController. Each apartment complex has units associated to it, and both can be retrieved (Get All and Get by Id), created, updated and deleted with the methods defined in these controllers. There is also a UsersController which implements part of the Login and Registration functionality.

#### Repositories

The Controllers work directory with the Repositories which are the second core leg of this project. The Repositories and RepositoryInterfaces folders contain the relevant classes to implement the Repository Pattern. In this case the ApartmentComplex and ApartmentUnit Repositories implement the update functionality and inherit from a base repository named Repository, which implements all the remaining CRUD operations.
There is also a UserRepository which implements Login and Registration functionality.

##### The Special Case of the UserRepository

More specifically, the Login method of the UserRepository retrieves the user from the database, checks that the password is valid, generates the JWT Token and retrieves the Security Key to encrypt that token. Then it generates a Token descriptor object which it uses to pass claims such as the username, role and sign in credentials (the byte version of the token - hashed). In the loginResponseDTO object it includes the Token.

The Register method of the UserRepository initializes an ApplicationUser object and creates the user with the UserManager object. This method also creates and assigns roles, retrieves the user from the db to return it, maps it to a UserDTO object and returns it to the Controller.

#### Other Elements

The API Project houses other important elements. Apart from the basic Program.cs which contains the pipeline and middleware configurations, appsettings.json and launchsettings.json, the Project houses the Images folder which contains the Images that correspond to the apartment complexes and units, and the Logs folders which contain the Serilog output file.

#### Packages and Dependencies

The remaining projects in the Solution are defined as dependencies of the API Project. In addition several packages are used in the API and worth mentioning: AutoMapper and AutoMapper Dependency injection, Serilog related packages, Swashbuckle for the Swagger UI and Authentication (JwtBarer) and JsonPatch packages and MVC Packages for Verioning and Json. The EFCore, EFCore SqlServer and EFCore Tool packages are also included, as well as the OpenApi package for AspNetCore.


### BuenosAiresRealEstate.API.Data

The Data Project has the ApplicationDbContext class which defines the models in their relationship to EF Core and has some seed data. It also defines an ApplicationUser class which extends the UserIdentity class to add a Name property. The Migrations folder also exists inside the data Project. The EF Core, EFCore SqlServer, and EFCore Tools packages are defined here as is the Identity package for EFCore.

### BuenosAiresRealEstate.API.Models

The Models Project houses the Models and DTOs used in the Solution

#### Models

Models are useful for mapping to the database and DTOs for sending and receiving data between Controllers and the Swagger UI (or any other application using the API). The Models include an ApartmentComplex and ApartmentUnit Model, as well as an APIResponse model which is created to unify the responses provided by actions defined in the controller. That is so that the different methods return a unified object. The APIResponse class has a StatusCode, IsSuccess, Errors and Result property.

There are two additional Models defined here. One is the Pagination model with a PageSize and PageNumber property. The Pagination class (Model) is created to add Pagination to the Response Header in the ApartmentComplexController GetApartmentComplexes action.

The last Model is the InMemoryDataStorageApartmentComplexes Model which works with Version 2 of the ApartmentComplexController. The model is a static class containing a static property of type List<ApartmentComplexDTO> which has dummy in memory data to return to the GetApartmentComplexes method in the Controller (version 2 Controller).

#### DTOs

There are two folders housing the DTOs. The regular "DTOs" folder has ApartmentComplexDTO and ApartmentUnitDTO which are used in Get, GetbyId and in part of the Create methods to return the DTO object to the Response. The Create and Update DTOs (ApartmentComplexCreateDTO, ApartmentUnitCreateDTO, ApartmentComplexUpdateDTO, ApartmentUnitUpdateDTO) are used as parameters in the Create and Update actions received in the requests received to create or update the DTOs.

In addition there are the IdentityDTOs, which include the LoginRequestDTO and LoginResponseDTOs, used to process the request and response for Login, the RegistrationRequestDTO to do likewise (the response for the Registration is simply a 200 Ok Response - no DTO needed). And the UserDTO used in the Login and Registration methods of the UserRepository to work with User data. The UserDTO is a field in the loginResponseDTO so it gets added in the Login method, and the UserDTO is the return type for the Register method in the UserRepository to communicate with the UserController.

### BuenosAiresRealEstate.API.Utilities

The Utilities project has two files. One is the StaticDetails file, and the Mapping file which contains the mappings relevant to AutoMapper to map the DTOs with the Models.

## In Closing

The API Solution is an API Service that exposes various method for Apartment Complexes and Apartment Unit and has implemented login and registration functionality. It can be consumed by other services or applications.
