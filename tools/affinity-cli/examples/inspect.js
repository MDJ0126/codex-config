// Trusted read-only script. Use `run examples/inspect.js`.
const { Application } = require('/application.js');
const { Document } = require('/document.js');
console.log(JSON.stringify({
  version: Application.version,
  documents: Document.all.map(document => ({
    id: String(document.sessionUuid),
    title: document.title,
    path: document.path,
    dpi: document.dpi,
    width: document.widthPixels,
    height: document.heightPixels
  }))
}));
