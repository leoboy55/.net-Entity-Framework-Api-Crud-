# csharpnetcase

voorwoord

*Dit was mijn eerste .net api die ik zelf heb gemaakt. Zelf heb ik wel paar api calls gedaan met php of javascript. Toch was dit op C# voor de eerste keer. Hiervoor heb ik de .net core ef gebruikt om mijn .net api project op te starten.*

**Wat ging goed?**

- Opzetten van het .net Api framework ging makkelijk.
- De domain class aanmaken ging soepel.
- Sqlite db maken met de tabel addresses ging goed.
- De db connectie is soepel opgezet door middel van EF Core Migration in de command line.
- Controller class aanmaken om de basic api calls op te zetten ging goed, deze maken gebruik van de async Task<Action result> en niet de interface variant.
- Het aanmaken van een Api call extern ging goed.

**Wat kon beter?**

- De logica zitten nu in de controllers en konden achteraf beter in de services class gezet moeten worden. Beter voor de onderhoud.
- Het filter zonder gebruik te maken van een lange if/switch linq statement is niet geluk. Ik kon het helaas niet dinamisch maken, en heb geen externe package voor gebruikt. leg me aub uit hoe jullie dit hadden gedaan daar ben ik erg benieuwd naar.
- Meer comments moeten maken door de code beter uit te leggen, wel bij de Api call controller gedaan.



