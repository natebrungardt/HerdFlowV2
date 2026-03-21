import { useEffect, useState } from "react";
import { createCow, getCows } from "../services/cowService";
import type { Cow } from "../types/cow";

function CowsPage() {
  const [cows, setCows] = useState<Cow[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [tagNumber, setTagNumber] = useState("");
  const [breed, setBreed] = useState("");
  const [healthStatus, setHealthStatus] = useState("");

  useEffect(() => {
    async function loadCows() {
      try {
        const data = await getCows();
        setCows(data);
      } catch (err) {
        setError("Failed to load cows");
      } finally {
        setLoading(false);
      }
    }

    loadCows();
  }, []);

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();

    try {
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
      setError("Failed to create cow");
    }
  }

  if (loading) return <p>Loading cows...</p>;
  if (error) return <p>{error}</p>;

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
      {cows.length === 0 ? (
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
