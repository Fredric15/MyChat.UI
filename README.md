# MyChat

MyChat är en webbaserad chattapplikation byggd med **ASP.NET Core Razor Pages**. Projektet är strukturerat enligt en trelagersarkitektur för att hålla en tydlig separation mellan användargränssnitt, affärslogik och databashantering.

## 🏗️ Projektstruktur

Lösningen är uppdelad i tre huvudsakliga projekt:

* **MyChat.UI (User Interface)**
    Presentationslagret. Detta är en ASP.NET Core Razor Pages-applikation som innehåller alla webbsidor (t.ex. inloggning, registrering, användardetaljer och meddelandesidor) samt frontend-resurser (CSS, JS, Bootstrap).
* **MyChat.BLL (Business Logic Layer)**
    Affärslogiklagret. Här hanteras applikationens centrala logik. Innehåller tjänster (som `MessageService`) och DTO:er (Data Transfer Objects som `MessageDto`) för att skicka data smidigt mellan lagren.
* **MyChat.DAL (Data Access Layer)**
    Dataåtkomstlagret. Ansvarar för all kommunikation med databasen med hjälp av **Entity Framework Core**. Projektet använder två separata databaskontexter:
    * `AuthDbContext`: Hanterar användarkonton och autentisering.
    * `AppDbContext`: Hanterar applikationsdata, till exempel chattmeddelanden (`MessageModel`).

## ✨ Funktioner

* Användarregistrering och inloggning.
* Säker sessionshantering och utloggning.
* Skicka och läsa chattmeddelanden.

## 🛠️ Teknisk stack

* **Språk:** C#
* **Ramverk:** .NET / ASP.NET Core Razor Pages
* **ORM:** Entity Framework Core
* **Frontend:** HTML, CSS, JavaScript, Bootstrap

## 🚀 Komma igång

För att köra projektet lokalt på din maskin behöver du ha [.NET SDK](https://dotnet.microsoft.com/download) installerat.

1. **Klona projektet** till din lokala maskin.
2. **Öppna terminalen** i projektets rotmapp (eller öppna lösningen `MyChat.UI.slnx` i Visual Studio / Rider).
3. **Uppdatera databasen** genom att köra Entity Framework-migreringarna för båda databaskontexterna. Om du använder terminalen:
   ```bash
   dotnet ef database update --project MyChat.DAL --startup-project MyChat.UI --context AuthDbContext
   dotnet ef database update --project MyChat.DAL --startup-project MyChat.UI --context AppDbContext
