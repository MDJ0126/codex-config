# GrowingCity prototype

[English](../en/README.md) · [한국어](../ko/README.md) · [日本語](../ja/README.md) · [简体中文](../zh-Hans/README.md) · [繁體中文](../zh-Hant/README.md) · [Deutsch](../de/README.md) · [Français](../fr/README.md) · [Español](../es/README.md) · [Português](../pt/README.md)

An automatically growing city becomes your bombing playground. Adjacent buildings combine into larger buildings; destroyed areas rebuild.

Open Assets/Scenes/City Prototype.unity in Unity 6000.0.83f1 with URP 17.0.4. This repository currently contains a Unity prototype; browser gameplay has not been verified.

Drag across the city and release to launch a formation in that direction. Aircraft fly from outside the screen to outside the screen. Keys: 1 bomb, 2 nuclear, Space pause/resume, Esc cancel drag.

Each aircraft starts at one bomb per second. Upgrade drop rate, bomb capacity, blast radius, bomber damage, formation size, flight speed, armor and health. City growth upgrades speed up new construction. Upgrades apply to subsequent sorties or new buildings.

Every aircraft that arrives adds a fixed 50% normal sortie charge. Two arrivals provide one use; extra uses are banked. Normal passive recharge takes 10 game seconds and fills only up to one use.

Nuclear bombing has an independent 100-second cooldown, starting when a valid command is accepted, including queued commands. It starts ready. It does not consume normal sortie credit. Arrival bonuses and 3x speed do not reduce it. Pause stops the timer; NEW CITY resets it.

Buildings fire level-scaled missiles with limited tracking. Aircraft can take damage and be shot down. Larger buildings have more health; damage can shrink them. Destruction grants gold and reduces city development.

Language defaults to the system on first launch. The language button cycles through System and nine languages; an explicit choice is saved on this device. Unsupported system languages fall back to English.

## Controls and settings

Original external assets are read-only: Assets/polyperfect/Low Poly Epic City/. Project changes use owned assets and adapters. Do not redistribute store assets independently.

Current limits: actual Unity rendering, runtime language switching, browser deployment and gameplay balance remain unverified in this task. Source compilation and independent logic checks do not replace Play testing.

[HTML](index.html)
