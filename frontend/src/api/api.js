import { apiUrl } from "../config/api";

function buildUrl(path) {
  if (!apiUrl) {
    throw new Error("Falta configurar VITE_API_URL.");
  }
  return `${apiUrl.replace(/\/$/, "")}${path}`;
}

async function request(path, options = {}) {
  let response;
  try {
    response = await fetch(buildUrl(path), options);
  } catch {
    throw new Error("No fue posible conectar con el backend.");
  }

  const contentType = response.headers.get("content-type") || "";
  const body = contentType.includes("application/json") ? await response.json() : await response.text();

  if (!response.ok) {
    const detail = typeof body === "object" && body?.detail ? body.detail : "La solicitud no pudo completarse.";
    throw new Error(detail);
  }

  return body;
}

export function getInitiatives() {
  return request("/initiatives");
}

export function createInitiative(initiative) {
  return request("/initiatives", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(initiative),
  });
}

export function analyzeInitiative(initiativeId) {
  return request(`/initiatives/${initiativeId}/analyze`, { method: "POST" });
}
