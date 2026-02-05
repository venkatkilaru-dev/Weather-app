# Weather-app
Calling a weather api and doing the necessary steps to show the weather

# Weather History Data Recorder

This project reads dates from a text file, fetches historical weather data from the Open‑Meteo API, stores the results locally, exposes backend endpoints, and displays the aggregated data in a Blazor UI.

This repository was created as part of a coding assignment and demonstrates:
- API integration
- Local JSON storage
- Backend API design
- Blazor Server UI with interactive components

---

### **Backend**
- Reads and parses 'dates.txt'
- Handles invalid dates safely
- Calls Open‑Meteo for each valid date
- Saves results under 'weather-data/' as JSON files
- Exposes:
  - 'GET /api/weather/{date}' → fetch or load a single date
  - 'GET /api/weather' → return all stored weather entries
  - 'GET /api/test-weather/{date}' → raw API test (no storage)
  - 'GET /api/test-dates' → parsed date list

### **Frontend (Blazor Server)**
- Loads aggregated weather data from '/api/weather'
- Displays results in a sortable table
- Sorting by:
  - Date
  - Max temperature
- Clicking a row shows detailed weather info
- Uses '@rendermode InteractiveServer' for interactivity

---

## 🛠️ Tech Stack

- .NET 9 / C#
- Blazor Server (Razor Components)
- HttpClientFactory
- Open‑Meteo API
- Local JSON file storage

---

## 📦 How to Run the Backend

### 1. Clone the repository
git clone https://github.com/venkatkilaru-dev/Weather-app.git
cd Weather-app

### 2. Restore and build
dotnet build

### 3. Run the Backend
dotnet run

### 4. Test the API
http://localhost:5291/api/weather
http://localhost:5291/api/weather/2021-06-30
http://localhost:5291/api/test-dates

### How to run the UI 

### 1. Start the app
dotnet run

### 2. Open the UI
http://localhost:5291/weather


You should see:
- A loading message
- A table of weather entries
- Sorting buttons
