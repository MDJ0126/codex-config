# GrowingCity

[English](Documentation/en/README.md) · [한국어](Documentation/ko/README.md) · [日本語](Documentation/ja/README.md) · [简体中文](Documentation/zh-Hans/README.md) · [繁體中文](Documentation/zh-Hant/README.md) · [Deutsch](Documentation/de/README.md) · [Français](Documentation/fr/README.md) · [Español](Documentation/es/README.md) · [Português](Documentation/pt/README.md)

[Documentation — system language](Documentation/index.html)

Unity 6000.0.83f1 / URP 17.0.4. Open `Assets/Scenes/City Prototype.unity`.

Automatically growing city and directional bomber combat prototype. Nuclear cooldown: 100 seconds, independent of normal sortie credits.

External assets under `Assets/polyperfect/Low Poly Epic City/` must remain unchanged. Actual Play and browser deployment require separate verification.

City growth now accelerates automatically with time (up to 8x). Missiles start at building level 3. Fuel, ammunition and power facilities trigger chain explosions when destroyed.

Volatile buildings use a bright blue body material. Destruction now uses multiple collapse motions and physical facade fragments. Actual Play/shader rendering remains unverified.

Prepared city:96 cells; growth batches:4; bounded24×24 map. Advanced direct construction increases from90 to300 game seconds, including reconstruction.
