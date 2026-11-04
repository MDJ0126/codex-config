# Vegetation palette

## Start palette

Click Start palette and save an asset in the project. Editing opens after saving; cancelling leaves the start screen unchanged. With Samples installed, five plant cards are added. Pick a card, assign a ground collider and click Use selected ground. Enable Scene brush or Fill area. Add plant opens its source settings; assign a profile there. Without Samples, add your own profiles.

## Mix enabled entries

Palette weights express relative frequency, not guaranteed final counts. Large plants are scheduled before grass; minimum spacing, slope, valid surfaces and exclusions can reduce the result. Disable mixing and select one row to paint a single type.

## Boundary and exclusions

The canvas is the editable scope. Existing patches remain untouched unless you explicitly adopt one. Rectangles and ellipses use canvas-local XZ size. For a polygon, click ground points and press Enter; unfinished polygons cannot be filled. Exclusions reserve each collider’s world XZ bounding footprint, not its exact silhouette.

## Save canvas scene

Painted positions and layer references are saved in the scene. The palette stores type choices and brush settings in an Editor-only asset. Save the palette and canvas scene separately. A completed brush stroke is one Undo group; Esc rolls back the current stroke. Scene input and native keyboard routing require actual editor verification.

## Rebuild canvas from palette

Rebuild replaces all vegetation owned by this canvas using the current palette. Undo restores the previous layout. Other canvases are unchanged.

## Placement settings

A brush stamp adds at most512 plants; the canvas defaults to20,000 instances and permits up to100,000. Spacing or missing valid surfaces can stop filling before the density target. Added geometry increases rendering cost; these are authoring controls, not a measured runtime optimization.

## Properties

Native Unity Physics, Undo/Redo, palette and scene save/reopen, and nine-language window updates passed. Start/cancel, plant-card selection and the tool menu were checked with native mouse input. Native Scene drag/shortcuts and all nine visual layouts remain unverified.
