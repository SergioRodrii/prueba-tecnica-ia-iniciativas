import React, { useState } from "react";

import AnalysisResult from "./AnalysisResult";

function formatDate(value) {
  const date = new Date(value);
  return Number.isNaN(date.getTime()) ? value : new Intl.DateTimeFormat("es-CO", { dateStyle: "medium", timeStyle: "short" }).format(date);
}

export default function InitiativeCard({ initiative, onAnalyze }) {
  const [analysis, setAnalysis] = useState(initiative.analysis_result);
  const [isAnalyzing, setIsAnalyzing] = useState(false);

  async function handleAnalyze() {
    setIsAnalyzing(true);
    try {
      setAnalysis(await onAnalyze(initiative.id));
    } catch {
      return;
    } finally {
      setIsAnalyzing(false);
    }
  }

  return (
    <article className="initiative-card">
      <h3>{initiative.name}</h3>
      <p>{initiative.description}</p>
      <dl>
        <div><dt>Estado</dt><dd>{initiative.status}</dd></div>
        <div><dt>Creada</dt><dd>{formatDate(initiative.created_at)}</dd></div>
      </dl>
      <button type="button" onClick={handleAnalyze} disabled={isAnalyzing}>
        {isAnalyzing ? "Analizando..." : "Analizar con IA"}
      </button>
      {analysis && <AnalysisResult analysis={analysis} />}
    </article>
  );
}
