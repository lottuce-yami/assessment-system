# Assessment System

A full-stack web application for creating, managing, and taking quizzes with detailed performance analytics.

*Project Context:  
This is a course project (курсова робота) developed as part of a university curriculum. The original project topic in Ukrainian is: "Розробка клієнт-серверного додатку для оцінювання рівня знань громадян щодо персоналізованого контенту" (Development of a client-server application for assessing knowledge levels of citizens regarding personalized content).*

## System Overview

Assessment System is a client-server application that allows users to take quizzes and administrators to manage quiz content. The system tracks user performance, provides topic-based analytics, and supports multiple languages.

## Architecture

The project consists of two main components:

- **Backend**: ASP.NET Core 8 REST API with SQLite database (this repository)
- **Frontend**: Vue 3 single-page application with Vuetify UI components ([lottuce-yami/assessment-system-client](https://github.com/lottuce-yami/assessment-system-client))

Communication between frontend and backend uses JWT-based authentication. The API exposes endpoints for user management, quiz operations, question handling, answer submission, and result analytics.

## Features

- **User Management**: Registration, login, and JWT token-based authentication
- **Quiz Management**: Create, edit, and organize quizzes with multiple questions
- **Quiz Taking**: Interactive quiz interface with immediate scoring
- **Result Tracking**: Historical records of completed quizzes with detailed answers
- **Topic Analytics**: Performance breakdown by topic area
- **Admin Dashboard**: Tools for quiz creation and management
- **Internationalization**: Multi-language support via Vue i18n

## Tech Stack

**Backend:**

- ASP.NET Core 8
- Entity Framework Core
- SQLite
- JWT Bearer Authentication

**Frontend:**

- Vue 3
- Vuetify 3
- Vue Router
- Vue i18n
- Axios
- Vite

**Development:**

- .NET 8
- Node.js v24
- Docker & Docker Compose

## Setup Instructions

### Backend Setup

1. Restore NuGet packages:

```bash
dotnet restore
```

2. Configure `appsettings.json` with a valid JWT key

3. Apply database migrations:

```bash
dotnet ef database update
```

4. Run the API:

```bash
dotnet run
```

### Frontend Setup

Instructions can be found in the frontend repository: [lottuce-yami/assessment-system-client](https://github.com/lottuce-yami/assessment-system-client)
