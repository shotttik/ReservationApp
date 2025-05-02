# 🐳 Reservation App Dockerization

This repository helps you **dockerize** a .NET 8 Reservation application with SQL Server, Redis, and MongoDB — ready for **local development and testing**.

<div align="center">
  
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)
![.NET 8](https://img.shields.io/badge/.NET%208-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)
![Redis](https://img.shields.io/badge/Redis-DC382D?style=for-the-badge&logo=redis&logoColor=white)
![MongoDB](https://img.shields.io/badge/MongoDB-47A248?style=for-the-badge&logo=mongodb&logoColor=white)

</div>

## 📋 Table of Contents

- [Repository Structure](#-repository-structure)
- [How to Set Up and Run](#-how-to-set-up-and-run)
  - [1. Clone the Project](#1-clone-the-project)
  - [2. Add the Docker Files](#2-add-the-docker-files)
  - [3. Build and Start Containers](#3-build-and-start-containers)
  - [4. Prepare SQL Server Database](#4-prepare-sql-server-database)
  - [5. Test the API Endpoints](#5-test-the-api-endpoints)
- [Services Running](#-services-running)
- [Important Notes](#-important-notes)
- [Author](#-author)

## 📂 Repository Structure

| File                                      | Purpose                                                  |
| :---------------------------------------- | :------------------------------------------------------- |
| `docker-compose.yml`                      | Defines and runs multi-container Docker applications.    |
| `Dockerfile`                              | Builds the Docker image for the Reservation API.         |
| `full-database.sql`                       | SQL script to create tables and initialize the database. |
| `Reservation APP.postman_collection.json` | Postman collection for testing API endpoints.            |

## 🚀 How to Set Up and Run

### 1. Clone the Project

```bash
git clone https://github.com/shotttik/ReservationApp.git
cd ReservationApp
```

### 2. Add the Docker Files

Ensure the following files are inside the cloned project directory:

- `docker-compose.yml`
- `Dockerfile`
- `full-database.sql`
- `Reservation APP.postman_collection.json`

If they are missing, manually move or upload them into the root directory of the project.

### 3. Build and Start the Containers

Run the following command inside the project directory:

```bash
docker-compose up --build
```

This will:

- Build the Reservation API Docker image.
- Pull and start:
  - SQL Server
  - Redis
  - MongoDB
- Start the Reservation API container.

> ℹ️ Wait a few seconds for the services to fully initialize.

### 4. Prepare SQL Server Database

After containers are running:

- Open a SQL Server management tool like **Azure Data Studio** or **SQL Server Management Studio (SSMS)**.
- Connect using:
  - **Server:** `localhost,1433`
  - **Username:** `sa`
  - **Password:** `Strong123!`
- Create a new database named:

```bash
$ docker cp full-database.sql sqlserver:/
$ docker exec -it sqlserver bash
$ /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P 'Strong123!' -C -Q "CREATE DATABASE Reservation; SELECT name FROM sys.databases;"
$ /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P 'Strong123!' -i ./full-database.sql -C
```

```sql
CREATE DATABASE Reservation;
```

- After that, **select the** `Reservation` database and execute the content of `full-database.sql` to create the required tables.
  Example:

```sql
USE Reservation;
-- Then execute the contents of full-database.sql
```

### 5. Test the API Endpoints

- Open **Postman**.
- Import the file: `Reservation APP.postman_collection.json`.
- Set the API base URL to:

```
http://localhost:5000
```

- Now you can send requests and test all Reservation App functionalities!

## 📦 Services Running

| Service         | Port  | Details                          |
| --------------- | ----- | -------------------------------- |
| SQL Server      | 1433  | SQL database for Reservation App |
| Redis           | 6379  | Caching server                   |
| MongoDB         | 27017 | Logs API calls (via Serilog)     |
| Reservation API | 5000  | The main .NET 8 web application  |

## 🔥 Important Notes

- The API connects **internally** to the services using **Docker service names** (not `localhost`).
  - Example: `Server=sqlserver;...`
- Ensure ports **1433**, **6379**, **27017**, and **5000** are not occupied.
- If SQL Server is slow to start, the API may throw transient connection errors — wait 20–30 seconds and retry.
- Default environment variables (DB, Redis, JWT settings) are configured inside `docker-compose.yml`.

## ✍️ Author

- **Shota Akhlouri**
- GitHub: [shotttik](https://github.com/shotttik)

---

<div align="center">
  <p><i>Happy Dockerizing! 🐋</i></p>
</div>
