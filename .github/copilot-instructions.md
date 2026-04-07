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
- **NEVER use AutoMapper or third-party mappers**. All mapping between Domain Entities and DTOs must be done via static `ToDto()` extension methods in `MappingExtensions` class to maintain high performance and compile-time safety.

## Data Architecture & Ownership
### Global Recipe/Creator Model
- **Recipes are owned by Users (Creators)**, not Households
- Recipes are **globally discoverable** when `IsPublic = true`
- Use `SavedRecipe` junction table for users saving recipes to personal cookbooks
- **Recipe Creator relationship:** `Recipe.CreatorId` → `User.Id` (Restrict delete)
- **Personal Cookbook:** Users save public recipes via `SavedRecipe` table

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