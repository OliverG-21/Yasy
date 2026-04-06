# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Yasy is a course management and customer administration system built with Blazor Server (.NET 10). It runs as a Docker container stack (Blazor Server + PostgreSQL + Nginx) on a Synology NAS. UI framework is MudBlazor.

**Target users:** Internal staff only (no public/customer-facing access).
**Auth:** OAuth via Microsoft Azure Entra ID (internal employees).
**Language:** German UI, English code.

## Build & Run

```bash
# Build entire solution
dotnet build Yasy.slnx

# Run the Blazor Server app locally (hot reload)
dotnet watch --project src/Yasy.Server

# Run tests
dotnet test

# Run a single test
dotnet test --filter "FullyQualifiedName~TestClassName.TestMethodName"

# Docker (production)
cd docker
cp .env.example .env   # set POSTGRES_PASSWORD
docker compose up --build
```

## Architecture

Clean Architecture Light — four projects with unidirectional dependencies:

```
Yasy.Server  →  Yasy.Infrastructure  →  Yasy.Core  →  Yasy.Shared
     ↓                                       ↑
     └───────────────────────────────────────┘
```

| Project | Purpose |
|---------|---------|
| **Yasy.Server** | Blazor Server host, Razor components, pages, DI setup. All UI lives here. |
| **Yasy.Core** | Domain entities (`Entities/`), interfaces (`Interfaces/`). No infrastructure dependencies. |
| **Yasy.Infrastructure** | EF Core DbContext (`Data/YasyDbContext.cs`), repository implementations, PostgreSQL via Npgsql. |
| **Yasy.Shared** | DTOs and enums shared across projects. Will also be used by the MAUI app in Stage 2. |

## Key Technical Decisions

- **Blazor Server interactivity** — all components render server-side via SignalR. No WASM.
- **MudBlazor** — all UI components use MudBlazor (no Bootstrap). Layout uses `MudLayout`/`MudDrawer`/`MudAppBar`.
- **EF Core + PostgreSQL** — `YasyDbContext` auto-discovers `IEntityTypeConfiguration` implementations from the Infrastructure assembly.
- **Docker Compose** — three services: `yasy` (app on port 8080), `db` (PostgreSQL 17), `nginx` (reverse proxy with WebSocket support for SignalR).

## Conventions

- Entities go in `Yasy.Core/Entities/`, interfaces in `Yasy.Core/Interfaces/`
- EF configurations go in `Yasy.Infrastructure/Data/` alongside the DbContext
- Blazor pages go in `Yasy.Server/Components/Pages/`, layouts in `Components/Layout/`
- Connection string key: `ConnectionStrings__DefaultConnection`
