import React from "react";

import InitiativeCard from "./InitiativeCard";

export default function InitiativeList({ initiatives, onAnalyze }) {
  if (initiatives.length === 0) {
    return <p>No hay iniciativas registradas.</p>;
  }

  return (
    <section>
      <h2>Iniciativas</h2>
      <div className="initiative-list">
        {initiatives.map((initiative) => (
          <InitiativeCard key={initiative.id} initiative={initiative} onAnalyze={onAnalyze} />
        ))}
      </div>
    </section>
  );
}
