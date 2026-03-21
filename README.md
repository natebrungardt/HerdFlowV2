# HerdFlowV2

HerdFlowV2 is a local MVP application for tracking cows.

It includes:

- React + TypeScript frontend
- ASP.NET Core backend API
- PostgreSQL database (via Docker)

---

## Run Locally

### 1. Start the Database

From the project root:

````bash
docker compose up -d

cd backend/HerdFlow.Api
dotnet restore
dotnet ef database update
dotnet watch run

http://localhost:5062

http://localhost:5062/swagger

cd frontend
npm install
npm run dev

http://localhost:5173

frontend/.env

VITE_API_URL=http://localhost:5062/api/cow
# HerdFlowV2

HerdFlowV2 is a local MVP application for tracking cows.

It includes:
- React + TypeScript frontend
- ASP.NET Core backend API
- PostgreSQL database (via Docker)

---

## 🚀 Run Locally

### 1. Start the Database

From the project root:

```bash
docker compose up -d
````

---

### 2. Run the Backend

```bash
cd backend/HerdFlow.Api
dotnet restore
dotnet ef database update
dotnet watch run
```

#### Backend URL

```
http://localhost:5062
```

#### Swagger UI

```
http://localhost:5062/swagger
```

---

### 3. Run the Frontend

```bash
cd frontend
npm install
npm run dev
```

#### Frontend URL

```
http://localhost:5173
```

---

## ⚙️ Frontend Environment Variable

Create a file at:

```
frontend/.env
```

Add:

```env
VITE_API_URL=http://localhost:5062/api/cow
```

---

## ✅ Current Features

- View all cows
- Create a cow
- Prevent duplicate tag numbers
- Backend validation for required fields
- Clean error handling (409 for duplicate tags)

---

## 📦 Tech Stack

- React + TypeScript
- ASP.NET Core
- Entity Framework Core
- PostgreSQL
- Docker
