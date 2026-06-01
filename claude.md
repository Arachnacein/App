# Project: App

ASP.NET Core 8 Web API, Blazor .NET 7 UI. Domena: System zarządzania budżetem.

## Stack

- .NET 7 dla UI i .NET8 dla WebAPI, C# 12
- EF Core 8.0.8
- MediatR 12.4.6 (CQRS)
- FluentValidation
- MudBlazor 7.15.0
- xUnit 2.5.3 + FluentAssertions 7 + Moq 4.20
- Keycloak 1.0.18
- Ocelot 23.4.0

## Struktura

- `/src/ApiGateway` — Dockerfile, Program.cs, ocelot_config.json

- `/src/BudgetManager/Controllers` — kontrolery
- `/src/BudgetManager/Data` — EF Core (`ApplicationDbContext`, konfiguracje), integracje
- `/src/BudgetManager/Dto` — klasy DTO
- `/src/BudgetManager/Exceptions` — klasy wyjątków
- `/src/BudgetManager/Features  — CQRS (`Features/<Moduł>/<Command/Query>/<NazwaFeature>`), behaviors, walidatory
- `/src/BudgetManager/Installers` — klasy inicjalizacyjne
- `/src/BudgetManager/Mappers` — mapowanie
- `/src/BudgetManager/Middlewares` — exception handling
- `/src/BudgetManager/Migrations` — migracje db
- `/src/BudgetManager/Models` — encje, enumy
- `/src/BudgetManager/Repositories` — repozytoria, operacje na DB
- `/src/BudgetManager/Services` — serwisy, warstwa pomiędzy repozytorium a kontrolerem lub handlerem
- `/src/BudgetManager` — Dockerfile

- `/src/IdentityManager/Controllers` — kontrolery
- `/src/IdentityManager/Exceptions` — klasy wyjątków
- `/src/IdentityManager/Middlewares` — exception handling
- `/src/IdentityManager/Models` — encje, enumy
- `/src/IdentityManager/Services` — serwisy, warstwa pomiędzy repozytorium a kontrolerem lub handlerem
- `/src/IdentityManager` - Dockerfile 

- `/src/UI/Components` — dialogi, elementy interfejsu użytkownika
- `/src/UI/Extensions` — metody rozszerzeniowe
- `/src/UI/Models` — modele widokowe
- `/src/UI/Pages` — strony 
- `/src/UI/Resources` — internacjonalizacja, pliki językowe
- `/src/UI/Services` — serwis sesji użytkownika
- `/src/UI/Shared` — struktura strony
- `/src/UI` — Dockerfile, Program.cs, GlobalInfoClass.cs

- `/tests/UnitTests` — testy warstw mapperów + middlewares + respozytoriów + serwisów

Feature = folder `/Features/<Moduł>/(Commands|Queries)/<NazwaFeature>/` z plikami: `<NazwaFeature>Command.cs` (zawiera Command oraz Handler) lub `<NazwaFeature>Query.cs` (zawiera Query oraz Handler), `<Moduł>Controller.cs`.

## Komendy

- Build: `dotnet build`
- Build docker-compose: `docker-compose build`
- Run docker-compose: `docker-compose up`
- Testy jednostkowe: `dotnet test tests/UnitTests`

## Konwencje

- **Nazewnictwo**: `Save<Moduł>Command` + `Save<Moduł>CommandHandler` + `<Moduł>Controller.cs`, `Delete<Moduł>Command` + `Delete<Moduł>CommandHandler`, `Update<Moduł>Command` + `Update<Moduł>CommandHandler`, `Retrieve<Moduł>Query` + `Retrieve<Moduł>QueryHandler`
- **Testy**: `<NazwaMetody>_When<warunek>_Should<oczekiwanie>`
- **Async**: wszystkie publiczne metody zwracające I/O są async, z `CancellationToken` jako ostatni parametr
- **Read-only queries w EF Core**: ZAWSZE `AsNoTracking()`

## Używamy

- **CQRS** - za pomocą MediatR
- **Czystej architektury** 
- **mikroserwisów** 
- **Bazy danych SQL**

## NIE używamy

- **NIE** AutoMapper — mapowanie ręczne w mapperach
- **NIE** zmieniaj publicznego API bez uzgodnienia

## Git

- Branche: `feature/<short-desc>`, `bugfix/<short-desc>`
- Commity: conventional commits — `<type>(<scope>): <message>`
- NIE commituj do `main` bezpośrednio — zawsze PR

## WAŻNE

- Dla zmian dotykających **>3 plików**: używaj Plan Mode i pokaż plan do zatwierdzenia
- Po każdej serii zmian: uruchom `dotnet build` i odpowiednie testy — pokaż wynik
- Nigdy nie commituj sekretów (connection strings, API keys) — sprawdź diff przed commitem
- Przed zmianami w migracjach EF Core: pokaż diff wygenerowanej migracji do review