# ShelfLife Project Instructions for GitHub Copilot

## Tech Stack
- **Backend:** .NET 10 Web API (C#)
- **Frontend:** React 19 + Vite
- **Database:** PostgreSQL (via OrbStack/Docker)
- **ORM:** Entity Framework Core (Code-First)
- **Styling:** Tailwind CSS v4 (CSS-first configuration)

## Backend Guidelines (C#)
- Use **file-scoped namespaces** (e.g., `namespace ShelfLife.API;`).
- Use **Async/Await** for all I/O operations (Database, File System).
- Use **Records** for DTOs (Data Transfer Objects).
- Controller endpoints must return `ActionResult<T>`.
- **Strictly avoid** raw SQL; use LINQ and EF Core methods.
- Use Dependency Injection for all services and repositories.

## Frontend Guidelines (React)
- Use **Functional Components** with Hooks only.
- Use **Tailwind v4** classes for styling. Do NOT create separate CSS files.
- Avoid `useEffect` for data fetching; prefer custom hooks or React Query patterns if possible.
- Ensure all components are strictly typed (if using TypeScript) or Prop-Types.
- Use standard `fetch` with `async/await` for API calls.

## Project Structure
- **Monorepo:**
  - `/shelflife-api`: .NET Backend
  - `/shelflife-client`: React Frontend