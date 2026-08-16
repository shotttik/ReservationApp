# 🏨 ReservationApp

> A modern reservation management backend built with **.NET 8**, following **Clean Architecture** principles and designed with scalability, maintainability, and separation of concerns in mind.

---

## ✨ Tech Stack

| Technology | Purpose |
|---|---|
| ⚙️ **.NET 8 / ASP.NET Core** | REST API |
| 🗃️ **Entity Framework Core** | Data access & ORM |
| 🗄️ **SQL Server** | Primary database |
| ⚡ **Redis** | Caching |
| 🍃 **MongoDB** | API & application logging |
| 🐇 **RabbitMQ** | Asynchronous messaging |
| 🔄 **Background Workers** | Background task processing |
| 📋 **Serilog** | Structured logging |
| 🐳 **Docker & Docker Compose** | Containerized environment |

---

## 🏗️ Architecture

The backend follows **Clean Architecture**, keeping business logic independent from infrastructure concerns.

```text
┌─────────────────────────────┐
│         RS.CBC.API          │
│       🌐 REST API           │
└──────────────┬──────────────┘
               │
┌──────────────▼──────────────┐
│     RS.CBC.Application      │
│   🧠 Business Logic         │
└──────────────┬──────────────┘
               │
┌──────────────▼──────────────┐
│       RS.CBC.Domain         │
│   📦 Core Domain            │
└──────────────┬──────────────┘
               │
┌──────────────▼──────────────┐
│    RS.CBC.Infrastructure    │
│ 🔧 DB • Cache • Messaging   │
└─────────────────────────────┘
```

### 🔌 Infrastructure

```text
                    ReservationApp
                          │
        ┌─────────────────┼─────────────────┐
        │                 │                 │
   🗄️ SQL Server       ⚡ Redis          🍃 MongoDB
     Database           Cache              Logs
        │
        └──────────── 🐇 RabbitMQ
                         │
                  🔄 Background Workers
```

---

## 🚀 Getting Started

### 📋 Prerequisites

Make sure you have installed:

- 🐳 [Docker Desktop](https://www.docker.com/products/docker-desktop)
- 📁 [Git](https://git-scm.com/)

### 1️⃣ Clone the Repository

```bash
git clone https://github.com/shotttik/ReservationApp.git
cd ReservationApp
```

### 2️⃣ Start the Application

```bash
docker-compose up --build
```

Docker Compose will start the API together with all required infrastructure services.

### 3️⃣ Access the API

| Service | URL |
|---|---|
| 🌐 API | http://localhost:8080 |
| 📖 Swagger | http://localhost:8080/swagger |

---

## 🧩 Included Services

The Docker Compose environment includes:

- 🖥️ **ReservationApp API** — .NET 8 Web API
- 🗄️ **SQL Server** — primary relational database with automatic migrations
- 🍃 **MongoDB** — application and API logging
- ⚡ **Redis** — caching layer
- 🐇 **RabbitMQ** — asynchronous message processing
- 🔄 **Background Workers** — processing queued tasks

---

## 🧪 API Testing

A Postman collection is included in the repository:

```text
📄 Reservation APP.postman_collection.json
```

Import the collection into **Postman** to test the available API endpoints.

---

## 🛑 Stopping the Application

To stop and remove the running containers:

```bash
docker-compose down
```

---

## 📁 Project Structure

```text
ReservationApp/
│
├── 📂 RS.CBC.API/
├── 📂 RS.CBC.Application/
├── 📂 RS.CBC.Domain/
├── 📂 RS.CBC.Infrastructure/
│
├── 🐳 docker-compose.yml
└── 🧪 Reservation APP.postman_collection.json
```

---

## 🎯 Project Goals

ReservationApp was built as a practical implementation of **modern .NET backend engineering**, combining:

**Clean Architecture** • **REST APIs** • **EF Core** • **SQL Server**  
**Caching** • **Asynchronous Messaging** • **Background Processing**  
**Structured Logging** • **Docker** • **Scalable Infrastructure**

> 💡 The goal is to demonstrate how a modern backend can be structured beyond basic CRUD operations while keeping the system clean, maintainable, and ready to evolve.

---

### ⭐ If you find this project useful, feel free to star the repository!