# Iniciativas de Negocio con IA

Aplicación full-stack para el registro de **Iniciativas de Negocio** y su análisis asistido por **Inteligencia Artificial** (Ollama).

---

## Arquitectura del Proyecto

El sistema está construido con una arquitectura de microservicios:

```
                  ┌──────────────────────────────┐
                  │    Frontend (React + Vite)   │
                  │     http://localhost:5173    │
                  └──────────────┬───────────────┘
                                 │ Configurable vía VITE_API_URL
                         ┌───────┴───────┐
                         ▼               ▼
 ┌──────────────────────────────┐ ┌──────────────────────────────┐
 │   Backend Principal (FastAPI)│ │    Backend Secundario (.NET) │
 │    http://localhost:8000     │ │    http://localhost:5001     │
 └──────────────┬───────────────┘ └──────────────┬───────────────┘
                │                                │ (Delega llamadas de IA)
                │                                ▼
                │ ┌──────────────────────────────────────────────┐
                ├─►  Ollama AI (Local Host / Puerto 11434)       │
                │ └──────────────────────────────────────────────┘
                ▼
 ┌───────────────────────────────────────────────────────────────┐
 │ Base de Datos SQLite Compartida (initiatives.db)              │
 └───────────────────────────────────────────────────────────────┘
```

1. **`backend-fastapi` (Python / FastAPI)**: Servidor principal. Gestiona el CRUD de iniciativas, la persistencia en SQLite y la integración con la IA (Ollama) mediante un **Patrón Adapter** desacoplado.
2. **`backend-dotnet` (C# / .NET 8 Web API)**: Servidor secundario. Expone los mismos endpoints, consulta y escribe en la misma base de datos SQLite y delega las peticiones de análisis de IA a FastAPI vía HTTP.
3. **`frontend` (React + Vite)**: Interfaz de usuario SPA para crear, listar y evaluar iniciativas de negocio.

---

## Requisitos Previos

1. **Ollama corriendo en el Host**:
   - Instalar Ollama y descargar el modelo seleccionado:
     ```bash
     ollama pull llama3.1
     ```
   - Asegurarse de que Ollama acepte conexiones externas (especialmente para Docker):
     ```bash
     OLLAMA_HOST=0.0.0.0 ollama serve
     ```
2. **Docker & Docker Compose** (opción recomendada) **O** entornos locales (**Python 3.11+**, **.NET 8 SDK**, **Node.js 20+**).

---

## Clonar el Repositorio

```bash
git clone https://github.com/SergioRodrii/prueba-tecnica-ia-iniciativas.git
cd prueba-tecnica-ia-iniciativas
```

---

## Opción 1: Ejecución con Docker Compose (Recomendado)

1. **Crear archivo de variables de entorno**:
   ```bash
   cp .env.example .env
   ```

2. **Levantar todos los servicios**:
   ```bash
   docker compose up --build -d
   ```

3. **Acceso a las aplicaciones**:
   - **Frontend**: [http://localhost:5173](http://localhost:5173)
   - **Backend FastAPI**: [http://localhost:8000/docs](http://localhost:8000/docs)
   - **Backend .NET**: [http://localhost:5001/initiatives](http://localhost:5001/initiatives)

---

## Opción 2: Ejecución Local sin Docker

### 1. Backend FastAPI (Python)
```bash
cd backend-fastapi
python -m venv .venv
source .venv/bin/activate             # En Windows: .venv\Scripts\activate
pip install -r requirements.txt
uvicorn app.main:app --reload --port 8000
```

### 2. Backend .NET
```bash
# En Windows PowerShell:
cd backend-dotnet

$env:ConnectionStrings__InitiativesDb="Data Source=../backend-fastapi/initiatives.db;Cache=Shared"
$env:FASTAPI_BASE_URL="http://127.0.0.1:8000"
$env:FASTAPI_TIMEOUT_SECONDS="3600"

dotnet run --urls http://127.0.0.1:5001


# En Linux:
cd backend-dotnet

ConnectionStrings__InitiativesDb="Data Source=../backend-fastapi/initiatives.db;Cache=Shared" \
FASTAPI_BASE_URL="http://127.0.0.1:8000" \
FASTAPI_TIMEOUT_SECONDS=3600 \

dotnet run --urls http://127.0.0.1:5001

```

### 3. Frontend (React + Vite)
```bash
cd frontend
npm install
npm run dev
```

---

## Cambiar el Frontend entre FastAPI y .NET

El Frontend está diseñado para ser 100% intercambiable entre los dos backends mediante la variable `VITE_API_URL` en [.env](file:///.env):

- **Para consumir el backend de FastAPI**:
  ```env
  VITE_API_URL=http://localhost:8000
  ```
- **Para consumir el backend de .NET**:
  ```env
  VITE_API_URL=http://localhost:5001
  ```

> **Nota**: Tras modificar `.env`, en Docker ejecuta `docker compose up --build -d` para reconstruir la SPA; en modo local reinicia `npm run dev`.

---

## Variables de Entorno (`.env`)

Referencia de configuración extraída de [.env.example](file:///.env.example):

| Variable | Descripción | Valor Local por Defecto |
| :--- | :--- | :--- |
| `OLLAMA_BASE_URL` | URL del servicio Ollama | `http://localhost:11434` |
| `OLLAMA_MODEL` | Modelo LLM a utilizar | `llama3.1` |
| `OLLAMA_TIMEOUT_SECONDS` | Timeout máximo para respuestas de IA | `3600` |
| `DATABASE_URL` | Cadena de conexión SQLite para FastAPI | `sqlite:///./initiatives.db` |
| `ConnectionStrings__InitiativesDb` | Cadena de conexión SQLite para .NET | `Data Source=../backend-fastapi/initiatives.db;Cache=Shared` |
| `FASTAPI_BASE_URL` | URL base de FastAPI (usada por .NET para delegar IA) | `http://127.0.0.1:8000` |
| `CORS_ALLOWED_ORIGINS` | Orígenes permitidos por los backends | `http://localhost:5173,http://127.0.0.1:5173` |
| `VITE_API_URL` | Endpoint objetivo consumido por el Frontend | `http://localhost:8000` |

---
