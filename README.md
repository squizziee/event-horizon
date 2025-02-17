<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgments">Acknowledgments</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project
This is apprenticeship application project for Modsen Inc. Server code is alright, client is a bit laggy and buggy, but still usable.

### Built With

This piece of software was built with basic set of tools for optimal workflow and experience consistency.

Server technologies:

* C# 12
* ASP.NET Core 8
* Entity Framework 8 with NpgSQL
* PostgreSQL
* FluentValidation, AutoMapper
* Swagger (Swashbuckle)

Client technologies:

* ReactJS 19
* Axios 1.7.9
* Swiper, Toastify, Material UI

<!-- GETTING STARTED -->
## Getting Started
### Prerequisites

* Docker (docker-compose)

### Installation

Installing the project is really straightforward:

1. Clone repository:
   
   ```sh
   git clone https://github.com/squizziee/event-horizon.git
   ```
2. Run Docker Compose
   ```sh
   cd event-horizon
   docker-compose up -d
3. Wait for completion
4. Ensure all containers are running. If not, launch idle containers

<!-- USAGE EXAMPLES -->
## Usage

### Option 1: use Swagger
After Docker container initialization is done, Swagger client will be available at `https://localhost:8081/swagger/index.html`. To test project API, follow these steps:
* Navigate to Dev and make `api/dev/admin` request to create administrator account. You will need it to mutate data through the API.
* Log into administrator account (`api/auth/login`).
  * Default email: `admin@horizon.com`
  * Default password: `admin`
  * Credentials can be changed in `appsettings.json` of `EventHorizon.API` project (should be done before admin creation)
* (Optional) To fill database with randomized data, make `api/dev/seed` request.
* Use endpoints, have fun :)

### Option 2: use React Client
After Docker container initialization is done, React client will be available at `http://localhost:3000`. Make sure to wait for a fat minute 
when opening client for the first time as it takes its time to load. To test project API. Admin creation and seeding can be done only through Swagger/Postman/Insomnia.
Data mutation can still be done through React client with administaror credentials at `http://localhost:3000/admin`


<!-- CONTACT -->
## Contact

Telegram: [@theshad0wrealm](https://t.me/theshad0wrealm)

Project Link: https://github.com/squizziee/event-horizon

<!-- ACKNOWLEDGMENTS -->
## Acknowledgments

* [ASP.NET Core Documentation](https://learn.microsoft.com/en-us/aspnet/core/?view=aspnetcore-8.0)
* [FluentValidation Documentation](https://docs.fluentvalidation.net/en/latest/)
* [AutoMapper Documentation](https://docs.automapper.org/en/stable/)
* [Material UI](https://mui.com/material-ui/)
* [React-select](https://react-select.com/home)
* [React-dropzone](https://react-dropzone.js.org/)
