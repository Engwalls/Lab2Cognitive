### Vad du ska göra

Bygga en applikation som använder sig av Azure AI-komponenter för bilder. 

### Tekniska krav

AI-komponenterna ska anropas via din C#-kod och resultaten hanteras av din C#-kod.

### Funktionella krav

Användaren ska kunna skicka in information som bearbetas av AI-komponenten och sedan presenteras resultatet för användaren.

### Din inlämning

- Skicka in en text-fil som innehåller länken till ett Github-repo med din kod
- Du ska också boka in 15 min med läraren för att demonstrera att din lösning fungerar via skärmdelning.

### Exempel:

1. **Exempel som uppfyller kriterierna för denna labb:** En app där användaren ombeds ladda upp en fil genom att ge en lokal filsökväg, eller en URL, och får reda på vad bilden innehåller, där intressanta objekt taggas. En miniatyrbild ska skapas. Kanske kan användaren själv välja storleken på miniatyren? Eller vad för output-bild, ska den ha bounding boxes? (bygger på 5.1)
2. **Avancerat exempel** (för dig som vill ha en betydligt större utmaning) Bildklassificering Först tränar du en custom vision-modell – kanske på något intresse du har? Sedan bygger du en konsollapp där användaren kan klistra in en URL, eller en filsökväg, och få reda på vad bilden taggats som. Bildklassificering i sig går vi igenom i Microsoft-övningarna, men sedan behöver du hantera att använda C# Prediction SDK:n . 
    
    **Några resurser för exempel 2:**
    
    [www.kaggle.com](http://www.kaggle.com/) – här finns datasets som du kan använda dig av
    
    **[Custom Vision - Using the C# Prediction SDK](https://www.youtube.com/watch?v=wQtZDXJNmUE) (från 2018, ej testad)**
    
    https://github.com/Wintellect/DataScienceExamples/tree/master/Cognitive%20Services/Custom%20Vision/HerbCustomVision (ovanstående exempels kod)
