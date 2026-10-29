# Omitted shape outline fix

WorkbookDrawings now marks a shape as NoLine when both its local a:ln and its xdr:style/a:lnRef are absent. Explicit outlines, explicit noFill and the existing inherited-style behavior are retained. Source XLSX packages were not edited.

Build: zero warnings/errors. Production reader/edit/save/reopen suite: 713 assertions passed, including all 13 borderless gradient shapes and retained authored catalog outlines. Six-file package audit passed. Production raster preview was regenerated and visually inspected: the gradient gallery is borderless. Native Unity UI and screenshot recapture were not performed under the existing computer-use prohibition. All nine guide descriptions were regenerated; documentation checks report only pending/stale native screenshot requirements. Historical screenshot/source hashes remain unchanged.

The inherited line-style resolver was not expanded by this focused fix.
