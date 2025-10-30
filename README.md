# IndigoLabs Speech Recognition API

Ta projekt predstavlja preprost RESTful spletni API, ki omogoča prepoznavanje govora iz WAV datotek s pomočjo storitve Azure Cognitive Services Speech to Text. API sprejme avdio datoteko, jo obdela v realnem času in vrne prepoznano besedilo ter zaznan jezik v obliki JSON odgovora prvih 20 sekund WAV datoteke.

## Opis delovanja

Aplikacija omogoča nalaganje WAV datotek preko POST zahtevka na en sam API endpoint. Datoteka se začasno shrani, nato pa se posreduje Azurejevemu SpeechRecognizer objektu, ki prepozna govor in vrne besedilo. Sistem podpira samodejno zaznavanje jezika med angleščino (`en-US`), slovenščino (`sl-SI`), nemščino (`de-DE`) in francoščino (`fr-FR`).

V primeru uspešne prepoznave aplikacija vrne status `200` in JSON objekt, ki vsebuje zaznani jezik ter besedilo. Če prepoznava spodleti ali če naložena datoteka ni ustrezne vrste, se vrne status `400` z opisom napake.

## Tech Stack

Projekt je razvit v **.NET 8.0** okolju in uporablja naslednje tehnologije:
- **ASP.NET Core Web API**
- **Swagger/OpenAPI** za dokumentacijo in testiranje endpointov
- **Azure Cognitive Services Speech SDK** za prepoznavanje govora
- 
## Nastavitve in konfiguracija

V datoteki `appsettings.json` je potrebno dodati ustrezne Azure podatke:

```json
"AzureSpeech": {
  "Key": "YOUR_AZURE_KEY_HERE",
  "Region": "westeurope"
}
```
Ključ veljaven do 27.11.2025 je 'DtBcjTRDCbDH7e4CdKyOCB6VWKcrI4yTi33Ws8gSHwsCF1ksqn2zJQQJ99BJAC5RqLJXJ3w3AAAYACOG2c9D', ali ga pa sami naredite na Azure portalu [Klikni tukaj](https://azure.microsoft.com/en-us/pricing/purchase-options/azure-account?icid=ai-services) .

## Uporaba

1. Zaženite aplikacijo z F5 ali ukazom dotnet run
2. Poiščite endpoint **POST** `/api/speech/recognize`.  
3. Kliknite **Try it out**.  
4. V polje za nalaganje izberite svojo WAV datoteko (največ 25 MB).  
5. Pošljite zahtevek s klikom na **Execute**.  

Če je vse pravilno, bo strežnik vrnil JSON odgovor z zaznanim jezikom in prepoznanim besedilom.

### Primer odgovora (uspešna prepoznava)

```json
{
  "language": "en-US",
  "text": "Hello, this is a recognized test sentence.",
  "success": true,
  "error": ""
}
