# Backend API

This folder contains a minimal .NET 7 API that exposes a single endpoint to return all ticket IDs from a MySQL database.

Run with Docker Compose from the repository root:

```bash
docker-compose up --build
```

After startup the API will be available at http://localhost:5000/tickets/ids

Hinweis: Die API gibt Ticket-IDs als 6-stellige Strings zurück (z. B. "000001", "000002").

**GET /tickets/ids** liefert jetzt zusätzlich `first_name`, `last_name` und `balance` für jedes Ticket.
