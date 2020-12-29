# PayloadDistribution_CodeForBWI

Das Projekt ist eine einfache c# .NET Konsolenanwendung, die beim Ausführen die geforderten Beladelisten für das Szenario errechnet und anzeigt.

Der Algorithmus:
1. Es wird nach einer Position gesucht, die mindestens einmal auf dem Transporter Platz findet, und dabei das höchstmögliche Wert/Gewicht Verhältnis besitzt.
   Von dieser Position wird so viel wie möglich zugeladen.
2. Solange noch Nutzlast zur Verfügung steht, wird Punkt 2. wiederholt.
3. Findet sich keine Position, die PLatz findet, so wird der Ablauf für den nächsten Transporter wiederholt.
4. Der Prozess endet, wenn alle Positionen verladen sind oder Keine Positionen mehr dem letzten Transporter zugeladen werden können

