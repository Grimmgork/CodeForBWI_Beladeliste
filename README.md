# PayloadDistribution_CodeForBWI

Das Projekt ist eine einfache c# .NET Konsolenanwendung für Windows, die beim Ausführen die geforderten Beladelisten für das Szenario errechnet und anzeigt.
Dieses Repository stellt ein VisualStudio Projekt dar.
Die fertige Applikation als .exe befindet sich hier: ~\PayloadDistribution_CodeForBWI\PayloadDistribution\bin\Beladeliste.exe

Der Algorithmus:

1. Es wird nach einer Position gesucht, die mindestens einmal auf dem Transporter Platz findet, und dabei das höchstmögliche Wert/Gewicht Verhältnis besitzt.
   Von dieser Position wird so viel wie möglich zugeladen. Steht noch Nutzlast zur verfügung, wird weiter nach einer anderen Position mit möglichst hohem Nutzen/Gewicht Verhältnis    gesucht.
2. Findet keine Position Platz, so wird der nächste Transporter beladen
3. Der Prozess endet, wenn alle Positionen verladen sind oder Keine Positionen mehr dem letzten Transporter zugeladen werden können.

Der Algorithmus ist in der Methode "Distribute" in der Programm.cs Klasse implementiert.



