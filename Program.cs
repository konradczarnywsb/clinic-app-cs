using clinic_app_cs;

static int InputInt(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        if (int.TryParse(Console.ReadLine(), out int result))
            return result;
        Console.WriteLine("Podaj liczbę całkowitą.");
    }
}

static DateTime InputDate(string prompt)
{
    while (true)
    {
        Console.Write($"{prompt} (DD-MM-RRRR HH:MM): ");
        var raw = Console.ReadLine() ?? "";
        if (DateTime.TryParseExact(raw, "dd-MM-yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out var date))
            return date;
        Console.WriteLine("Nieprawidłowy format daty. Użyj: DD-MM-RRRR HH:MM");
    }
}

static void PrintSeparator() => Console.WriteLine(new string('-', 40));

// --- Lekarze ---

static void MenuAddDoctor(Clinic clinic)
{
    PrintSeparator();
    Console.WriteLine("DODAJ LEKARZA");
    Console.Write("Imię: "); var firstName = Console.ReadLine() ?? "";
    Console.Write("Nazwisko: "); var lastName = Console.ReadLine() ?? "";
    Console.Write("Specjalizacja: "); var specialization = Console.ReadLine() ?? "";
    Console.Write("Numer licencji (PWZ): "); var licenseNumber = Console.ReadLine() ?? "";
    while (true)
    {
        try
        {
            Console.Write("Telefon: "); var phone = Console.ReadLine() ?? "";
            var doctor = new Doctor(firstName, lastName, phone, specialization, licenseNumber);
            clinic.AddDoctor(doctor);
            Console.WriteLine($"Dodano lekarza: {doctor}");
            break;
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Błąd: {e.Message}");
        }
    }
}

static void MenuListDoctors(Clinic clinic)
{
    PrintSeparator();
    Console.WriteLine("LISTA LEKARZY");
    if (clinic.Doctors.Count == 0) { Console.WriteLine("Brak lekarzy w systemie."); return; }
    foreach (var d in clinic.Doctors) Console.WriteLine(d);
}

static void MenuDoctors(Clinic clinic)
{
    while (true)
    {
        PrintSeparator();
        Console.WriteLine("LEKARZE");
        Console.WriteLine("1. Dodaj lekarza");
        Console.WriteLine("2. Lista lekarzy");
        Console.WriteLine("0. Powrót");
        Console.Write("Wybór: ");
        switch (Console.ReadLine()?.Trim())
        {
            case "1": MenuAddDoctor(clinic); break;
            case "2": MenuListDoctors(clinic); break;
            case "0": return;
            default: Console.WriteLine("Nieznana opcja."); break;
        }
    }
}

// --- Pacjenci ---

static void MenuAddPatient(Clinic clinic)
{
    PrintSeparator();
    Console.WriteLine("DODAJ PACJENTA");
    Console.Write("Imię: "); var firstName = Console.ReadLine() ?? "";
    Console.Write("Nazwisko: "); var lastName = Console.ReadLine() ?? "";
    string dateOfBirth;
    while (true)
    {
        Console.Write("Data urodzenia (DD-MM-RRRR): ");
        dateOfBirth = Console.ReadLine() ?? "";
        if (DateTime.TryParseExact(dateOfBirth, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out _))
            break;
        Console.WriteLine("Nieprawidłowy format daty. Użyj: DD-MM-RRRR");
    }
    while (true)
    {
        try
        {
            Console.Write("Telefon: "); var phone = Console.ReadLine() ?? "";
            Console.Write("PESEL: "); var pesel = Console.ReadLine() ?? "";
            var patient = new Patient(firstName, lastName, phone, pesel, dateOfBirth);
            clinic.AddPatient(patient);
            Console.WriteLine($"Dodano pacjenta: {patient}");
            break;
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Błąd: {e.Message}");
        }
    }
}

static void MenuListPatients(Clinic clinic)
{
    PrintSeparator();
    Console.WriteLine("LISTA PACJENTÓW");
    if (clinic.Patients.Count == 0) { Console.WriteLine("Brak pacjentów w systemie."); return; }
    foreach (var p in clinic.Patients) Console.WriteLine(p);
}

static void MenuPatients(Clinic clinic)
{
    while (true)
    {
        PrintSeparator();
        Console.WriteLine("PACJENCI");
        Console.WriteLine("1. Dodaj pacjenta");
        Console.WriteLine("2. Lista pacjentów");
        Console.WriteLine("0. Powrót");
        Console.Write("Wybór: ");
        switch (Console.ReadLine()?.Trim())
        {
            case "1": MenuAddPatient(clinic); break;
            case "2": MenuListPatients(clinic); break;
            case "0": return;
            default: Console.WriteLine("Nieznana opcja."); break;
        }
    }
}

// --- Wizyty ---

static void MenuAddAppointment(Clinic clinic)
{
    PrintSeparator();
    Console.WriteLine("REJESTRACJA WIZYTY");

    if (clinic.Doctors.Count == 0) { Console.WriteLine("Brak lekarzy w systemie. Najpierw dodaj lekarza."); return; }
    if (clinic.Patients.Count == 0) { Console.WriteLine("Brak pacjentów w systemie. Najpierw dodaj pacjenta."); return; }

    Console.WriteLine("Dostępni lekarze:");
    foreach (var d in clinic.Doctors) Console.WriteLine($"  {d}");
    var doctorId = InputInt("ID lekarza: ");
    var doctor = clinic.FindDoctorById(doctorId);
    if (doctor == null) { Console.WriteLine("Nie znaleziono lekarza o podanym ID."); return; }

    Console.WriteLine("Dostępni pacjenci:");
    foreach (var p in clinic.Patients) Console.WriteLine($"  {p}");
    var patientId = InputInt("ID pacjenta: ");
    var patient = clinic.FindPatientById(patientId);
    if (patient == null) { Console.WriteLine("Nie znaleziono pacjenta o podanym ID."); return; }

    var date = InputDate("Data i godzina wizyty");
    Console.Write("Notatki (opcjonalnie, Enter aby pominąć): ");
    var notes = Console.ReadLine()?.Trim() ?? "";

    var appointment = new Appointment(doctor, patient, date, notes);
    clinic.AddAppointment(appointment);
    Console.WriteLine($"Zarejestrowano wizytę:\n{appointment}");
}

static void MenuListAppointments(Clinic clinic)
{
    PrintSeparator();
    Console.WriteLine("LISTA WIZYT");
    if (clinic.Appointments.Count == 0) { Console.WriteLine("Brak wizyt w systemie."); return; }
    foreach (var a in clinic.Appointments) { Console.WriteLine(a); Console.WriteLine(); }
}

static void MenuAppointments(Clinic clinic)
{
    while (true)
    {
        PrintSeparator();
        Console.WriteLine("WIZYTY");
        Console.WriteLine("1. Zarejestruj wizytę");
        Console.WriteLine("2. Lista wizyt");
        Console.WriteLine("0. Powrót");
        Console.Write("Wybór: ");
        switch (Console.ReadLine()?.Trim())
        {
            case "1": MenuAddAppointment(clinic); break;
            case "2": MenuListAppointments(clinic); break;
            case "0": return;
            default: Console.WriteLine("Nieznana opcja."); break;
        }
    }
}

// --- Wszyscy w systemie (polimorfizm) ---

static void MenuAllPersons(Clinic clinic)
{
    PrintSeparator();
    Console.WriteLine("WSZYSCY W SYSTEMIE");
    var persons = clinic.Doctors.Cast<Person>().Concat(clinic.Patients).ToList();
    if (persons.Count == 0) { Console.WriteLine("Brak osób w systemie."); return; }
    foreach (var person in persons)
        Console.WriteLine($"{person.FirstName} {person.LastName} -> {person.GetInfo()}");
}

// --- Menu główne ---

var clinic = new Clinic();
while (true)
{
    PrintSeparator();
    Console.WriteLine("PRZYCHODNIA — MENU GŁÓWNE");
    Console.WriteLine("1. Lekarze");
    Console.WriteLine("2. Pacjenci");
    Console.WriteLine("3. Wizyty");
    Console.WriteLine("4. Wszyscy w systemie");
    Console.WriteLine("0. Wyjście");
    Console.Write("Wybór: ");
    switch (Console.ReadLine()?.Trim())
    {
        case "1": MenuDoctors(clinic); break;
        case "2": MenuPatients(clinic); break;
        case "3": MenuAppointments(clinic); break;
        case "4": MenuAllPersons(clinic); break;
        case "0": Console.WriteLine("Do widzenia!"); return;
        default: Console.WriteLine("Nieznana opcja."); break;
    }
}
