# Courier & Delivery Management System

A desktop application for managing a courier/delivery business — customers, shipments,
delivery drivers, and payments — built with C# Windows Forms and SQL Server using ADO.NET.

Developed as a semester project for Advanced Programming (COSC-5136).

---

## Features

- **Dashboard** with live stats (total / monthly / weekly / today's shipments) and charts
  (monthly shipments bar chart + shipment-status pie chart).
- **Customers** — full CRUD with field validation and live search.
- **Shipments** — full CRUD, auto-generated tracking numbers, status workflow
  (Pending → In Transit → Delivered / Cancelled), search + status filter.
- **Drivers** — manage delivery riders; drivers can be assigned to shipments.
- **Payments** — auto-generated when a shipment is delivered; mark Paid/Unpaid.
- **Login screen** to enter the system.
- Responsive UI — list loads and searches run **asynchronously** so the window never freezes.
- Sortable data grids (click a column header) and a status bar showing the current user/section.

---

## Tech Stack

- **C# / .NET** (Windows Forms)
- **ADO.NET** with `Microsoft.Data.SqlClient` (connected model)
- **SQL Server** for storage
- **WinForms.DataVisualization** for the dashboard charts

---

## Architecture

The solution is split into two projects for separation of concerns:

| Project | Responsibility |
|---|---|
| **CourierApp.Core** | Models, service interfaces (contracts), and the ADO.NET data-access services. No UI. |
| **CourierManager** | Windows Forms UI — login, main shell with sidebar navigation, views, and add/edit dialogs. |

The UI depends only on the service **interfaces** (e.g. `ICustomerService`), never on the concrete
database classes. This keeps the layers loosely coupled and makes the data source swappable.

```
CourierApp.Core
 ├─ Models/        Customer, Shipment, Driver, Payment, ValidationResult
 ├─ Contracts/     ICustomerService, IShipmentService, IDriverService, IPaymentService, IDashboardService
 ├─ Services/      DBCustomerService, DBShipmentService, DBDriverService, DBPaymentService, DBDashboardService
 └─ Utilities/     form-mode enums

CourierManager
 ├─ Forms/         frmLogin, MainForm, CustomerForm, ShipmentForm, DriverForm
 ├─ Views/         DashboardView, CustomerView, ShipmentView, DriverView, PaymentView
 └─ Program.cs
```

---

## Database

The system uses four related tables:

- **Customer** — people who send shipments
- **Driver** — delivery riders
- **Shipment** — references a Customer and (optionally) a Driver
- **Payment** — references a Shipment

Run [`Database_Setup.sql`](Database_Setup.sql) in SQL Server Management Studio (or any SQL client)
to create the `CourierDB` database, the tables, and some sample data.

---

## Getting Started

### Prerequisites
- Visual Studio 2022 (or newer) with the **.NET desktop development** workload
- SQL Server (Express / Developer / LocalDB)

### Setup
1. Clone the repository:
   ```bash
   git clone https://github.com/abeerah06/Courier-Management-System.git
   ```
2. Open `CourierManager.sln` in Visual Studio.
3. Run `Database_Setup.sql` against your SQL Server instance to create `CourierDB`.
4. Open `CourierManager/app.config` and make sure the connection string matches your server:
   ```xml
   <connectionStrings>
     <add name="CourierDB"
          connectionString="Server=.;Database=CourierDB;Integrated Security=True;TrustServerCertificate=True;" />
   </connectionStrings>
   ```
   (Change `Server=.` to your instance name if needed, e.g. `(localdb)\MSSQLLocalDB`.)
5. Press **F5** to build and run.

### Login
```
Username: admin
Password: admin123
```

---

## Notes

- The login is a simple built-in check for demonstration purposes only.
- `TrustServerCertificate=True` is used for local development; it should not be used in production.
