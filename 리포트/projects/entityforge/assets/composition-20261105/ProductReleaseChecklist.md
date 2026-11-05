# TurnRoom product release checks

## Product UI and installation

- Commands live under `Tools/TurnRoom`, with existing shortcuts and validation pairs preserved. Do not disable automatic compilation or change project/editor preferences without the user's instruction.
- Shared settings own the display language across products and projects. Localize windows, Inspectors, options, tooltips and status/error text in all nine supported languages; verify actual switching in open windows. Keep editor resources, integrations, samples and development code in their appropriate assemblies.
- Use ordinary text for product explanations, instructions and normal status. Reserve the shared help box for warnings/errors or information requiring separate attention. Name-only tooltips use the compact style. When a window has no icon, remove the empty icon gap before its title.
- The minimized-window shelf belongs below the console/status line. Check the actual pressed interval, restoration, intermediate visibility and normal editor activation, not just final window flags.
- Global settings refresh once when opened and expose a corner refresh icon. Use `최신 버전` for an equal public version, green for available updates, a store link for each registered product, and an update action only when an update is available. Update/dependency actions open Package Manager; they do not install packages silently.

## Sales copy and claims

- Entity Forge remains a free Asset Store product. Preserve the saved Free asset setting during metadata updates and uploads; do not apply the other products' paid pricing to it.
- Entity Forge runtime QA includes explicit Forge motion and automatic source renderers sharing the same mesh/material in one scene. Check proxy pages, original-renderer suppression, movement/material synchronization, fallback and disposal in actual players; test counts alone do not certify rendering.

- Keep copy short: purpose, useful features, verified compatibility, dependencies and material limitations. Apply supported portal languages separately from product UI translations. Save, reopen and compare persisted fields; a saved draft is not a submitted product.
- Explain user benefits plainly. Entity Forge can describe automatic DOTS conversion for supported rendering and project optimization, while preserving the actual supported scope. Performance numbers require paired player measurements with scene/build/hardware provenance. Never infer improvement from object count or use an unverified percentage.

## Sales images

- Use the approved DDT feature-gallery style as the reference: a restrained dark background, product accent, a readable short headline, one concise explanation and a large actual product/result image. Inspect the reference itself before making variants.
- Preserve the approved DDT cover/card/lead-image composition with three deliberately overlapping workbook windows. Entity Forge cover/card/lead images show an actual included example scene; keep configuration-window captures for secondary feature images. Compare the approved composition before exporting replacements.
- Give every product a comparable, useful gallery. For the current refresh, prepare sixteen distinct gallery images per product, plus the required cover/card/icon/social slots. Count gallery images separately from key-image variants. Do not pad a gallery with duplicate captures or repeated claims.
- Feature images should demonstrate different workflows, comparisons and included samples. Use real Unity captures or real generated outputs; do not fabricate an interface or imply a static illustration is a running product. Label material limits and older source captures honestly.
- Produce and edit images in actual Affinity. Keep editable native masters with separate text, artwork and screenshot layers, plus exported files, source paths, capture scope and hashes in the product's delivery folder. Review full size and store-thumbnail size for typography, clipping, contrast, spacing and legibility. Verify save/reopen/export consistency.

## Sales video and audio

- Promotional video must include audible, suitable audio unless the user explicitly requests silence. A valid video stream or an audio track alone is not a quality pass.
- Verify codec, duration, full decode, audio signal/loudness, clipping, long unintended silence, fades and picture/audio alignment, then actually audition the result. Preserve source footage and record any visual changes separately from an audio-only mux.
- Verify music/SFX rights and provide required attribution in the accompanying upload description and credits. Promotional-only music must not enter the customer Unity package. Preserve the license/source record.

## Final delivery

- Compare the final committed source with frozen exported packages and hashes before installation/upload. Run the official validator on release scope; distinguish development fixtures from shipped files. Preserve sample geometry, animation bindings, object identifiers, existing scenes and settings.
- Review the saved publisher gallery and playable video after a full reload. Compare every product slot with the frozen final source: actual video content/version and sound, generated poster, current logo/icon (including crop and padding), card/cover, image count/order and localized fields. A Video tag, successful upload or matching item count does not prove current media or playable sound. Preserve old media before replacement, verify the new video through its end and inspect the actual server-generated poster and saved icon at native size. Do not report local preparation, upload, draft save, submission and publication as the same event.
- While the user uses the computer, do not take mouse/keyboard focus or start/restart GUI processes. Continue verified background APIs and file work; leave steps that actually require foreground UI pending until allowed.
- Treat background resource use separately from focus. On a resource-limited request, run one image or video job at a time, apply and verify a CPU/priority budget, monitor memory and responsiveness, and retain existing outputs to avoid unnecessary work. Render each image once; bounded pixel transport must reuse that render, never rerender the full image for each row band. Verify transported pixels and close temporary transport documents before the next image.

