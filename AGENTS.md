Estoy construyendo una prueba técnica: una app para registrar "iniciativas de negocio"
y analizarlas con IA.

Stack:
- Backend principal: FastAPI (Python), capas separadas: routers / services / repositories.
- Backend secundario: .NET (ASP.NET Core Web API), que expone los mismos endpoints y
  para el análisis con IA invoca por HTTP al backend FastAPI (no reimplementa la IA).
- Base de datos: SQLite.
- Frontend: React + Vite, configurable por variable de entorno para apuntar a
  FastAPI o a .NET. 
- IA: Ollama corriendo localmente, pero el proveedor debe estar desacoplado
  (patrón adapter) para poder cambiarlo por OpenAI/Azure OpenAI más adelante
  sin tocar el resto de la lógica.

Entidad Initiative: id, name, description, status, business_problem,
expected_benefit, created_at (+ campo para guardar el resultado del análisis).

Reglas para todo el código que generes:
- Sin credenciales ni URLs hardcodeadas: todo por variables de entorno.
- Manejo explícito de errores (404 si no existe la iniciativa, error controlado
  si un servicio externo falla).
- Código organizado en capas, nunca todo en un archivo.
- Explícame las decisiones no triviales en comentarios breves o en tu respuesta,
  no solo el código.