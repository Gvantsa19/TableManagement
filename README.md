# TableManagement

This repository contains the Docker Compose setup for a web application that allows users to create and manage database tables, input column information, and view the data in a user-friendly interface. Additionally, it implements CRUD operations and secures the application with JWT-based authentication and authorization.

## Prerequisites

Before you begin, ensure you have the following installed on your machine:
 - Docker

## Setup Instructions
Clone the Repository

```python
git clone https://github.com/your-username/your-repo.git
cd your-repo
```

## Build and Start the Services
Use Docker Compose to build and start the backend services:

```python
docker-compose up -d --build
```

## This command will build the Docker images for service and start the containers.

Running the Application Locally
After running the docker-compose up --build command, the following services will be available:

Table API
```python
URL: https://localhost:8081/swagger/index.html
```
Stopping the Services
To stop and remove the containers, networks, and volumes defined in the docker-compose.yml file, run:
```python
docker-compose down
```

## Angular Docker Build
To build and run the Angular application, use the following command:
```python
docker run --rm -it -p 4201:4200 table-management-ui
```

## After running this command, the website will be displayed at:
```python
http://localhost:4201/?returnUrl=%2Fbackoffice%2Ftms
```

## Features
Create and manage database tables
Input and edit column information
View data in a user-friendly interface
Implement CRUD operations
Secure application with JWT-based authentication and authorization
Technologies Used
.NET Core
Angular
PostgreSQL
Docker
Contributing
Contributions are welcome! Please fork this repository and submit a pull request for any features, bug fixes, or enhancements.

Fork the repository.
Create a new branch (git checkout -b feature/your-feature-name).
Commit your changes (git commit -m 'Add some feature').
Push to the branch (git push origin feature/your-feature-name).
Open a pull request.
