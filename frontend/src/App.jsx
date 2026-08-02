import React, { useCallback, useEffect, useState } from "react";

import { analyzeInitiative, createInitiative, getInitiatives } from "./api/api";
import InitiativeForm from "./components/InitiativeForm";
import InitiativeList from "./components/InitiativeList";

function getErrorMessage(error) {
  return error instanceof Error ? error.message : "Ocurrió un error inesperado.";
}

export default function App() {
  const [initiatives, setInitiatives] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [errorMessage, setErrorMessage] = useState("");

  const loadInitiatives = useCallback(async () => {
    setIsLoading(true);
    setErrorMessage("");
    try {
      setInitiatives(await getInitiatives());
    } catch (error) {
      setErrorMessage(getErrorMessage(error));
      throw error;
    } finally {
      setIsLoading(false);
    }
  }, []);

  useEffect(() => {
    void loadInitiatives().catch(() => undefined);
  }, [loadInitiatives]);

  async function handleCreate(payload) {
    setErrorMessage("");
    try {
      await createInitiative(payload);
      await loadInitiatives();
    } catch (error) {
      setErrorMessage(getErrorMessage(error));
      throw error;
    }
  }

  async function handleAnalyze(initiativeId) {
    setErrorMessage("");
    try {
      return await analyzeInitiative(initiativeId);
    } catch (error) {
      setErrorMessage(getErrorMessage(error));
      throw error;
    }
  }

  return (
    <main className="page">
      <h1>Iniciativas de negocio</h1>
      <InitiativeForm onCreate={handleCreate} />
      {errorMessage && <p className="error-message">{errorMessage}</p>}
      {isLoading ? (
        <p>Cargando iniciativas...</p>
      ) : (
        <InitiativeList initiatives={initiatives} onAnalyze={handleAnalyze} />
      )}
    </main>
  );
}
