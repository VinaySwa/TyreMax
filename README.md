This is a .NET 8 Web API to manage tyre companies, tyre models, and individual tyres. 
This project uses a three‑layer architecture (Controllers → Services → Repository/Unit‑of‑Work), Entity Framework Core with SQLite, and built‑in ILogger for structured logging. It provides CRUD operations and a search by dimensions endpoint.

1. CRUD for:
     a. Tyre Companies (/api/TyreCompanies)
	   b. Tyre Models (/api/TyreModels)
	   c. Tyres (/api/Tyres)

2. Search Tyres by Dimensions: filter tyres by width, profile, rim size
3. Exception handling with custom DataAccessException and appropriate HTTP status codes
4. Structured logging via Microsoft.Extensions.Logging 
5. Data seeding via SQL scripts or EF Core migrations
6. Unit tests with xUnit, EF Core InMemory provider, and Moq

Project Clone:
git clone - https://github.com/VinaySwa/TyreMax/
cd TyreSaleService

Open the project in Visual studio.
Build and Run the project.
The API will start with Swagger window.

API Endpoints:
Tyre Companies
	GET /api/TyreCompanies - list all companies with models (and tyres if available)
	GET /api/TyreCompanies/{id} - get specific company
	GET /api/TyreCompanies/search?width={w}&profile={p}&rimSize={r} - companies having tyres of given dimensions
	POST /api/TyreCompanies - create new company
	PUT  /api/TyreCompanies - update existing company
	DELETE /api/TyreCompanies/{id} - delete company

Tyre Models
	GET /api/TyreModels - list all models (with tyres)
	GET /api/TyreModels/{id} - get specific model
	POST /api/TyreModels - create new model
	PUT  /api/TyreModels - update model
	DELETE /api/TyreModels/{id} - delete model

Tyres
	GET /api/Tyres - list all tyres
	GET /api/Tyres/{id} - get specific tyre
	GET /api/Tyres/search?width={w}&profile={p}&rimSize={r} - filter tyres by dimensions
	POST /api/Tyres - create new tyre
	PUT  /api/Tyres - update tyre
	DELETE /api/Tyres/{id} - delete tyre
