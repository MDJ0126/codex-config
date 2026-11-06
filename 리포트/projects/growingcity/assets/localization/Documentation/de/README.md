# GrowingCity-Prototyp

[English](../en/README.md) · [한국어](../ko/README.md) · [日本語](../ja/README.md) · [简体中文](../zh-Hans/README.md) · [繁體中文](../zh-Hant/README.md) · [Deutsch](../de/README.md) · [Français](../fr/README.md) · [Español](../es/README.md) · [Português](../pt/README.md)

Bombardiere eine automatisch wachsende Stadt. Nachbargebäude entwickeln sich gemeinsam weiter; zerstörte Gebiete werden wiederaufgebaut.

Assets/Scenes/City Prototype.unity in Unity 6000.0.83f1 mit URP 17.0.4 öffnen. Dies ist ein Unity-Prototyp; die Browserausführung wurde nicht geprüft.

Über die Stadt ziehen und loslassen, um die Formation in diese Richtung zu starten. Flugzeuge fliegen von außerhalb des Bildes nach außerhalb. 1: Bombe, 2: Nuklear, Leertaste: Pause/Weiter, Esc: Abbruch.

Anfangs wirft jedes Flugzeug eine Bombe pro Sekunde ab. Verbessere Abwurfrate, Bombenlast, Radius, Schaden, Formation, Flugtempo, Panzerung und Leben. Stadtwachstum beschleunigt Neubauten. Änderungen gelten für kommende Flüge oder Neubauten.

Jedes angekommene Flugzeug gibt feste 50% normale Einsatzladung. Zwei Ankünfte erlauben einen Einsatz; Überschüsse bleiben erhalten. Passives Laden dauert 10 Spielsekunden und füllt höchstens einen Einsatz.

Nuklearangriffe haben eine eigene Abklingzeit von 100 Sekunden ab Annahme eines gültigen Befehls, auch in der Warteschlange. Anfangs bereit, ohne normale Einsatzladung zu verbrauchen. Ankunftsboni und 3x-Tempo verkürzen sie nicht. Pause hält sie an; Neue Stadt setzt sie zurück.

Gebäude feuern stufenabhängige Raketen mit begrenzter Verfolgung. Flugzeuge können beschädigt und abgeschossen werden. Größere Gebäude haben mehr Leben und können durch Treffer schrumpfen. Zerstörung gibt Gold und senkt die Stadtentwicklung.

Beim ersten Start gilt die Systemsprache. Der Sprachknopf wechselt zwischen System und neun Sprachen; die Auswahl wird lokal gespeichert. Nicht unterstützte Sprachen nutzen Englisch.

## Steuerung und Einstellungen

Externe Originale unter Assets/polyperfect/Low Poly Epic City/ dürfen nicht verändert werden. Eigene Assets und Adapter verwenden. Store-Assets nicht separat weitergeben.

Grenzen: Unity-Darstellung, Sprachwechsel im Spiel, Web-Bereitstellung und Balance wurden hier nicht geprüft. Kompilierung und unabhängige Tests ersetzen keinen Play-Test.

[HTML](index.html)
