import { useEffect, useState } from "react";
import {
  getNotes,
  createNote,
  deleteNote,
  updateNote,
} from "../services/noteService";

type Note = {
  id: number;
  content: string;
  createdAt: string;
  updatedAt?: string;
};

type Props = {
  cowId: number;
};

function Notes({ cowId }: Props) {
  const [notes, setNotes] = useState<Note[]>([]);
  const [newNote, setNewNote] = useState("");
  const [editingId, setEditingId] = useState<number | null>(null);
  const [editingContent, setEditingContent] = useState("");

  useEffect(() => {
    async function loadNotes() {
      const data = await getNotes(cowId);
      setNotes(data);
    }

    loadNotes();
  }, [cowId]);

  async function handleAdd() {
    if (!newNote.trim()) return;

    try {
      const created = await createNote(cowId, newNote);
      setNotes((prev) => [created, ...prev]);
      setNewNote("");
    } catch {
      alert("Failed to add note");
    }
  }

  async function handleDelete(noteId: number) {
    await deleteNote(cowId, noteId);
    setNotes((prev) => prev.filter((n) => n.id !== noteId));
  }

  function startEditing(note: Note) {
    setEditingId(note.id);
    setEditingContent(note.content);
  }

  async function saveEdit(noteId: number) {
    if (!editingContent.trim()) return;

    try {
      const updated = await updateNote(cowId, noteId, editingContent);

      setNotes((prev) => prev.map((n) => (n.id === noteId ? updated : n)));

      setEditingId(null);
      setEditingContent("");
    } catch {
      alert("Failed to update note");
    }
  }

  return (
    <section className="dashboardCard">
      <div className="dataCardHeader">
        <h2 className="cardTitle">Notes</h2>
        <span className="cardSubtle">Internal record</span>
      </div>

      <div className="notesContainer">
        <div className="notesBody">
          {notes.length === 0 ? (
            <span className="emptyState">No notes yet.</span>
          ) : (
            notes.map((note) => (
              <div key={note.id} className="noteItem">
                <div
                  className="noteContent"
                  onDoubleClick={() => startEditing(note)}
                >
                  {editingId === note.id ? (
                    <textarea
                      ref={(el) => {
                        if (el) {
                          el.style.height = "auto";
                          el.style.height = el.scrollHeight + "px";
                        }
                      }}
                      value={editingContent}
                      onChange={(e) => setEditingContent(e.target.value)}
                      onBlur={() => saveEdit(note.id)}
                      onKeyDown={(e) => {
                        if (e.key === "Enter" && !e.shiftKey) {
                          e.preventDefault();
                          saveEdit(note.id);
                        }
                      }}
                      autoFocus
                      rows={Math.max(3, editingContent.split("\n").length)}
                    />
                  ) : (
                    <>
                      <span>{note.content}</span>
                      <span className="noteTimestamp">
                        {new Date(note.createdAt).toLocaleString(undefined, {
                          month: "short",
                          day: "numeric",
                          hour: "numeric",
                          minute: "2-digit",
                        })}
                        {note.updatedAt &&
                          note.updatedAt !== note.createdAt && (
                            <span> • Edited</span>
                          )}
                      </span>
                    </>
                  )}
                </div>

                <button onClick={() => handleDelete(note.id)}>✕</button>
              </div>
            ))
          )}
        </div>

        <div className="addNoteRow">
          <textarea
            value={newNote}
            onChange={(e) => setNewNote(e.target.value)}
            onKeyDown={(e) => {
              if (e.key === "Enter" && !e.shiftKey) {
                e.preventDefault();
                handleAdd();
              }
            }}
            placeholder="Add a note..."
          />
          <button onClick={handleAdd}>Add</button>
        </div>
      </div>
    </section>
  );
}

export default Notes;
