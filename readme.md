# ContractChecker

Biblioteka sluzaca do testowania komunikacji pomiedzy mikroserwisami.

### Cel biblioteki

Pozbycie się problemu spowodowanego tym, ze trzeba pamietac o aktualizacje kontraktow pomiedzy serwisami a nie zawsze pamietamy o aktualizacje kontraktów i moze stac sie niezgodne pomiędzy serwisami.

### Opis

Aplikacja działa na systemie health checków. Health check komunikuje się z usługami, które są połączone z główną usługą, a następnie pobiera od nich strukture kontraktów, aby upewnić się, ze struktura kontraktow jest zgodna. Wystarczy ze jeden kontrakt nie jest zgodny i health check zwroci informacje ze stan aplikacji jest unhealthy i zaloguje to w loggerze jako critical.

Jeden health check sprawdza 1 usluge.

Kontrakty pomiedzy uslugami moga posiadac tzw. pola opcjonalne, które mogą nie być brane pod uwage, jesli

### Dostępne health checki

- sprawdzajacy kontrakty pomiedzy mikrouslugami
- sprawdzajacy endpointy oraz zgodnosc metod HTTP - TODO

### Przykładowa konfiguracja

> TODO
