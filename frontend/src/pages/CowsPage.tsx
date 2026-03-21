import { useEffect, useState } from "react";
import { createCow, getCows } from "../services/cowService";
import type { Cow } from "../types/cow";

function CowsPage() {
  const [cows, setCows] = useState<Cow[]>([]);
  const [loading, setLoading] = useState(true);
  const [loadError, setLoadError] = useState("");
  const [submitError, setSubmitError] = useState("");
  const [tagNumber, setTagNumber] = useState("");
  const [breed, setBreed] = useState("");
  const [healthStatus, setHealthStatus] = useState("");

  useEffect(() => {
    async function loadCows() {
      try {
        setLoadError("");
        const data = await getCows();
        setCows(data);
      } catch (err) {
        const message =
          err instanceof Error ? err.message : "Failed to load cows";
        setLoadError(message);
      } finally {
        setLoading(false);
      }
    }

    loadCows();
  }, []);

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();

    try {
      setSubmitError("");

      const newCow = await createCow({
        tagNumber,
        breed,
        healthStatus,
      });

      setCows((prev) => [...prev, newCow]);
      setTagNumber("");
      setBreed("");
      setHealthStatus("");
    } catch (err) {
      const message =
        err instanceof Error ? err.message : "Failed to create cow";
      setSubmitError(message);
    }
  }

  return (
    <div>
      <h1>HerdFlow</h1>
      <h2>All Cows</h2>

      <form onSubmit={handleSubmit}>
        <input
          type="text"
          placeholder="Tag Number"
          value={tagNumber}
          onChange={(e) => setTagNumber(e.target.value)}
        />
        <input
          type="text"
          placeholder="Breed"
          value={breed}
          onChange={(e) => setBreed(e.target.value)}
        />
        <input
          type="text"
          placeholder="Health Status"
          value={healthStatus}
          onChange={(e) => setHealthStatus(e.target.value)}
        />
        <button type="submit">Add Cow</button>
      </form>

      {submitError && <p>{submitError}</p>}
      {loadError && <p>{loadError}</p>}

      {loading ? (
        <p>Loading cows...</p>
      ) : cows.length === 0 ? (
        <p>No cows found.</p>
      ) : (
        <ul>
          {cows.map((cow) => (
            <li key={cow.id}>
              #{cow.id} — Tag: {cow.tagNumber} — Breed: {cow.breed}
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}

export default CowsPage;
