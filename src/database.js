import PouchDB from "pouchdb-browser";

const missions = new PouchDB("mi-missions");

export function saveMission(record) {
  return missions.put(record);
}

export async function getMissions() {
  const { rows } = await missions.allDocs({ include_docs: true });
  return rows.map((record) => ({
    _id: record.id,
    ...(record.doc && record.doc),
  }));
}
