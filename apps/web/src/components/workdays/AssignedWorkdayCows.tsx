import { Link } from "react-router-dom";
import type { WorkdayCowAssignment } from "../../types/workday";

type AssignedWorkdayCowsProps = {
  assignments: WorkdayCowAssignment[];
  removingCowId: string | null;
  updatingCowId: string | null;
  onRemove: (cowId: string) => void;
  onToggleWorked: (cowId: string, isWorked: boolean) => void;
};

function AssignedWorkdayCows({
  assignments,
  removingCowId,
  updatingCowId,
  onRemove,
  onToggleWorked,
}: AssignedWorkdayCowsProps) {
  return (
    <div className="card workdaySelectedSummaryCard">
      <div className="sectionHeader">
        <div>
          <h2 className="sectionTitle">Assigned Cows</h2>
          <span className="sectionSubtle">
            Check a box when that cow has been worked.
          </span>
        </div>
        <span className="sectionSubtle">{assignments.length} on this workday</span>
      </div>

      {assignments.length === 0 ? (
        <p className="emptyState">No cows have been added to this workday yet.</p>
      ) : (
        <div className="workdaySelectionPills">
          {assignments.map((assignment) => (
            <div
              key={assignment.id}
              className={`workdaySelectionPill workdayAssignedPill${assignment.status === "Worked" ? " workdayAssignedPillWorked" : ""}`}
            >
              <label className="workdayWorkedCheckboxLabel">
                <input
                  type="checkbox"
                  className="workdayWorkedCheckbox"
                  checked={assignment.status === "Worked"}
                  disabled={updatingCowId === assignment.cowId}
                  onChange={(event) =>
                    onToggleWorked(assignment.cowId, event.target.checked)
                  }
                  aria-label={`Mark Tag #${assignment.cow.tagNumber} as worked`}
                />
              </label>

              <Link
                className="workdaySelectionPillLink"
                to={`/cows/${assignment.cow.id}`}
              >
                <span>Tag #{assignment.cow.tagNumber}</span>
                <span className="workdaySelectionPillMeta">
                  {assignment.status === "Worked" ? "Worked" : "Pending"} •{" "}
                  {assignment.cow.healthStatus || "Unknown health status"}
                </span>
              </Link>

              <button
                type="button"
                className="workdaySelectionRemoveButton"
                onClick={() => onRemove(assignment.cowId)}
                disabled={removingCowId === assignment.cowId}
                aria-label={`Remove cow #${assignment.cow.tagNumber}`}
                title={`Remove cow #${assignment.cow.tagNumber}`}
              >
                {removingCowId === assignment.cowId ? "…" : "X"}
              </button>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}

export default AssignedWorkdayCows;
