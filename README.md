# Callio – PRN232 Project

**Callio** là một ứng dụng mạng xã hội và nhắn tin thời gian thực được xây dựng bằng ASP.NET Core, sử dụng kiến trúc tách biệt giữa Web API và MVC Client.

---

## 📋 Mục lục

- [Giới thiệu](#giới-thiệu)
- [Tính năng](#tính-năng)
- [Kiến trúc hệ thống](#kiến-trúc-hệ-thống)
- [Công nghệ sử dụng](#công-nghệ-sử-dụng)
- [Cấu trúc dự án](#cấu-trúc-dự-án)
- [Yêu cầu hệ thống](#yêu-cầu-hệ-thống)
- [Hướng dẫn cài đặt](#hướng-dẫn-cài-đặt)
- [Cơ sở dữ liệu](#cơ-sở-dữ-liệu)
- [Thành viên nhóm](#thành-viên-nhóm)

---

## Giới thiệu

Callio là đồ án môn học PRN232 – Lập trình Web với ASP.NET Core. Dự án cung cấp nền tảng nhắn tin, gọi video, quản lý bạn bè và quản trị người dùng theo thời gian thực.

---

## Tính năng

### 👤 Người dùng (User)
- Đăng ký, đăng nhập, quên mật khẩu và đặt lại mật khẩu qua OTP
- Chỉnh sửa thông tin cá nhân (avatar, tên, trạng thái)
- Xem danh sách bạn bè, gửi và chấp nhận lời mời kết bạn
- Chặn / bỏ chặn người dùng
- Nhắn tin và gọi video thời gian thực với bạn bè
- Báo cáo / tố cáo người dùng vi phạm

### 🛡️ Quản trị viên (Admin)
- Quản lý danh sách người dùng (tìm kiếm, khoá tài khoản)
- Xem và xử lý các tố cáo (Accusations)
- Xem danh sách bạn bè và danh sách chặn của người dùng
- Xem nhật ký hệ thống (Logs)

---

## Kiến trúc hệ thống

```
┌─────────────────────────────────────────────┐
│          PRN232_Project_MVC (Frontend)       │
│   ASP.NET Core MVC + SignalR Client          │
│   Port: https://localhost:7180               │
└─────────────────┬───────────────────────────┘
                  │ HTTP / SignalR
┌─────────────────▼───────────────────────────┐
│          PRN232_Project_API (Backend)        │
│   ASP.NET Core Web API + SignalR Hub         │
│   Port: https://localhost:7098               │
└─────────────────┬───────────────────────────┘
                  │ Entity Framework Core
┌─────────────────▼───────────────────────────┐
│       SQL Server – Database: Callio_Test     │
└─────────────────────────────────────────────┘
```

---

## Công nghệ sử dụng

| Thành phần | Công nghệ |
|---|---|
| Backend API | ASP.NET Core Web API (.NET 8) |
| Frontend | ASP.NET Core MVC (.NET 8) |
| ORM | Entity Framework Core |
| Cơ sở dữ liệu | Microsoft SQL Server |
| Thời gian thực | SignalR (Chat & Video Call) |
| Mapping | AutoMapper |
| API Documentation | Swagger / OpenAPI |

---

## Cấu trúc dự án

```
PRN232-Project/
├── PRN232_Project/
│   ├── PRN232_Project.sln
│   ├── PRN232_Project_API/          # Web API Backend
│   │   ├── Controllers/
│   │   │   ├── AdminController.cs
│   │   │   ├── UsersController.cs
│   │   │   ├── FriendsController.cs
│   │   │   └── MessagesController.cs
│   │   ├── ChatHub.cs               # SignalR Hub
│   │   └── Program.cs
│   ├── PRN232_Project_MVC/          # MVC Frontend
│   │   ├── Controllers/
│   │   │   ├── HomeController.cs
│   │   │   ├── UserController.cs
│   │   │   └── AdminController.cs
│   │   ├── Views/
│   │   │   ├── User/                # Login, Chat, Friend, Block, Calling...
│   │   │   └── Admin/               # Users, Accusations, Logs...
│   │   └── Program.cs
│   ├── BusinessObjects/             # Entity models
│   ├── DataAccessObjects/           # DbContext & DAOs
│   ├── Repositories/                # Repository pattern
│   └── Services/                    # Business logic & DTOs
├── CREATE_DATABASE_CALLIO.sql       # Script tạo CSDL
└── README.md
```

---

## Yêu cầu hệ thống

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (hoặc SQL Server Express)
- Visual Studio 2022 hoặc Visual Studio Code

---

## Hướng dẫn cài đặt

### 1. Clone repository

```bash
git clone https://github.com/Yukibevip/PRN232-Project.git
cd PRN232-Project
```

### 2. Tạo cơ sở dữ liệu

Mở SQL Server Management Studio (SSMS) và thực thi file:

```
CREATE_DATABASE_CALLIO.sql
```

### 3. Cấu hình Connection String

Cập nhật chuỗi kết nối trong cả hai file `appsettings.json`:

- `PRN232_Project/PRN232_Project_API/appsettings.json`
- `PRN232_Project/PRN232_Project_MVC/appsettings.json`

```json
"ConnectionStrings": {
  "MyCallioDB": "Server=<your-server>;uid=<user>;pwd=<password>;database=Callio_Test;TrustServerCertificate=True;"
}
```

> ⚠️ **Lưu ý:** `TrustServerCertificate=True` chỉ nên dùng trong môi trường **phát triển (development)**. Trong môi trường production, hãy cấu hình chứng chỉ SSL hợp lệ và bỏ tuỳ chọn này.

### 4. Chạy ứng dụng

Mở solution `PRN232_Project.sln` bằng Visual Studio 2022, cấu hình **Multiple Startup Projects** cho cả `PRN232_Project_API` và `PRN232_Project_MVC`, sau đó nhấn **Run**.

Hoặc chạy từng project bằng CLI:

```bash
# Chạy API
cd PRN232_Project/PRN232_Project_API
dotnet run

# Chạy MVC (terminal khác)
cd PRN232_Project/PRN232_Project_MVC
dotnet run
```

### 5. Truy cập ứng dụng

| Dịch vụ | URL |
|---|---|
| MVC Frontend | https://localhost:7180 |
| Web API | https://localhost:7098 |
| Swagger UI | https://localhost:7098/swagger |

---

## Cơ sở dữ liệu

Database: **Callio_Test**

| Bảng | Mô tả |
|---|---|
| `Users` | Thông tin người dùng |
| `Messages` | Tin nhắn giữa người dùng |
| `FriendList` | Danh sách bạn bè |
| `FriendInvitation` | Lời mời kết bạn |
| `BlockList` | Danh sách chặn |
| `Accusations` | Tố cáo vi phạm |
| `Logs` | Nhật ký hệ thống |

---

## Thành viên nhóm

| Họ và tên | GitHub |
|---|---|
| Yukibevip | [@Yukibevip](https://github.com/Yukibevip) |