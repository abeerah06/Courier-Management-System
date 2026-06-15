# Final Exam Prep — ADO.NET + Async (Advanced Programming)

Covers the 3 handouts: **Lec 15 (ADO.NET basics)**, **Lec 16 (DataReader CRUD)**,
**Async/Await Responsive UI**. Every topic is mapped to your CourierManager code.

Format reminder: ~10% MCQ, the rest theory + writing code. So you must be able to
*write* a CRUD method and *explain* why, not just recognize answers.

---

## PART A — ADO.NET fundamentals (Lec 15)

### What ADO.NET is
- The core data-access technology in .NET — classes that let C# **connect to a DB, send SQL,
  read results, update data**.
- It is **NOT an ORM** — you write the SQL yourself (Entity Framework Core is the ORM, and it
  sits *on top of* ADO.NET).
- It is **NOT tied to one database** — a "provider" model lets the same code talk to SQL Server,
  PostgreSQL, MySQL, SQLite, etc. Only class names change.

### Two models (likely MCQ)
| Model | How it works | Classes | Used today? |
|---|---|---|---|
| **Connected** | open → query → read → close | Connection, Command, DataReader, Parameter | ✅ yes (what we use) |
| **Disconnected** | load everything into memory, work offline | DataSet, DataTable, DataAdapter | ❌ legacy |

### Providers (MCQ bait)
- SQL Server → **`Microsoft.Data.SqlClient`** (new, use this) / `System.Data.SqlClient` (old, deprecated).
- PostgreSQL → Npgsql. MySQL → MySqlConnector. SQLite → Microsoft.Data.Sqlite.
- All connection classes inherit from the abstract **`DbConnection`** (same for `DbCommand`, `DbDataReader`).

### The 4 core classes (MEMORIZE — guaranteed question)
| Class | Role |
|---|---|
| **SqlConnection** | the open channel to SQL Server. Does nothing until you call `Open()`. |
| **SqlCommand** | holds the SQL statement (SELECT/INSERT/UPDATE/DELETE). |
| **SqlParameter** | passes values safely → prevents SQL injection. |
| **SqlDataReader** | reads SELECT results one row at a time, forward-only. |

### The connection string
```
Server=localhost; Database=CourierDB; Integrated Security=True; TrustServerCertificate=True;
```
- `Server` = the machine (`.` or `localhost` = your PC).
- `Database` = which DB.
- `Integrated Security=True` = use Windows login (no user/pass).
- `TrustServerCertificate=True` = ok for local dev, NOT production.
- `Encrypt` = True by default in the new provider (TLS).
- **In your project**: it's in `app.config` under name `CourierDB`, read via `ConfigurationManager`.

### `using` + connection pooling (common theory question)
- `using` auto-calls `Close()`/`Dispose()` even if an exception is thrown → no leaked connections.
- **Pooling**: `Close()` doesn't really close the TCP socket — it returns the connection to a pool.
  Next `Open()` grabs a ready one in microseconds (a fresh TCP connect takes 50–100 ms).
- Two rules: (1) use the *exact same* connection string everywhere, (2) close quickly via `using`.
- "Creating a `SqlConnection` object does nothing — the connection opens only on `Open()`." ← memorize.

### The 3 execute methods (MCQ + code)
| Method | Use for | Returns |
|---|---|---|
| `ExecuteReader()` | SELECT | a SqlDataReader (loop with `Read()`) |
| `ExecuteNonQuery()` | INSERT / UPDATE / DELETE | int = rows affected |
| `ExecuteScalar()` | single value (e.g. `SELECT COUNT(*)`) | one object |

---

## PART B — DataReader CRUD & patterns (Lec 16)

### SQL injection (almost certainly on the exam)
- **Cause**: building SQL by joining strings: `"... WHERE Name = '" + txt.Text + "'"`.
- **Attack**: user types `' OR 1=1 --` → returns every row. Or `'; DROP TABLE Products; --` → deletes the table.
- **Fix**: ALWAYS use parameters. Never concatenate user input. Ever.
```csharp
string sql = "SELECT * FROM Customer WHERE Name = @Name";
cmd.Parameters.AddWithValue("@Name", txtName.Text);  // SQL and data travel separately
```

### The 5-step CRUD skeleton (MEMORIZE — you will write this)
```csharp
using (SqlConnection conn = new SqlConnection(connStr))  // 1. connection
{
    conn.Open();                                          // 2. open
    SqlCommand cmd = new SqlCommand(sql, conn);           // 3. command
    cmd.Parameters.AddWithValue("@X", value);             // 4. parameters
    cmd.ExecuteNonQuery(); // or ExecuteReader()           // 5. execute
}                                                         //    auto-close
```

### Reading rows — the read loop
```csharp
using (SqlDataReader reader = cmd.ExecuteReader())
{
    while (reader.Read())          // true = moved to next row; false = no more rows
    {
        string name = reader["Name"].ToString();
        decimal cost = Convert.ToDecimal(reader["Cost"]);
        int id = Convert.ToInt32(reader["Id"]);
    }
}
```
- **Forward-only**: can't go back a row.
- Two ways to read a column: indexer `reader["Name"]` (returns `object`, convert it) — **this course uses this** —
  or typed getters `reader.GetString(ordinal)`.
- **NULL**: check `reader.IsDBNull(ordinal)` before reading a nullable column, or compare `reader["X"] == DBNull.Value`.

### Nested `using` order (theory)
- Reader **inside** connection. Reader disposes first, then connection.
- If the connection closes before the reader finishes → `InvalidOperationException`.

### ExecuteNonQuery return value
- Returns **rows affected**. `return cmd.ExecuteNonQuery() > 0;` → true if something changed,
  false if the `WHERE` matched nothing (e.g. bad Id on UPDATE/DELETE).

### MapRow helper = DRY (Don't Repeat Yourself)
- GetAll, GetById, Search all need to turn a row into an object → write it **once** in `MapRow`.
- **In your code**: `DBCustomerService.MapRow`, `DBShipmentService.MapRow`, etc.

### Dynamic SQL (Search with optional filters)
```csharp
string sql = "SELECT * FROM Shipment WHERE CustomerName LIKE @q";
if (statusFilter != "All") sql += " AND Status = @Status";   // add clause only if filtering
// ... and only add @Status as a parameter if it's in the SQL
```
- **Rule**: never add a parameter that isn't in the SQL string (and vice versa) → error.
- **In your code**: `DBShipmentService.Search(query, statusFilter)`.

### LIKE with % wildcards
- `Name LIKE '%lap%'` = "contains lap". Build with `"%" + text + "%"`.
- It's the SQL version of C#'s `.Contains()`.

### Constructor injection + readonly (theory)
```csharp
public class DBCustomerService(string conn) : ICustomerService
{
    private readonly string _conn = conn;  // readonly = can only be set in constructor
}
```
- The service doesn't hard-code the connection string — it's **passed in** by the caller (MainForm).
- `readonly` = can't be reassigned after construction.

### Storing enums (if asked)
- SQL Server has no enum type → store as string with `.ToString()`, read back with `Enum.Parse<T>()`.
  (Your project mostly uses plain strings like Status, which is the same idea.)

---

## PART C — Async / Await (Async handout)

### Why the UI freezes
- WinForms runs on **one UI thread** that does two jobs: run your code + keep the window alive.
- A slow DB call on that thread = window freezes = "Not Responding".

### Task<T>
- An async method returns **`Task<T>`** = a "promise"/receipt that the result will be ready later.
- It is NOT the result itself — you get the result by `await`-ing it.

### Three ways to handle a Task (MCQ)
| Option | Verdict |
|---|---|
| `.Result` | ❌ blocks the UI thread (freezes), can deadlock. Don't. |
| `.ContinueWith(...)` | ⚠️ works, legacy, messy. Good to know it existed. |
| `await` | ✅ always use this — releases the thread, auto-returns to UI thread, normal try/catch. |

### await — what it does (5 steps, likely theory question)
1. `GetAllAsync()` starts the query, returns a Task immediately.
2. `await` **suspends** the method and **returns the UI thread** to keep the window alive.
3. UI stays responsive.
4. When the DB responds, .NET **resumes** the method **on the UI thread**.
5. The result is now available; the next line runs.

### The two rules (MEMORIZE)
1. **`async void` is for event handlers ONLY.** Every other async method returns `Task` or `Task<T>`.
   ```csharp
   private async void btnLoad_Click(...) { }   // event handler → async void OK
   private async Task LoadDataAsync() { }        // your own method → async Task
   ```
2. **Disable the button while it runs** (so a double-click doesn't fire it twice).

### The async ADO.NET swaps (MCQ table)
| Synchronous | Async |
|---|---|
| `conn.Open()` | `await conn.OpenAsync()` |
| `cmd.ExecuteReader()` | `await cmd.ExecuteReaderAsync()` |
| `reader.Read()` | `await reader.ReadAsync()` |
| `cmd.ExecuteNonQuery()` | `await cmd.ExecuteNonQueryAsync()` |
| method returns `T` | returns `Task<T>` |
| void event handler | `async void` (events only) |
| other void method | `async Task` |

### UI thread safety
- Only the UI thread may touch controls. `await` resumes on the UI thread automatically, so you're safe.
- You only worry about this if you use `Task.Run` for CPU-heavy work (don't touch controls inside it).
- DB async methods (`OpenAsync` etc.) are **I/O-bound** — they don't need `Task.Run`.

### In your code
- `DBCustomerService.GetAllAsync` / `SearchAsync` use `OpenAsync`/`ExecuteReaderAsync`/`ReadAsync`.
- `CustomerView.LoadAsync` is `async void`, calls `await _service.GetAllAsync()`, and disables the
  toolbar buttons via `SetLoading(true)` while it runs — that's Rule 2 in action.

---

## PART D — Code you must be able to WRITE from memory

**1. A GetAll (SELECT) method:**
```csharp
public List<Customer> GetAll()
{
    List<Customer> list = new List<Customer>();
    using (SqlConnection conn = new SqlConnection(_conn))
    {
        conn.Open();
        SqlCommand cmd = new SqlCommand("SELECT * FROM Customer", conn);
        using (SqlDataReader reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                Customer c = new Customer();
                c.Id = Convert.ToInt32(reader["Id"]);
                c.Name = reader["Name"].ToString();
                list.Add(c);
            }
        }
    }
    return list;
}
```

**2. An Add (INSERT) method with parameters:**
```csharp
public void Add(Customer c)
{
    using (SqlConnection conn = new SqlConnection(_conn))
    {
        conn.Open();
        string sql = "INSERT INTO Customer (Name, Phone) VALUES (@Name, @Phone)";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Name", c.Name);
        cmd.Parameters.AddWithValue("@Phone", c.Phone);
        cmd.ExecuteNonQuery();
    }
}
```

**3. A Delete returning bool:**
```csharp
public bool Delete(int id)
{
    using (SqlConnection conn = new SqlConnection(_conn))
    {
        conn.Open();
        SqlCommand cmd = new SqlCommand("DELETE FROM Customer WHERE Id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);
        return cmd.ExecuteNonQuery() > 0;
    }
}
```

**4. The async version of GetAll:**
```csharp
public async Task<List<Customer>> GetAllAsync()
{
    List<Customer> list = new List<Customer>();
    using (SqlConnection conn = new SqlConnection(_conn))
    {
        await conn.OpenAsync();
        SqlCommand cmd = new SqlCommand("SELECT * FROM Customer", conn);
        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
                list.Add(MapRow(reader));
        }
    }
    return list;
}
```

---

## PART E — Rapid-fire Q&A (self-test)

1. What are the 4 core ADO.NET classes? → Connection, Command, Parameter, DataReader.
2. Which execute method for INSERT? → ExecuteNonQuery.
3. Which for SELECT COUNT(*)? → ExecuteScalar.
4. How do you prevent SQL injection? → SqlParameter (never concatenate input).
5. What does `using` guarantee? → Dispose/Close even on exception.
6. What is connection pooling? → reuse of connections from a pool instead of new TCP each time.
7. Does `new SqlConnection()` connect? → No — only `Open()` connects.
8. Is SqlDataReader forward-only? → Yes.
9. How to handle a NULL column? → `reader.IsDBNull(ordinal)` before reading.
10. What does ExecuteNonQuery return? → number of rows affected.
11. What type does an async method return? → `Task` or `Task<T>`.
12. When is `async void` OK? → event handlers only.
13. Why not use `.Result`? → blocks/freezes the UI thread, can deadlock.
14. What does `await` do? → suspends the method, frees the UI thread, resumes on UI thread when done.
15. Async version of `reader.Read()`? → `await reader.ReadAsync()`.
16. Why interfaces for services? → loose coupling; swap implementations without changing UI.
17. What is the DRY principle here? → MapRow written once, reused by GetAll/GetById/Search.
18. What is `Microsoft.Data.SqlClient`? → the modern SQL Server provider (replaces System.Data.SqlClient).

---

## PART F — THE ACTUAL IN-CLASS CODE (exam is based on these two files)

Per your CR: exam = the 3 handouts + the code of **DBProductService** and **DBCustomerService**
from the MiniForm-BackOffice project. Study the real code's specific style — examiners ask about
exactly these details.

### The models
```csharp
public class Product {
    public string Id { get; set; }           // string, NOT int
    public string Name { get; set; }
    public ProductCategoryEnum Category { get; set; }   // enum
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public ProductStatusEnum Status { get; set; }       // enum
}
public class Customer {
    public string Id, Name, Phone, Email, Address;       // all strings
    public Customer() { Id = "C-" + Guid.NewGuid().ToString("N").Substring(0,9); }
}
```

### KEY QUIRKS of the in-class code (likely exam targets — know these!)

1. **Explicit interface implementation.** Methods are written as
   `Product IProductService.Add(...)` and `void ICustomerService.Add(...)` — NOT `public Add(...)`.
   → Consequence: you can only call them through the **interface** type, not the concrete class.
   `DBCustomerService s = new(...); s.Add(...)` ❌ won't compile.
   `ICustomerService s = new DBCustomerService(...); s.Add(...)` ✅ works.

2. **IDs are strings made from a GUID**, not database IDENTITY ints:
   `product.Id = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();`  // e.g. "A3F9B2"
   - `Guid.NewGuid()` = globally unique id. `"N"` = 32 hex chars, no dashes. `Substring(0,6)` = first 6.

3. **Enums stored as strings.** SQL Server has no enum type, so:
   - Saving: `cmd.Parameters.AddWithValue("@Category", product.Category.ToString());`
   - Reading back: `Enum.TryParse<ProductCategoryEnum>(catStr, ignoreCase: true, out var cat) ? cat : ProductCategoryEnum.None;`
   - Note: in-class code uses **`Enum.TryParse`** (safe, returns false on bad value) — not `Enum.Parse`.

4. **DBProductService is PARTLY implemented** (very likely a "what does this do?" question):
   - `Add` ✅ (sync, returns the product or null based on rows affected)
   - `GetAll` ✅ (sync, reads all products)
   - `Search` ✅ (ASYNC, with optional category/status filters = dynamic SQL)
   - `Update`, `Delete`, `GetById` → **`throw new NotSupportedException();`** (not implemented!)

5. **DBCustomerService is FULLY implemented** and **all synchronous**:
   `Add, Delete, GetAll, GetById, Search, Update` — all use the basic open→command→execute pattern.
   - `Add`/`Update` use `ExecuteNonQuery()` and check `rows > 0`.
   - `Delete` pops a `MessageBox` (UI inside the service — works, but not best practice; a fair exam discussion point).
   - `GetAll` does NOT wrap the reader in a `using` (just `SqlDataReader reader = cmd.ExecuteReader();`) —
     the others don't either. Textbook says you *should* wrap readers in `using`.

6. **`InMemoryCustomerService` exists too** — same `ICustomerService` interface, stores a `List<Customer>`
   in memory instead of a DB. This is the whole point of the interface: swap DB ↔ in-memory without
   changing the UI. Likely "why is there an interface?" question.

### The Product.Search — the one async method (study this closely)
```csharp
async Task<List<Product>> IProductService.Search(string text,
        ProductCategoryEnum? category, ProductStatusEnum? status)
{
    List<Product> products = new List<Product>();
    using (SqlConnection conn = new SqlConnection(_connectionstring))
    {
        await conn.OpenAsync();
        string sql = "SELECT * FROM Product WHERE NAME LIKE @name";
        if (category != null) sql += " AND Category = @cat";    // dynamic SQL
        if (status   != null) sql += " AND Status = @status";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@name", "%" + text.Trim() + "%");   // LIKE %...%
        if (category != null) cmd.Parameters.AddWithValue("@cat", category.ToString());
        if (status   != null) cmd.Parameters.AddWithValue("@status", status.ToString());
        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                Product prod = new Product();
                prod.Id = reader["Id"].ToString();
                prod.Name = reader["Name"].ToString();
                prod.Category = Enum.TryParse<ProductCategoryEnum>(reader["Category"].ToString(), true, out var c) ? c : ProductCategoryEnum.None;
                prod.Price = Convert.ToDecimal(reader["Price"]);
                prod.Stock = Convert.ToInt32(reader["Stock"]);
                prod.Status = Enum.TryParse<ProductStatusEnum>(reader["Status"].ToString(), true, out var s) ? s : ProductStatusEnum.Active;
                products.Add(prod);
            }
        }
    }
    return products;
}
```
This single method ties together: **async** (OpenAsync/ExecuteReaderAsync/ReadAsync), **parameters**,
**dynamic SQL**, **LIKE wildcards**, **enum parsing**, and **row mapping**. If you understand this one
method fully, you understand most of the course.

### Most likely exam tasks on these files
- "Write the `Add` method for Customer" → the 5-step INSERT with parameters.
- "Write/complete `GetAll`" → connection + reader loop + map rows.
- "Convert this synchronous method to async" → swap to OpenAsync/ExecuteReaderAsync/ReadAsync, return Task<T>.
- "What's wrong / what does this return?" → e.g. Update/Delete throw NotSupportedException; rows>0 logic.
- "Why explicit interface implementation / why an interface?" → loose coupling, swap DB↔InMemory.
- "Spot the SQL injection risk and fix it" → concatenation → parameters.
```
```
