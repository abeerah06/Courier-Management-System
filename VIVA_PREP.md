# CourierManager — Viva Preparation Guide

A delivery & shipment management desktop app (Windows Forms + ADO.NET + SQL Server).
Login: `admin` / `admin123`.

---

## 1. The 30-second project summary (memorize this)

> "It's a courier management system. There are two projects: a **core class library**
> that holds the data models and all the database logic, and a **Windows Forms** project
> for the UI. The UI never talks to the database directly — it goes through **service
> classes** that use **ADO.NET** to run SQL. There are four tables — Customer, Driver,
> Shipment, and Payment — and the app does full CRUD on them, with validation, search,
> a dashboard with charts, and role-based delete protection through foreign keys."

---

## 2. Architecture — how the layers fit together

```
  UI layer (CourierManager)              Core layer (CourierApp.Core)
  ─────────────────────────              ────────────────────────────
  frmLogin                               Models/      → Customer, Driver, Shipment, Payment
  MainForm  (sidebar nav)                Contracts/   → ICustomerService, IShipmentService...
  Views/    (one per table)              Services/    → DBCustomerService, DBShipmentService...
  Forms/    (add/edit dialogs)           Utilities/   → form-mode enums, ValidationResult
        │                                       ▲
        └──── calls interface methods ──────────┘
                                                │
                                          ADO.NET (Microsoft.Data.SqlClient)
                                                │
                                          SQL Server (CourierDB)
```

**Why two projects?** Separation of concerns. The core library knows nothing about
buttons or forms — it could be reused by a web app or console app. The UI depends on the
core, never the other way around.

**Data flow for "Add a customer":**
1. User fills `CustomerForm` and clicks Save.
2. The form builds a `Customer` object and calls `_service.Validate(customer)`.
3. If valid, it calls `_service.Add(customer)`.
4. `DBCustomerService.Add` opens a SQL connection and runs a parameterized INSERT.
5. The view reloads the grid via `GetAllAsync()`.

---

## 3. Key concepts you MUST be able to explain

### ADO.NET (the data access layer)
- We use `SqlConnection`, `SqlCommand`, and `SqlDataReader` from `Microsoft.Data.SqlClient`.
- Pattern in every method: open a connection (`using` so it auto-closes), create a command
  with SQL text, add parameters, execute, map the results into model objects.
- `ExecuteNonQuery()` → INSERT/UPDATE/DELETE (returns rows affected).
- `ExecuteReader()` → SELECT (returns rows to read one by one).
- `ExecuteScalar()` → a single value, e.g. `SELECT COUNT(*)`.

### Parameterized queries (VERY likely question)
- We never concatenate user input into SQL. We use `@parameters`:
  ```csharp
  cmd.Parameters.AddWithValue("@Name", c.Name);
  ```
- **Why?** Prevents **SQL injection** and handles quotes/special characters safely.

### `using` statement
- `using SqlConnection connection = new(_conn);` — guarantees the connection is **disposed**
  (closed and returned to the pool) even if an exception is thrown. Prevents connection leaks.

### Interfaces + dependency injection
- Each service has an interface (`ICustomerService`) and an implementation (`DBCustomerService`).
- The views accept the **interface** in their constructor, not the concrete class.
- **Why?** Loose coupling. We could swap `DBCustomerService` for a fake/in-memory version
  for testing without touching the UI. `MainForm` "injects" the services into each view.

### async / await
- `GetAllAsync` and `SearchAsync` use `OpenAsync` / `ExecuteReaderAsync` / `ReadAsync`.
- **Why async?** Database calls can be slow; running them asynchronously keeps the UI thread
  free so the window doesn't freeze ("Not Responding").
- `await` pauses the method until the DB returns, without blocking the UI.
- Two async methods per service (the list-load and the search).

### Validation
- `ValidationResult` collects a list of error messages and an `IsValid` flag.
- `Validate()` checks each field (required, length, regex for phone/email) and adds errors.
- The form shows all errors at once in a message box and stops the save if invalid.

### BindingSource + DataGridView
- We set `dgv.DataSource = bindingSource` and `bindingSource.DataSource = list`.
- `AutoGenerateColumns = false` means we define columns manually and bind each to a property
  via `DataPropertyName`.
- `bindingSource.Current` gives the currently selected row's object — used by Edit/Delete.

### Foreign keys & delete protection
- `Shipment.CustomerId` → `Customer.Id`, `Shipment.DriverId` → `Driver.Id`,
  `Payment.ShipmentId` → `Shipment.Id`.
- You can't delete a customer who has shipments — SQL throws, and we catch it and show a
  friendly message. This is **referential integrity**.

### Charts
- `System.Windows.Forms.DataVisualization.Charting` (WinForms.DataVisualization package).
- Bar chart = monthly shipment counts (grouped in SQL with `GROUP BY`).
- Pie chart = status breakdown.
- `IsXValueIndexed = true` makes each month sit at its own position (text X-values otherwise
  all stack at 0).

---

## 4. Likely viva questions & answers

**Q: Walk me through what happens when you click "Mark Delivered".**
A: It calls `_service.UpdateStatus(id, "Delivered")` which runs an UPDATE and also sets the
delivery date. Then it checks if a payment already exists for that shipment with
`GetByShipmentId`; if not, it auto-creates an Unpaid payment for the shipment cost. Then the
grid refreshes.

**Q: How do you prevent SQL injection?**
A: Parameterized queries — user input goes in as `@parameters`, never concatenated into the
SQL string.

**Q: What's the difference between `ExecuteReader`, `ExecuteNonQuery`, `ExecuteScalar`?**
A: Reader = read multiple rows (SELECT), NonQuery = no rows returned (INSERT/UPDATE/DELETE),
Scalar = a single value like a count.

**Q: Why interfaces instead of calling the DB class directly?**
A: Loose coupling and testability — the UI depends on a contract, not an implementation, so
the implementation can change or be mocked without changing the UI.

**Q: What does `using` do here?**
A: Ensures the SqlConnection is disposed/closed automatically, even on error.

**Q: Why are only some methods async?**
A: The ones that read lists from the DB (list load + search) are async to keep the UI
responsive. The small single-row operations are synchronous because they're fast.

**Q: How does the dashboard get its numbers?**
A: `DBDashboardService` runs aggregate SQL — `COUNT(*)` for the cards, `GROUP BY` month for
the bar chart, `GROUP BY Status` for the pie chart.

**Q: What happens if validation fails?**
A: `Validate()` returns a `ValidationResult` with `IsValid = false` and a list of messages;
the form shows them and does not save.

**Q: How is the navigation built?**
A: `MainForm` has a sidebar of buttons. Each button loads a `UserControl` "view" into the
content panel. Views are cached in a dictionary so they're not recreated every time.

**Q: Why DECIMAL for Cost and not FLOAT?**
A: Money should be exact; FLOAT has rounding errors. DECIMAL stores exact values.

**Q: What's the primary key / how are IDs generated?**
A: Each table has an `IDENTITY(1,1)` integer primary key — SQL Server auto-increments it.

---

## 5. "Explain your own code" — be ready to read these out loud

**A service method (DBCustomerService.Add):**
```csharp
public void Add(Customer c)
{
    using SqlConnection connection = new(_conn);   // open connection, auto-dispose
    connection.Open();
    string sql = @"INSERT INTO Customer (Name, Phone, Email, Address, City, CreatedAt)
                   VALUES (@Name, @Phone, @Email, @Address, @City, @CreatedAt)";
    SqlCommand cmd = new(sql, connection);
    BindParams(cmd, c);                            // adds the @parameters safely
    cmd.Parameters.AddWithValue("@CreatedAt", c.CreatedAt);
    cmd.ExecuteNonQuery();                          // runs the INSERT
}
```

**The async list load (view side):**
```csharp
private async void LoadAsync()
{
    SetLoading(true);                       // disable buttons + show "Loading..."
    var list = await _service.GetAllAsync();// non-blocking DB call
    _bindingSource.DataSource = list;       // bind results to the grid
    lblCount.Text = $"Total: {list.Count}";
    SetLoading(false);
}
```

**Validation:**
```csharp
if (string.IsNullOrWhiteSpace(c.Phone))
    result.AddError("Phone number is required.");
else if (!Regex.IsMatch(c.Phone.Trim(), @"^\d{11}$"))
    result.AddError("Phone must be exactly 11 digits.");
```

---

## 6. Rubric → where it lives in the code

| Rubric criterion | Where / how |
|---|---|
| Project Setup & Architecture | 2 projects, layered, `.sln` references core |
| Database & Connection | `app.config` connection string, `Database_Setup.sql`, 4 tables |
| Data Access Layer (ADO.NET) | `Services/DB*Service.cs` — SqlConnection/Command/Reader |
| UI (Navigation & CRUD) | `MainForm` sidebar, `Views/` grids, `Forms/` add-edit dialogs |
| Validation & UX | `ValidationResult`, regex checks, message boxes, status bar |
| Charting | `DashboardView` bar + pie charts |
| Code Quality | interfaces, DI, helper methods (`MapRow`, `BindParams`), 0 warnings |
| Bonus: search + filter | search boxes + status dropdown on Shipments |
| Bonus: dashboard | stats cards + 2 charts on one page |
| Bonus: async | `GetAllAsync` / `SearchAsync` (2 per service) |
| Bonus: status bar | bottom bar: user, section, timestamp |
| Bonus: column sorting | click column header → sorts grid |
| Bonus: loading indicator | `SetLoading` disables toolbar + shows "Loading..." |

---

## 7. Things the examiner might probe (be honest, don't bluff)

- **"What would you improve?"** → Could hash the login password / store users in DB instead
  of hard-coding; could add a repository layer; could page large result sets.
- **"Is the login secure?"** → No, it's a simple hard-coded check for the demo; in production
  you'd store hashed passwords in a Users table.
- **"What's `AddWithValue`'s downside?"** → It infers the SQL type, which can occasionally
  pick a non-ideal type; `Add` with an explicit `SqlDbType` is more precise. (Good to know.)
- If you don't know something, say "I'm not 100% sure, but I think..." — far better than
  making it up.

---

## 8. Final checklist before the viva
- [ ] Run the app once start-to-finish: login → each screen → add/edit/delete → mark delivered.
- [ ] Make sure SQL Server is running and `Database_Setup.sql` has been run.
- [ ] Be able to point to: a model, an interface, a service method, a view, a form.
- [ ] Be able to explain ADO.NET, parameterized queries, async, interfaces, foreign keys.
- [ ] Know your bonus features and where each one is.
