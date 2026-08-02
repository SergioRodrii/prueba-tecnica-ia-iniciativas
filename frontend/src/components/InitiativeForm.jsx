import React, { useState } from "react";

export default function InitiativeForm({ onCreate }) {
  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [isSubmitting, setIsSubmitting] = useState(false);

  async function handleSubmit(event) {
    event.preventDefault();
    setIsSubmitting(true);
    try {
      await onCreate({ name, description });
      setName("");
      setDescription("");
    } catch {
      return;
    } finally {
      setIsSubmitting(false);
    }
  }

  return (
    <form className="initiative-form" onSubmit={handleSubmit}>
      <h2>Crear iniciativa</h2>
      <label>
        Nombre
        <input value={name} onChange={(event) => setName(event.target.value)} required maxLength="255" />
      </label>
      <label>
        Descripción
        <textarea value={description} onChange={(event) => setDescription(event.target.value)} required />
      </label>
      <button type="submit" disabled={isSubmitting}>
        {isSubmitting ? "Guardando..." : "Guardar"}
      </button>
    </form>
  );
}
