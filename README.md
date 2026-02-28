# Food Ordering Helper Tool

Simple web application to manage food orders at festivals and concerts.

## Quickstart

Start from the repository root (requires Docker & Docker Compose):

```powershell
docker-compose up --build
```

After startup the backend API is available at http://localhost:5000 by default.

Note: Ticket IDs are returned as 6-digit strings (e.g. "000001").

## API Overview

Base URL: `http://localhost:5000`
Base function:
### /tickets
- Route for handling all things related to ticket

### /login
- Route for handling all things related to login

### /balance
- Route for handling all things related to balance

Check readme in backend folder for all api routes and more detailed info.

### Notes & further information
f
- The backend implementation is in the `Backend/` folder. See `Backend/README.md` for additional details and instructions.
- Database: MySQL (see `docker-compose.yml` and `mysql-init/init.sql`). The backend waits for the database to become available and logs startup errors to the console if needed.

If you want to add more endpoints (e.g. top-up balance, record purchases or delete tickets), modify `Backend/Program.cs`.

---


