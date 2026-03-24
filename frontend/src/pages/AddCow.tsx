import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { createCow } from "../services/cowService";
import "../styles/CowDetailPage.css";

type FormState = {
  tagNumber: string;
  ownerName: string;
  breed: string;
  sex: string;
  healthStatus: string;
  heatStatus: string;
  breedingStatus: string;
  livestockGroup: string;
  dateOfBirth: string;
  purchaseDate: string;
  saleDate: string;
  purchasePrice: string;
  salePrice: string;
  notes: string;
};

const initialFormState: FormState = {
  tagNumber: "",
  ownerName: "",
  breed: "",
  sex: "",
  healthStatus: "Healthy",
  heatStatus: "",
  breedingStatus: "",
  livestockGroup: "",
  dateOfBirth: "",
  purchaseDate: "",
  saleDate: "",
  purchasePrice: "",
  salePrice: "",
  notes: "",
};

function getOwnerInitial(name: string) {
  if (!name.trim()) return "?";
  return name.trim().charAt(0).toUpperCase();
}

function formatCurrencyPreview(value: string) {
  if (!value) return "—";

  const numericValue = Number(value);

  if (Number.isNaN(numericValue)) return "—";

  return new Intl.NumberFormat("en-US", {
    style: "currency",
    currency: "USD",
    maximumFractionDigits: 0,
  }).format(numericValue);
}

function AddCowPage() {
  const navigate = useNavigate();
  const [formData, setFormData] = useState<FormState>(initialFormState);
  const [error, setError] = useState("");
  const [saving, setSaving] = useState(false);

  function handleChange(
    event: React.ChangeEvent<
      HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement
    >,
  ) {
    const { name, value } = event.target;

    setFormData((current) => ({
      ...current,
      [name]: value,
    }));
  }

  async function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setError("");
    setSaving(true);

    try {
      await createCow({
        tagNumber: formData.tagNumber,
        ownerName: formData.ownerName,
        breed: formData.breed,
        sex: formData.sex,
        healthStatus: formData.healthStatus || "Healthy",
        heatStatus: formData.heatStatus || null,
        breedingStatus: formData.breedingStatus || null,
        livestockGroup: formData.livestockGroup,
        dateOfBirth: formData.dateOfBirth || null,
        purchaseDate: formData.purchaseDate || null,
        saleDate: formData.saleDate || null,
        purchasePrice: formData.purchasePrice
          ? Number(formData.purchasePrice)
          : null,
        salePrice: formData.salePrice ? Number(formData.salePrice) : null,
        notes: formData.notes || null,
      });

      navigate("/cows");
    } catch (err) {
      const message =
        err instanceof Error ? err.message : "Failed to create cow";
      setError(message);
      setSaving(false);
      return;
    }

    setSaving(false);
  }

  return (
    <div className="cowDetailPage">
      <div className="cowDetailShell">
        <form className="cowDashboardGrid" onSubmit={handleSubmit}>
          <div className="leftColumn">
            <section className="dashboardCard heroCard">
              <div className="eyebrow">New Herd Record</div>

              <div className="heroHeader">
                <div className="titleBlock">
                  <h1 className="cowTitle">Add New Cow</h1>
                  <p className="cowSubtitle">
                    Create a new herd record with ownership, lifecycle, and
                    status details.
                  </p>
                </div>

                <div
                  style={{
                    display: "flex",
                    gap: "12px",
                    flexWrap: "wrap",
                    justifyContent: "flex-end",
                  }}
                >
                  <button
                    type="button"
                    className="deleteButton"
                    onClick={() => navigate("/cows")}
                    style={{ background: "rgba(255, 255, 255, 0.08)" }}
                  >
                    Cancel
                  </button>

                  <button
                    type="submit"
                    className="deleteButton"
                    disabled={saving}
                  >
                    {saving ? "Saving..." : "Save Cow"}
                  </button>
                </div>
              </div>

              <div className="metricsGrid">
                <div className="metricCard">
                  <label className="metricLabel" htmlFor="healthStatus">
                    Health Status
                  </label>
                  <select
                    id="healthStatus"
                    name="healthStatus"
                    className="infoValue"
                    value={formData.healthStatus}
                    onChange={handleChange}
                    style={{
                      width: "100%",
                      background: "transparent",
                      border: "none",
                      outline: "none",
                      padding: 0,
                    }}
                  >
                    <option value="Healthy">Healthy</option>
                    <option value="NeedsTreatment">Needs Treatment</option>
                  </select>
                  <div className="metricAccent" />
                </div>

                <div className="metricCard">
                  <label className="metricLabel" htmlFor="livestockGroup">
                    Livestock Group
                  </label>
                  <select
                    id="livestockGroup"
                    name="livestockGroup"
                    className="infoValue"
                    value={formData.livestockGroup}
                    onChange={handleChange}
                    required
                    style={{
                      width: "100%",
                      background: "transparent",
                      border: "none",
                      outline: "none",
                      padding: 0,
                    }}
                  >
                    <option value="">Select group</option>
                    <option value="Breeding Cow">Breeding Cow</option>
                    <option value="Market Cow">Market Cow</option>
                    <option value="Feeder Cow">Feeder Cow</option>
                  </select>
                  <div className="metricAccent" />
                </div>

                <div className="metricCard">
                  <label className="metricLabel" htmlFor="breedingStatus">
                    Breeding Status
                  </label>
                  <select
                    id="breedingStatus"
                    name="breedingStatus"
                    className="infoValue"
                    value={formData.breedingStatus}
                    onChange={handleChange}
                    style={{
                      width: "100%",
                      background: "transparent",
                      border: "none",
                      outline: "none",
                      padding: 0,
                    }}
                  >
                    <option value="">Select status</option>
                    <option value="Open">Open</option>
                    <option value="Bred">Bred</option>
                    <option value="Pregnant">Pregnant</option>
                    <option value="N/A">N/A</option>
                  </select>
                  <div className="metricAccent" />
                </div>
              </div>
            </section>

            <section className="dashboardCard">
              <div className="dataCardHeader">
                <h2 className="cardTitle">Cow Details</h2>
                <span className="cardSubtle">Enter profile information</span>
              </div>

              <div className="infoGrid">
                <div className="infoTile">
                  <label className="infoLabel" htmlFor="tagNumber">
                    Tag Number
                  </label>
                  <input
                    id="tagNumber"
                    name="tagNumber"
                    className="infoValue"
                    value={formData.tagNumber}
                    onChange={handleChange}
                    placeholder="Enter tag number"
                    required
                    style={{
                      width: "100%",
                      background: "transparent",
                      border: "none",
                      outline: "none",
                      padding: 0,
                    }}
                  />
                </div>

                <div className="infoTile">
                  <label className="infoLabel" htmlFor="ownerName">
                    Owner
                  </label>
                  <input
                    id="ownerName"
                    name="ownerName"
                    className="infoValue"
                    value={formData.ownerName}
                    onChange={handleChange}
                    placeholder="Enter owner name"
                    required
                    style={{
                      width: "100%",
                      background: "transparent",
                      border: "none",
                      outline: "none",
                      padding: 0,
                    }}
                  />
                </div>

                <div className="infoTile">
                  <label className="infoLabel" htmlFor="breed">
                    Breed
                  </label>
                  <input
                    id="breed"
                    name="breed"
                    className="infoValue"
                    value={formData.breed}
                    onChange={handleChange}
                    placeholder="Enter breed"
                    style={{
                      width: "100%",
                      background: "transparent",
                      border: "none",
                      outline: "none",
                      padding: 0,
                    }}
                  />
                </div>

                <div className="infoTile">
                  <label className="infoLabel" htmlFor="sex">
                    Sex
                  </label>
                  <select
                    id="sex"
                    name="sex"
                    className="infoValue"
                    value={formData.sex}
                    onChange={handleChange}
                    style={{
                      width: "100%",
                      background: "transparent",
                      border: "none",
                      outline: "none",
                      padding: 0,
                    }}
                  >
                    <option value="">Select sex</option>
                    <option value="Male">Male</option>
                    <option value="Female">Female</option>
                  </select>
                </div>

                <div className="infoTile">
                  <label className="infoLabel" htmlFor="heatStatus">
                    Heat Status
                  </label>
                  <select
                    id="heatStatus"
                    name="heatStatus"
                    className="infoValue"
                    value={formData.heatStatus}
                    onChange={handleChange}
                    style={{
                      width: "100%",
                      background: "transparent",
                      border: "none",
                      outline: "none",
                      padding: 0,
                    }}
                  >
                    <option value="">Select heat status</option>
                    <option value="InHeat">In Heat</option>
                    <option value="NotInHeat">Not In Heat</option>
                    <option value="Unknown">Unknown</option>
                  </select>
                </div>

                <div className="infoTile">
                  <label className="infoLabel" htmlFor="dateOfBirth">
                    Date of Birth
                  </label>
                  <input
                    id="dateOfBirth"
                    name="dateOfBirth"
                    type="date"
                    className="infoValue"
                    value={formData.dateOfBirth}
                    onChange={handleChange}
                    style={{
                      width: "100%",
                      background: "transparent",
                      border: "none",
                      outline: "none",
                      padding: 0,
                    }}
                  />
                </div>

                <div className="infoTile">
                  <label className="infoLabel" htmlFor="purchaseDate">
                    Purchase Date
                  </label>
                  <input
                    id="purchaseDate"
                    name="purchaseDate"
                    type="date"
                    className="infoValue"
                    value={formData.purchaseDate}
                    onChange={handleChange}
                    style={{
                      width: "100%",
                      background: "transparent",
                      border: "none",
                      outline: "none",
                      padding: 0,
                    }}
                  />
                </div>

                <div className="infoTile">
                  <label className="infoLabel" htmlFor="saleDate">
                    Sale Date
                  </label>
                  <input
                    id="saleDate"
                    name="saleDate"
                    type="date"
                    className="infoValue"
                    value={formData.saleDate}
                    onChange={handleChange}
                    style={{
                      width: "100%",
                      background: "transparent",
                      border: "none",
                      outline: "none",
                      padding: 0,
                    }}
                  />
                </div>

                <div className="infoTile">
                  <label className="infoLabel" htmlFor="purchasePrice">
                    Purchase Price
                  </label>
                  <input
                    id="purchasePrice"
                    name="purchasePrice"
                    type="number"
                    className="infoValue"
                    value={formData.purchasePrice}
                    onChange={handleChange}
                    placeholder="Enter purchase price"
                    style={{
                      width: "100%",
                      background: "transparent",
                      border: "none",
                      outline: "none",
                      padding: 0,
                    }}
                  />
                </div>

                <div className="infoTile">
                  <label className="infoLabel" htmlFor="salePrice">
                    Sale Price
                  </label>
                  <input
                    id="salePrice"
                    name="salePrice"
                    type="number"
                    className="infoValue"
                    value={formData.salePrice}
                    onChange={handleChange}
                    placeholder="Enter sale price"
                    style={{
                      width: "100%",
                      background: "transparent",
                      border: "none",
                      outline: "none",
                      padding: 0,
                    }}
                  />
                </div>
              </div>
            </section>

            <section className="dashboardCard activityCard">
              <div className="dataCardHeader">
                <h2 className="cardTitle">Form Tips</h2>
                <span className="cardSubtle">Before saving</span>
              </div>

              <div className="activityList">
                <div className="activityItem">
                  <div className="activityDot" />
                  <div>
                    <div className="activityText">
                      Tag number, owner, breed, sex, and health status should be
                      filled out.
                    </div>
                    <div className="activityMeta">Required fields</div>
                  </div>
                </div>

                <div className="activityItem">
                  <div className="activityDot" />
                  <div>
                    <div className="activityText">
                      Add notes for treatment history, breeding context, or any
                      special handling details.
                    </div>
                    <div className="activityMeta">Recommended</div>
                  </div>
                </div>

                <div className="activityItem">
                  <div className="activityDot" />
                  <div>
                    <div className="activityText">
                      Review the live summary on the right before saving the
                      record.
                    </div>
                    <div className="activityMeta">Quality check</div>
                  </div>
                </div>
              </div>
            </section>
          </div>

          <div className="rightColumn">
            <section className="dashboardCard rightSummaryCard">
              <div className="dataCardHeader">
                <h2 className="cardTitle">Profile Summary</h2>
                <span className="cardSubtle">Live preview</span>
              </div>

              <div className="ownerRow">
                <div className="ownerAvatar">
                  {getOwnerInitial(formData.ownerName)}
                </div>
                <div className="ownerMeta">
                  <div className="ownerName">
                    {formData.ownerName || "No owner yet"}
                  </div>
                  <div className="ownerRole">Primary owner</div>
                </div>
              </div>

              <div className="kpiStack">
                <div className="kpiRow">
                  <span className="kpiLabel">Purchase Price</span>
                  <span className="kpiValue">
                    {formatCurrencyPreview(formData.purchasePrice)}
                  </span>
                </div>

                <div className="kpiRow">
                  <span className="kpiLabel">Sale Price</span>
                  <span className="kpiValue">
                    {formatCurrencyPreview(formData.salePrice)}
                  </span>
                </div>

                <div className="kpiRow">
                  <span className="kpiLabel">Status</span>
                  <span className="kpiValue">
                    {formData.breedingStatus || "—"}
                  </span>
                </div>

                <div className="kpiRow">
                  <span className="kpiLabel">Tag Number</span>
                  <span className="kpiValue">
                    {formData.tagNumber ? `#${formData.tagNumber}` : "—"}
                  </span>
                </div>
              </div>
            </section>

            <section className="dashboardCard">
              <div className="dataCardHeader">
                <h2 className="cardTitle">Notes</h2>
                <span className="cardSubtle">Internal record</span>
              </div>

              <textarea
                name="notes"
                className="notesBody"
                value={formData.notes}
                onChange={handleChange}
                placeholder="Add internal notes, treatment details, breeding context, or anything else worth tracking..."
                style={{
                  width: "100%",
                  minHeight: "180px",
                  background: "transparent",
                  border: "none",
                  outline: "none",
                  resize: "vertical",
                  color: "inherit",
                  font: "inherit",
                }}
              />
            </section>

            {error ? (
              <section className="dashboardCard">
                <div className="dataCardHeader">
                  <h2 className="cardTitle">Error</h2>
                  <span className="cardSubtle">Submission issue</span>
                </div>
                <div className="notesBody" style={{ color: "#ffb4b4" }}>
                  {error}
                </div>
              </section>
            ) : null}
          </div>
        </form>
      </div>
    </div>
  );
}

export default AddCowPage;
