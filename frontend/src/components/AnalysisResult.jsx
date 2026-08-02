import React from "react";

const fields = [
  ["Problema de negocio", "business_problem"],
  ["Objetivos sugeridos", "suggested_objectives"],
  ["Beneficios esperados", "expected_benefits"],
  ["Riesgos", "risks"],
  ["Preguntas abiertas", "open_questions"],
];

export default function AnalysisResult({ analysis }) {
  return (
    <section className="analysis-result">
      <h4>Resultado del análisis</h4>
      {fields.map(([label, key]) => (
        <div key={key}>
          <strong>{label}</strong>
          {Array.isArray(analysis[key]) ? (
            <ul>{analysis[key].map((item) => <li key={item}>{item}</li>)}</ul>
          ) : (
            <p>{analysis[key]}</p>
          )}
        </div>
      ))}
    </section>
  );
}
