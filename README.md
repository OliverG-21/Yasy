# Yasy - Kurs- und Kundenverwaltung

Yasy ist ein internes Verwaltungssystem fuer Kurse, Workshops und Kunden. Es richtet sich an Mitarbeiter, die Kursanmeldungen, Anwesenheitslisten und Guthabenkarten verwalten.

## Funktionsumfang

### Kundenverwaltung
- Anlegen, Bearbeiten und Loeschen von Kunden mit Kontaktdaten, Adresse und Profilbild
- Geburtstagsuebersicht auf dem Dashboard
- Master-Detail-Ansicht im Apple-Contacts-Stil (mobilfaehig)

### Kursverwaltung
- **Regulaere Kurse**: Woechentlich wiederkehrend an festem Wochentag, Uhrzeit und Dauer
- **Workshops**: Einmalige Veranstaltungen mit Datum, Start-/Endzeit und Festpreis
- Preismodelle fuer Kurse: DropIn, 5er-Karte, 10er-Karte

### Kalender
- Globaler Kalender mit Monats-, Wochen- und Tagesansicht (Heron.MudCalendar)
- Kurstermine werden automatisch aus den Kursdaten generiert
- Einzelne Termine koennen abgesagt und wieder aktiviert werden
- Absage eines Termins macht automatisch alle Anwesenheits- und Kartenbuchungen rueckgaengig

### Kursanmeldungen
- Kunden koennen zu beliebig vielen Kursen und Workshops angemeldet werden
- Anmeldedatum wird gespeichert
- Abmeldung als Soft-Delete mit Historie (Anmelde- und Abmeldedatum sichtbar)
- Sichtbarkeit in beide Richtungen: Am Kunden die Kurse, am Kurs die Teilnehmer

### Anwesenheitsliste
- Pro Kurs und Quartal eine tabellarische Uebersicht
- Spalten = Kurstermine des Quartals, Zeilen = angemeldete Kunden
- Klick auf eine Zelle toggled: leer -> anwesend -> abgesagt -> leer
- Abgesagte Kurstermine sind gesperrt und ausgegraut
- Restguthaben pro Kunde wird in der Tabelle angezeigt
- Anwesenheit bucht automatisch von der Guthabenkarte ab
- Roter Haken bei Buchung ohne verfuegbares Guthaben

### Guthabenkarten
- 5er- und 10er-Karten pro Kunde, kursuebergreifend gueltig
- Kaufhistorie mit Preis und Datum
- Fortschrittsbalken: X von Y Besuchen verbraucht
- Aufklappbarer Nutzungsverlauf pro Karte (Kurs + Datum)
- Karte wird automatisch als aufgebraucht markiert wenn alle Besuche verbraucht sind
- Manuelle Abbuchung mit Kursauswahl (nur angemeldete Kurse) und Terminauswahl (tatsaechliche Kurstermine)
- Bei Kartenkauf werden unbezahlte Anwesenheiten automatisch nachgebucht

### Geteilte Guthabenkarten
- Eine Karte kann explizit mit anderen Kunden geteilt werden
- Mitnutzer werden pro Karte verwaltet (hinzufuegen/entfernen)
- Beim Abbuchen werden eigene Karten bevorzugt, dann geteilte Karten
- Nutzungsverlauf zeigt wer welchen Besuch genutzt hat
- Bei Kartenkauf des Besitzers werden auch unbezahlte Termine der Mitnutzer automatisch nachgebucht

### Dashboard
- Geburtstage der naechsten 7 Tage mit Altersanzeige
- Guthabenkarten mit 2 oder weniger Restbesuchen (farblich nach Dringlichkeit)
- Kurse der aktuellen Woche mit Teilnehmerzahl und Absagen
- Direktlinks zu Kunden und Anwesenheitslisten

### Weitere Features
- Dark Mode mit automatischer Erkennung der OS-Einstellung
- Mobile-optimiertes Layout (responsive Master-Detail)
- Deutsche Benutzeroberflaeche

## Technologie-Stack

| Komponente | Technologie |
|------------|-------------|
| Frontend | Blazor Server (.NET 10), MudBlazor |
| Backend | ASP.NET Core, Entity Framework Core |
| Datenbank | PostgreSQL 17 |
| Kalender | Heron.MudCalendar |
| Deployment | Docker Compose (App + PostgreSQL + Nginx) |
| Auth | Microsoft Azure Entra ID (OAuth) |

## Architektur

Clean Architecture Light mit vier Projekten:

```
Yasy.Server  ->  Yasy.Infrastructure  ->  Yasy.Core  ->  Yasy.Shared
     |                                        ^
     +----------------------------------------+
```

| Projekt | Aufgabe |
|---------|---------|
| **Yasy.Server** | Blazor Server Host, Razor-Komponenten, Seiten, DI-Konfiguration |
| **Yasy.Core** | Domain-Entities, Repository-Interfaces. Keine Infrastruktur-Abhaengigkeiten. |
| **Yasy.Infrastructure** | EF Core DbContext, Repository-Implementierungen, PostgreSQL via Npgsql |
| **Yasy.Shared** | DTOs und Enums, geteilt zwischen allen Projekten |

## Voraussetzungen

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/) und Docker Compose (fuer Produktion)
- PostgreSQL 17 (lokal oder via Docker)

## Installation und Betrieb

### Lokale Entwicklung

1. **Repository klonen**

   ```bash
   git clone <repository-url>
   cd Yasy
   ```

2. **PostgreSQL starten** (via Docker)

   ```bash
   cd docker
   docker compose up db -d
   cd ..
   ```

   Alternativ eine lokale PostgreSQL-Instanz mit den Zugangsdaten aus `appsettings.Development.json` einrichten.

3. **Anwendung starten**

   ```bash
   dotnet run --project src/Yasy.Server
   ```

   Die Datenbank wird beim ersten Start automatisch migriert. Die App ist unter `http://localhost:5006` erreichbar.

4. **Hot Reload** (optional, kann auf macOS Probleme mit FileSystemWatcher verursachen)

   ```bash
   dotnet watch --project src/Yasy.Server
   ```

### Produktion (Docker Compose)

1. **Umgebungsvariablen konfigurieren**

   ```bash
   cd docker
   cp .env.example .env
   ```

   In `.env` das PostgreSQL-Passwort setzen:

   ```
   POSTGRES_PASSWORD=ein-sicheres-passwort
   ```

2. **Stack starten**

   ```bash
   docker compose up --build -d
   ```

   Der Stack besteht aus drei Services:
   - **yasy**: Blazor Server App (intern Port 8080)
   - **db**: PostgreSQL 17 mit persistentem Volume
   - **nginx**: Reverse Proxy mit WebSocket-Support fuer SignalR (Port 80/443)

3. **Zugriff**

   Die App ist ueber den Nginx-Reverse-Proxy unter `http://<server-ip>` erreichbar.

### Datenbank-Migrationen

Migrationen werden beim App-Start automatisch angewendet (`Database.MigrateAsync()`).

Neue Migration erstellen:

```bash
dotnet ef migrations add <MigrationName> \
  --project src/Yasy.Infrastructure \
  --startup-project src/Yasy.Server
```

### Build und Tests

```bash
# Gesamte Solution bauen
dotnet build Yasy.slnx

# Tests ausfuehren
dotnet test

# Einzelnen Test ausfuehren
dotnet test --filter "FullyQualifiedName~TestClassName.TestMethodName"
```

## Konfiguration

### Verbindungszeichenfolge

Die Datenbankverbindung wird ueber die Umgebungsvariable oder `appsettings.json` konfiguriert:

```
ConnectionStrings__DefaultConnection=Host=localhost;Database=yasy;Username=yasy;Password=yasy
```

### Profilbilder

Profilbilder werden im Dateisystem gespeichert. Der Pfad ist konfigurierbar ueber `DataPath` in `appsettings.json` (Standard: `data/` im Projektverzeichnis).

## Projektstruktur

```
Yasy/
  src/
    Yasy.Server/              # Blazor Server App
      Components/
        Layout/               # MainLayout, NavMenu
        Pages/
          Calendar/            # Kalenderseite
          Courses/             # Kursverwaltung, Anwesenheitsliste
          Customers/           # Kundenverwaltung
      Models/                  # ViewModels (CalendarEvent)
      Services/                # CalendarService
    Yasy.Core/                 # Domain-Schicht
      Entities/                # Customer, Course, CourseCard, etc.
      Interfaces/              # Repository-Interfaces
    Yasy.Infrastructure/       # Datenzugriff
      Data/
        Configurations/        # EF Core Entity-Konfigurationen
        Migrations/            # EF Core Migrationen
        Repositories/          # Repository-Implementierungen
    Yasy.Shared/               # Geteilte Typen
      Enums/                   # CourseType, CardType, AttendanceStatus
  tests/
    Yasy.Tests/                # Unit Tests
  docker/
    docker-compose.yml         # Produktions-Stack
    Dockerfile                 # Multi-Stage Build
    nginx/                     # Nginx-Konfiguration
```

## Lizenz

Proprietary - Nur fuer den internen Gebrauch.
