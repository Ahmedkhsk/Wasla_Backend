<div align="center">

<img src="https://img.shields.io/badge/ASP.NET_Core-9.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white"/>
<img src="https://img.shields.io/badge/Entity_Framework_Core-Latest-512BD4?style=for-the-badge&logo=dotnet&logoColor=white"/>
<img src="https://img.shields.io/badge/SQL_Server-Latest-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white"/>
<img src="https://img.shields.io/badge/SignalR-Realtime-00BFFF?style=for-the-badge"/>
<img src="https://img.shields.io/badge/Firebase-FCM-FFCA28?style=for-the-badge&logo=firebase&logoColor=black"/>
<img src="https://img.shields.io/badge/Hangfire-Background_Jobs-2C3E50?style=for-the-badge"/>

# 🌐 Wasla Backend

**A full-featured community service platform connecting residents with local service providers.**  
Built with ASP.NET Core 9, following clean layered architecture with real-time capabilities.

</div>

---

## 📋 Table of Contents

- [Overview](#-overview)
- [Features](#-features)
- [Tech Stack](#️-tech-stack)
- [Architecture](#️-architecture)
- [Project Structure](#-project-structure)
- [Modules](#-modules)
- [Real-Time Hubs](#-real-time-hubs)
- [Getting Started](#-getting-started)
- [Environment Configuration](#-environment-configuration)
- [Testing](#-testing)

---

## 🧭 Overview

**Wasla** is a multi-service backend platform targeting the Saudi Arabian market, designed to connect residents with a wide range of local service providers — from doctors and gyms to restaurants, drivers, and technicians. It supports real-time communication, background job processing, AI-powered content moderation, payment integration, and a social feed.

---

## ✨ Features

| Category | Feature |
|---|---|
| 🔐 **Auth** | JWT Authentication, Role-Based Authorization, Refresh Tokens, Email Verification |
| 👥 **Users** | Residents, Doctors, Drivers, Technicians, Gyms, Restaurants, Admins, SuperAdmins |
| 💬 **Chat** | Real-time messaging via SignalR with soft-delete, unread counts, media messages |
| 🚗 **Rides** | Live ride tracking, dispatch jobs, fare estimation, ride lifecycle management |
| 🍽️ **Restaurant** | Full ordering system: menu, cart, checkout, reservations, order tracking |
| 🏋️ **Gym** | Package management, QR-code check-in, booking with status tracking |
| 🩺 **Doctor** | Service scheduling, time slot management, appointment booking |
| 🔧 **Technician** | Specialized booking system with availability management |
| ⭐ **Reviews** | ML-based toxicity filtering using Microsoft.ML |
| 📢 **Social** | Posts, comments, reactions, reports, hide/unhide feed content |
| 🔔 **Notifications** | In-app notifications + Firebase Cloud Messaging push notifications |
| 💳 **Payments** | Paymob integration with strategy pattern (Cash / Online) |
| ⏱️ **Background Jobs** | Hangfire-powered scheduled and recurring tasks |
| 🌍 **Localization** | Arabic/English multilingual responses |
| 🛡️ **Rate Limiting** | Custom middleware-based rate limiting per client |

---

## 🛠️ Tech Stack

| Technology | Purpose |
|---|---|
| **ASP.NET Core 9** | Web API framework |
| **Entity Framework Core** | ORM & database migrations |
| **SQL Server** | Primary relational database |
| **SignalR** | Real-time WebSocket communication |
| **Hangfire** | Background job scheduling |
| **AutoMapper** | DTO ↔ Entity mapping |
| **Firebase (FCM)** | Push notifications |
| **Microsoft.ML** | ML-based toxicity classification |
| **Paymob** | Payment gateway integration |
| **ASP.NET Core Identity** | User management & role system |

---

## 🏗️ Architecture

```
┌──────────────────────────────────────────────────────────┐
│                        Controllers                        │
│  (Admin, Auth, Chat, Doctor, Driver, Gym, Restaurant...) │
└──────────────────────┬───────────────────────────────────┘
                       │
┌──────────────────────▼───────────────────────────────────┐
│                        Services                           │
│           (Business logic, validation, mapping)           │
└──────────────────────┬───────────────────────────────────┘
                       │
┌──────────────────────▼───────────────────────────────────┐
│                      Repositories                         │
│           (Data access, EF Core queries)                  │
└──────────────────────┬───────────────────────────────────┘
                       │
┌──────────────────────▼───────────────────────────────────┐
│                    EF Core / SQL Server                   │
└──────────────────────────────────────────────────────────┘
```

Cross-cutting concerns are handled via:
- **Middlewares**: Exception handling, rate limiting
- **Helpers**: Localization, pagination, file operations, token management
- **Factories**: User creation, payment strategy resolution
- **Strategies**: Payment processing (Cash / Paymob)

---

## 📁 Project Structure

```
Wasla_Backend/
├── Controllers/
│   ├── Admin/            # Admin & SuperAdmin controllers
│   ├── Authentication/   # Account & Roles
│   ├── Chat/             # Chat & messaging
│   ├── Doctor/           # Booking, services, profile
│   ├── Driver/           # Driver profile, ride management
│   ├── General/          # Banners, notifications, reviews, favourites
│   ├── Gym/              # Gym profile, packages, bookings
│   ├── Restaurant/       # Menu, cart, orders, reservations
│   ├── Social/           # Posts, comments, reactions
│   ├── Technician/       # Technician profile & bookings
│   ├── Resident/         # Resident profile
│   ├── UserEvent/        # User event tracking
│   ├── PaymentController.cs
│   └── HangfireController.cs
│
├── Services/
│   ├── Interfaces/       # Service contracts
│   └── Implementation/   # Business logic per domain
│
├── Repositories/
│   ├── Interfaces/       # Repository contracts
│   └── Implementation/   # EF Core data access per domain
│
├── Models/               # EF Core entity models
├── DTOs/                 # Request/response data transfer objects
├── Mappings/             # AutoMapper profiles
├── Hubs/                 # SignalR hubs
├── Middlewares/          # Custom middleware
├── Helpers/              # Utilities, extensions, configurations
├── Factories/            # Object creation patterns
├── Strategies/           # Strategy pattern implementations
├── Enums/                # Application enumerations
├── Exceptions/           # Custom exception types
└── Data/
    ├── Context.cs        # EF Core DbContext
    └── Migrations/       # Database migrations
```

---

## 📦 Modules

### 🩺 Doctor Module
- Complete profile registration & management
- Service scheduling with working days and time slots
- Appointment booking with status tracking (`Pending`, `Confirmed`, `Cancelled`, `Completed`)
- Real-time booking notifications via `BookingHub`

### 🏋️ Gym Module
- Multi-package subscription system
- QR code generation & validation for check-ins
- Gym booking with status lifecycle
- Multilingual gym profile support (AR/EN)

### 🍽️ Restaurant Module
- Menu management: categories, items, availability toggles
- Full cart & checkout flow
- Order tracking with status updates via `OrderHub`
- Table reservation system

### 🚗 Driver / Ride Module
- Driver profile with vehicle details and status
- Ride request → dispatch → accept → complete lifecycle
- Live location tracking via `RideHub`
- Fare estimation using `GeoHelper`

### 🔧 Technician Module
- Specialization-based booking system
- Booking request management with status transitions

### 💬 Chat Module
- One-to-one messaging with media support
- Soft-delete (per-user hide) with deletion timestamps
- Unread message counts respecting visibility state
- Typing indicators via `ChatHub`

### 📢 Social Module
- Posts with media attachments
- Nested comments, reactions (Like, Love, etc.)
- Content reporting & hide/unhide
- ML toxicity filtering on user-generated content

---

## 📡 Real-Time Hubs

| Hub | Path | Purpose |
|---|---|---|
| `ChatHub` | `/chatHub` | Real-time messaging, typing indicators, read receipts |
| `BookingHub` | `/bookingHub` | Booking status updates for doctors/services |
| `RideHub` | `/rideHub` | Ride lifecycle events, driver location tracking |
| `ServiceHub` | `/serviceHub` | Generic service provider notifications |
| `ReviewHub` | `/reviewHub` | Real-time review notifications |
| `OrderHub` | `/orderHub` | Restaurant order status updates |

All hubs use **JWT Bearer authentication** with WebSocket query parameter support.

---

## 🚀 Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- SQL Server (local or remote)
- Firebase project (for push notifications)

### Setup

```bash
# Clone the repository
git clone https://github.com/your-org/Wasla_Backend.git
cd Wasla_Backend

# Restore dependencies
dotnet restore

# Apply database migrations
dotnet ef database update

# Run the application
dotnet run --project Wasla_Backend
```

The API will be available at `https://localhost:5001` with Swagger UI at `/swagger`.

---

## 🔐 Environment Configuration

Configure the following via **User Secrets** (development) or environment variables (production):

```json
{
  "JWT": {
    "SecretKey": "your-secret-key",
    "Issuer": "WaslaBackend",
    "Audience": "WaslaClients",
    "ExpiryMinutes": 60
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=WaslaDb;..."
  },
  "Paymob": {
    "ApiKey": "your-paymob-api-key",
    "IntegrationId": "...",
    "IframeId": "..."
  },
  "Firebase": {
    "ProjectId": "your-project-id",
    "CredentialPath": "path/to/firebase-credentials.json"
  },
  "EmailSettings": {
    "SmtpServer": "smtp.example.com",
    "Port": 587,
    "SenderEmail": "no-reply@wasla.com",
    "SenderPassword": "..."
  }
}
```

---

## 🧪 Testing

The solution includes a test project (`Wasla_Backend.Tests`) covering core service logic:

```bash
dotnet test
```

**Covered services:**
- `CartService`, `OrderService`, `ReservationService`
- `MenuItemService`, `MenuItemCategoryService`
- `RestaurantService`, `RestaurantCategoryService`
- `RideService`, `UserService`

Tests use mock factories (`MockFactory`, `HubMockHelper`) for SignalR hub simulation.

---

## 👥 User Roles

| Role | Access |
|---|---|
| `resident` | Core consumer — booking, ordering, rides, chat, social |
| `doctor` | Manage services, view & respond to bookings |
| `driver` | Manage ride availability, accept & complete rides |
| `technician` | Manage bookings, update service status |
| `gym` | Manage packages, validate QR check-ins |
| `restaurant` | Manage menu, orders, and reservations |
| `admin` | Platform moderation, user management |
| `superadmin` | Full access including admin management |

---

## 📌 Status

> ✅ Core modules implemented and stable  
> 🚧 Continuous improvements and new features in progress
