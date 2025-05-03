# 🏨 ReservationApp Docker Setup

Simple Docker setup for running the ReservationApp locally with all its dependencies!

## 🚀 Quick Start

### Prerequisites
- 🐳 [Docker](https://www.docker.com/products/docker-desktop)
- 📁 [Git](https://git-scm.com/)

### Get Started in 3 Steps

#### 1️⃣ Clone the repo
```bash
git clone https://github.com/shotttik/ReservationApp.git
cd ReservationApp
```

#### 2️⃣ Start everything up
```bash
docker-compose up --build
```

#### 3️⃣ Access the API
- API is available at: http://localhost:8080
- Swagger docs: http://localhost:8080/swagger

## ℹ️ What's Included

- 🖥️ .NET API 
- 🗄️ SQL Server (with auto migrations!)
- 📊 MongoDB
- ⚡ Redis

## 🛑 Stopping

```bash
docker-compose down
```

## 🧪 Testing

Import `Reservation APP.postman_collection.json` into Postman to test all endpoints.

---

Happy coding! 🎉