# Aplikacja Przychodni

Konsolowa aplikacja w C# do zarządzania przychodnią. Umożliwia dodawanie lekarzy i pacjentów oraz rejestrację wizyt.

## Uruchomienie

```bash
dotnet run
```

Wymagany .NET 10.0+.

## Struktura plików

```
clinic-app-cs/
├── Person.cs          # abstrakcyjna klasa bazowa Person
├── Doctor.cs          # klasa Doctor (dziedziczy po Person)
├── Patient.cs         # klasa Patient (dziedziczy po Person)
├── MedicalRecord.cs   # klasa MedicalRecord (kompozycja z Patient)
├── Appointment.cs     # klasa Appointment (wizyta)
├── Clinic.cs          # klasa Clinic (rejestr danych)
└── Program.cs         # menu konsolowe i punkt wejścia
```

## Struktura klas

### `Person` (Person.cs) — klasa abstrakcyjna

Abstrakcyjna klasa bazowa dla wszystkich osób w systemie. Nie można tworzyć jej obiektów bezpośrednio. Definiuje kontrakt `GetInfo()`, który muszą zrealizować klasy pochodne.

| Pole | Typ | Dostęp | Opis |
|------|-----|--------|------|
| `Id` | `int` | tylko getter | Unikalny identyfikator, nadawany automatycznie |
| `FirstName` | `string` | tylko getter | Imię |
| `LastName` | `string` | tylko getter | Nazwisko |
| `PhoneNumber` | `string` | getter + setter | Numer telefonu — setter waliduje, czy nie jest pusty |

Metody:
- `GetInfo()` — metoda abstrakcyjna, zaimplementowana inaczej w każdej klasie pochodnej (polimorfizm)
- `ToString()` — zwraca `"[Id] Imię Nazwisko (Telefon)"`

---

### `Doctor : Person` (Doctor.cs)

Rozszerza `Person` o dane specyficzne dla lekarza.

| Pole | Typ | Dostęp | Opis |
|------|-----|--------|------|
| `Specialization` | `string` | tylko getter | Specjalizacja medyczna |
| `LicenseNumber` | `string` | tylko getter | Numer licencji (PWZ) |

Metody:
- `GetInfo()` → `"Lekarz | Specjalizacja: X | PWZ: Y"`

---

### `Patient : Person` (Patient.cs)

Rozszerza `Person` o dane specyficzne dla pacjenta. W konstruktorze tworzy obiekt `MedicalRecord` (kompozycja).

| Pole | Typ | Dostęp | Opis |
|------|-----|--------|------|
| `Pesel` | `string` | getter + setter | PESEL — setter waliduje dokładnie 11 cyfr |
| `DateOfBirth` | `string` | tylko getter | Data urodzenia (format DD-MM-RRRR) |
| `MedicalRecord` | `MedicalRecord` | tylko getter | Karta pacjenta tworzona automatycznie przy rejestracji |

Metody:
- `GetInfo()` → `"Pacjent | PESEL: X | Data ur.: Y"`

---

### `MedicalRecord` (MedicalRecord.cs)

Karta pacjenta — tworzona automatycznie wewnątrz konstruktora `Patient` (relacja kompozycji, cykl życia powiązany z pacjentem).

| Pole | Typ | Dostęp | Opis |
|------|-----|--------|------|
| `PatientId` | `int` | tylko getter | ID pacjenta, do którego należy karta |
| `_notes` | `List<string>` | brak bezpośredniego | Lista notatek medycznych |

Metody:
- `AddNote(note)` — dodaje notatkę, waliduje czy nie jest pusta
- `GetNotes()` — zwraca `IReadOnlyList<string>` z listą notatek

---

### `Appointment` (Appointment.cs)

Reprezentuje wizytę lekarską. Przyjmuje obiekty `Doctor` i `Patient` jako parametry konstruktora (relacja agregacji).

| Pole | Typ | Dostęp | Opis |
|------|-----|--------|------|
| `Id` | `int` | tylko getter | Unikalny identyfikator, nadawany automatycznie |
| `Doctor` | `Doctor` | tylko getter | Lekarz prowadzący wizytę |
| `Patient` | `Patient` | tylko getter | Pacjent |
| `Date` | `DateTime` | tylko getter | Data i godzina wizyty |
| `Notes` | `string` | tylko getter | Notatki (opcjonalne) |
| `Status` | `string` | `private set` | Status wizyty — zmieniany wyłącznie przez metody |

Metody:
- `Cancel()` — zmienia status na `"anulowana"`
- `Complete()` — zmienia status na `"zakończona"`

---

### `Clinic` (Clinic.cs)

Centralny rejestr danych aplikacji. Przechowuje kolekcje lekarzy, pacjentów i wizyt (relacja agregacji).

| Metoda | Opis |
|--------|------|
| `AddDoctor(doctor)` | Dodaje lekarza do rejestru |
| `AddPatient(patient)` | Dodaje pacjenta do rejestru |
| `AddAppointment(appointment)` | Dodaje wizytę do rejestru |
| `FindDoctorById(id)` | Zwraca `Doctor?` o podanym ID lub `null` |
| `FindPatientById(id)` | Zwraca `Patient?` o podanym ID lub `null` |

---

## Diagram dziedziczenia i relacji

```
              Person (abstract)
             /                 \
         Doctor              Patient ──kompozycja──> MedicalRecord
              \              /
               Appointment  (agregacja: przyjmuje Doctor i Patient)

Clinic  (agregacja: przechowuje listy Doctor[], Patient[], Appointment[])
```

### Relacje między klasami

| Relacja | Między | Opis |
|---------|--------|------|
| Dziedziczenie | `Person → Doctor`, `Person → Patient` | Doctor i Patient dziedziczą pola i kontrakt Person |
| Kompozycja | `Patient → MedicalRecord` | MedicalRecord tworzony w konstruktorze Patient, nieodłączny od pacjenta |
| Agregacja | `Appointment → Doctor, Patient` | Wizyta przechowuje referencje do niezależnie istniejących obiektów |
| Kolekcja | `Clinic → Doctor[], Patient[], Appointment[]` | Clinic zarządza listami obiektów |
| Abstrakcja | `Person (abstract) → Doctor, Patient` | Person definiuje kontrakt `GetInfo()`, klasy pochodne go realizują |

---

## Zasady OOP zastosowane w projekcie

### Enkapsulacja
Pola są ukryte za właściwościami C# (`{ get; }`, `{ get; private set; }`):
- Pola tylko do odczytu mają wyłącznie getter (np. `Id`, `FirstName`)
- Pola z walidacją mają getter i setter (np. `PhoneNumber` — sprawdza niepustość, `Pesel` — sprawdza 11 cyfr)
- Status wizyty można zmienić wyłącznie przez metody `Cancel()` i `Complete()` (`private set`)

### Abstrakcja
`Person` jest klasą `abstract` i deklaruje abstrakcyjną metodę `GetInfo()`. Nie można utworzyć obiektu klasy `Person` bezpośrednio — wymusza to implementację kontraktu w klasach pochodnych.

### Polimorfizm
Metoda `GetInfo()` jest wywoływana na liście obiektów typu `Person`, ale każdy typ wykonuje ją inaczej:
```csharp
var persons = clinic.Doctors.Cast<Person>().Concat(clinic.Patients).ToList();
foreach (var person in persons)
    Console.WriteLine($"{person.FirstName} {person.LastName} -> {person.GetInfo()}");

// Doctor:  "Lekarz | Specjalizacja: Kardiologia | PWZ: 12345"
// Patient: "Pacjent | PESEL: 12345678901 | Data ur.: 15-03-1990"
```

---

## Działanie programu

Po uruchomieniu wyświetla się menu główne:

```
PRZYCHODNIA — MENU GŁÓWNE
1. Lekarze
2. Pacjenci
3. Wizyty
4. Wszyscy w systemie
0. Wyjście
```

### Lekarze
- **Dodaj lekarza** — wprowadzenie imienia, nazwiska, specjalizacji, numeru licencji i telefonu
- **Lista lekarzy** — wyświetlenie wszystkich lekarzy w systemie

### Pacjenci
- **Dodaj pacjenta** — wprowadzenie imienia, nazwiska, daty urodzenia, telefonu i numeru PESEL (walidowany)
- **Lista pacjentów** — wyświetlenie wszystkich pacjentów w systemie

### Wizyty
- **Zarejestruj wizytę** — wybór lekarza i pacjenta z listy (po ID), podanie daty i opcjonalnych notatek
- **Lista wizyt** — wyświetlenie wszystkich zarejestrowanych wizyt ze statusem

### Wszyscy w systemie
Demonstracja polimorfizmu — wyświetla wszystkich lekarzy i pacjentów, wywołując `GetInfo()` na każdym obiekcie. Ta sama metoda zwraca różne informacje w zależności od typu obiektu.

### Formaty dat

| Pole | Format | Przykład |
|------|--------|---------|
| Data urodzenia pacjenta | `DD-MM-RRRR` | `15-03-1990` |
| Data i godzina wizyty | `DD-MM-RRRR HH:MM` | `20-06-2026 10:30` |

### Walidacja danych

| Pole | Reguła |
|------|--------|
| Telefon | Nie może być pusty |
| PESEL | Dokładnie 11 cyfr |
| Daty | Muszą być w podanym formacie |

## Użycie AI

Stworzenie dokumentacji na podstawie analizy kodu
