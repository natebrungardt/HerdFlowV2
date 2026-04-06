import { useEffect, useState } from "react";
import HerdListView from "../../components/cows/HerdListView";
import { getRemovedCows } from "../../services/cowService";
import type { Cow } from "../../types/cow";
import "../../styles/AllCows.css";

function formatRemovedDate(dateValue: string | null | undefined) {
  if (!dateValue) {
    return null;
  }

  const date = new Date(dateValue);

  if (Number.isNaN(date.getTime())) {
    return null;
  }

  return new Intl.DateTimeFormat("en-US", {
    month: "short",
    day: "numeric",
    year: "numeric",
  }).format(date);
}

function sortRemovedCowsByDate(cows: Cow[]) {
  return [...cows].sort((leftCow, rightCow) => {
    const leftRemovedAt = leftCow.removedAt
      ? new Date(leftCow.removedAt).getTime()
      : null;
    const rightRemovedAt = rightCow.removedAt
      ? new Date(rightCow.removedAt).getTime()
      : null;

    if (leftRemovedAt === null && rightRemovedAt !== null) {
      return 1;
    }

    if (leftRemovedAt !== null && rightRemovedAt === null) {
      return -1;
    }

    if (
      leftRemovedAt !== null &&
      rightRemovedAt !== null &&
      leftRemovedAt !== rightRemovedAt
    ) {
      return rightRemovedAt - leftRemovedAt;
    }

    return leftCow.tagNumber.localeCompare(rightCow.tagNumber, undefined, {
      numeric: true,
      sensitivity: "base",
    });
  });
}

function RemovedCows() {
  const [cows, setCows] = useState<Cow[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    async function loadRemovedCows() {
      try {
        setError("");
        const data = await getRemovedCows();
        setCows(data);
      } catch (err) {
        const message =
          err instanceof Error ? err.message : "Failed to load cows";
        setError(message);
      } finally {
        setLoading(false);
      }
    }

    loadRemovedCows();
  }, []);

  return (
    <HerdListView
      cows={cows}
      loading={loading}
      error={error}
      title="Archived Cows"
      subtitle="Review archived herd records and jump back into individual histories."
      getCowHref={(cow) => `/cows/${cow.id}`}
      emptyMessage="No archived cows found."
      sortCows={sortRemovedCowsByDate}
      getCowSupplementaryMeta={(cow) => {
        const formattedDate = formatRemovedDate(cow.removedAt);
        return formattedDate ? `Date Removed: ${formattedDate}` : null;
      }}
    />
  );
}

export default RemovedCows;
