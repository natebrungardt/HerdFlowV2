const API_BASE_URL = import.meta.env.VITE_API_URL;

export async function getNotes(cowId: number) {
  const res = await fetch(`${API_BASE_URL}/${cowId}/notes`);
  return res.json();
}

export async function createNote(cowId: number, content: string) {
  const res = await fetch(`${API_BASE_URL}/${cowId}/notes`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({ content }),
  });

  return res.json();
}

export async function deleteNote(cowId: number, noteId: number) {
  await fetch(`${API_BASE_URL}/${cowId}/notes/${noteId}`, {
    method: "DELETE",
  });
}

export async function updateNote(
  cowId: number,
  noteId: number,
  content: string,
) {
  const res = await fetch(`${API_BASE_URL}/${cowId}/notes/${noteId}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({ content }),
  });

  return res.json();
}
