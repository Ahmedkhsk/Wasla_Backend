# Wasla Backend 🚀

Backend for **Wasla** — a community service platform connecting residents with service providers (doctors, gyms, technicians, drivers, restaurants).

---

## ✨ Features

* 🔐 Authentication & Role Management (JWT + Identity)
* 📦 Multi-service system:

  * Doctors
  * Gyms
  * Technicians
  * Drivers
  * Restaurants
* 💬 Real-time chat using SignalR
* 🚗 Ride system with live tracking
* ⭐ Reviews with ML-based toxicity filtering
* 📢 Social feed (posts, comments, reactions)
* 🔔 Notifications (in-app + Firebase push)
* 💳 Payment integration (Paymob)
* ⏱️ Background jobs (Hangfire)

---

## 🛠️ Tech Stack

* ASP.NET Core 9
* Entity Framework Core (SQL Server)
* SignalR
* Hangfire
* Firebase (FCM)
* Microsoft.ML
* AutoMapper

---

## 🏗️ Architecture

Clean layered architecture:

Controllers → Services → Repositories → EF Core

---

## 📡 Real-Time

* ChatHub → messaging
* BookingHub → booking updates
* RideHub → ride lifecycle

---

## 🚀 Getting Started

```bash
dotnet restore
dotnet ef database update
dotnet run
```

---

## 🔐 Environment

Use **User Secrets** or environment variables for:

* JWT
* Paymob
* Firebase
* Email

---

## 📌 Status

✅ Core modules implemented
🚧 Continuous improvements & enhancements

