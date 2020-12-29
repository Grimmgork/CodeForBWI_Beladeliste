# PayloadDistribution_CodeForBWI

Das Projekt ist eine einfache c# .NET Konsolenanwendung für Windows, die beim Ausführen die geforderten Beladelisten für das Szenario errechnet und anzeigt.
Diese Ordnerstruktur stellt ein VisualStudio Projekt dar.
Die fertige Applikation als .exe befindet sich hier: ~\PayloadDistribution_CodeForBWI\PayloadDistribution\bin\Beladeliste.exe

Der Algorithmus:
1. Es wird nach einer Position gesucht, die mindestens einmal auf dem Transporter Platz findet, und dabei das höchstmögliche Wert/Gewicht Verhältnis besitzt.
   Von dieser Position wird so viel wie möglich zugeladen.
2. Solange noch Nutzlast zur Verfügung steht, wird Punkt 2. wiederholt.
3. Findet sich keine Position, die Platz findet, so wird beim nächsten Transporter wieder bei Punkt 1. angefangen.
4. Der Prozess endet, wenn alle Positionen verladen sind oder Keine Positionen mehr dem letzten Transporter zugeladen werden können.

