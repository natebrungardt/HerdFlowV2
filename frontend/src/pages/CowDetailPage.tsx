import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { getCowById, deleteCow } from "../services/cowService";
import type { Cow } from "../types/cow";

function CowDetailPage() {
  const { id } = useParams();
  const [cow, setCow] = useState<Cow | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const navigate = useNavigate();

  useEffect(() => {
    async function loadCow() {
      try {
        if (!id) return;
        const data = await getCowById(Number(id));
        setCow(data);
      } catch (err) {
        const message =
          err instanceof Error ? err.message : "Failed to load cow";
        setError(message);
      } finally {
        setLoading(false);
      }
    }

    loadCow();
  }, [id]);

  async function handleDelete() {
    if (!cow) return;

    const confirmed = window.confirm(`Delete cow with tag #${cow.tagNumber}?`);

    if (!confirmed) return;

    try {
      await deleteCow(cow.id);
      navigate("/cows");
    } catch (err) {
      const message =
        err instanceof Error ? err.message : "Failed to delete cow";
      setError(message);
    }
  }

  if (loading) return <p>Loading cow...</p>;
  if (error) return <p style={{ color: "red" }}>{error}</p>;
  if (!cow) return <p>Cow not found</p>;

  return (
    <div style={{ padding: "20px", maxWidth: "1000px", margin: "0 auto" }}>
      {/* NAVBAR */}
      <div style={{ display: "flex", gap: "10px", marginBottom: "20px" }}>
        <div style={navBox}>Tag #: {cow.tagNumber}</div>
        <div style={navBox}>Health: {cow.healthStatus}</div>
        <div style={navBox}>Group: {cow.livestockGroup}</div>
        <div style={navAction} onClick={handleDelete}>
          Remove
        </div>
      </div>

      {/* MAIN GRID */}
      <div
        style={{
          display: "grid",
          gridTemplateColumns: "1fr 2fr 1fr",
          gap: "20px",
        }}
      >
        {/* LEFT PANEL */}
        <div style={panel}>
          <p>
            <strong>Purchase Price:</strong> {cow.purchasePrice}
          </p>
          <p>
            <strong>Sale Price:</strong> {cow.salePrice}
          </p>
          <p>
            <strong>Purchase Date:</strong> {cow.purchaseDate}
          </p>
          <p>
            <strong>Sale Date:</strong> {cow.saleDate}
          </p>
        </div>

        {/* CENTER PANEL */}
        <div style={panel}>
          <div style={field}>
            <strong>Owner:</strong> {cow.ownerName}
          </div>
          <div style={field}>
            <strong>Health:</strong> {cow.healthStatus}
          </div>
          <div style={field}>
            <strong>Breeding:</strong> {cow.breedingStatus}
          </div>
          <div style={field}>
            <strong>Breed:</strong> {cow.breed}
          </div>
          <div style={field}>
            <strong>Sex:</strong> {cow.sex}
          </div>
          <div style={field}>
            <strong>DOB:</strong> {cow.dateOfBirth}
          </div>
        </div>

        {/* RIGHT PANEL */}
        <div style={panel}>
          <strong>Notes</strong>
          <p>{cow.notes}</p>
        </div>
      </div>

      {/* ACTIVITY LOG */}
      <div style={{ marginTop: "30px", ...panel }}>
        <h2>Activity Log</h2>
        <p>Coming soon...</p>
      </div>
    </div>
  );
}

// styles
const panel = {
  border: "1px solid #ccc",
  borderRadius: "10px",
  padding: "15px",
};

const field = {
  border: "1px solid #ddd",
  borderRadius: "8px",
  padding: "10px",
  marginBottom: "10px",
};

const navBox = {
  border: "1px solid #ccc",
  borderRadius: "8px",
  padding: "10px 15px",
  fontWeight: "bold",
};

const navAction = {
  border: "1px solid #ccc",
  borderRadius: "8px",
  padding: "10px 15px",
  cursor: "pointer",
};

export default CowDetailPage;
