# Fade runtime diagnosis and bitmap content correction

The user confirmed that the first bitmap implementation did not animate correctly, then observed black followed by the original card after the clock fix. These observations are accepted as runtime evidence.

Confirmed source defect: the animation lived in EditorWindow.Update while the real window remained hidden until completion. Unity 6000.0 documents Update as running on visible windows. It now subscribes directly to EditorApplication.update and unsubscribes on disable.

Actual Unity logs after that change confirmed begin -> midpoint -> complete. The user's black-frame observation showed that a successful PrintWindow call did not establish valid captured Unity GPU content. This path is removed from PreviewFade.

The fade frame is now composed from the same card background/border/accent colors, existing status-icon texture and pre-minimize document snapshot. Non-readable source textures are read through a temporary RenderTexture, not a hidden-window screenshot. The complete BGRA buffer is uploaded to an offscreen GDI bitmap and its title/close glyphs are drawn before the separate overlay is faded. Real Unity/document window alpha, group restore and minimize code are unchanged.

Native error stages are recorded as taskbar.fade.prepare.<phase>.<code>; successful begin/midpoint/complete events contain no document contents or paths. No screen capture or UI control tools were used.

Unity 6000.0 reference:
https://docs.unity3d.com/6000.0/Documentation/ScriptReference/EditorWindow.Update.html

Actual visual confirmation of the newly composed frame remains pending. Existing historical screenshot/source hashes are unchanged.

Validation after frame correction: build passed with zero warnings/errors; 88 hover assertions and 153 taskbar assertions passed. The new offscreen GDI test verified bitmap upload, readback, BGR channels and top-down orientation without creating or showing a window. Native visual confirmation remains pending.
