namespace clinic_app_cs;

class Appointment
{
    private static int _nextId = 1;

    public Appointment(Doctor doctor, Patient patient, DateTime date, string notes = "")
    {
        Id = _nextId++;
        Doctor = doctor;
        Patient = patient;
        Date = date;
        Notes = notes;
        Status = "zaplanowana";
    }

    public int Id { get; }
    public Doctor Doctor { get; }
    public Patient Patient { get; }
    public DateTime Date { get; }
    public string Notes { get; }
    public string Status { get; private set; }

    public void Cancel() => Status = "anulowana";
    public void Complete() => Status = "zakończona";

    public override string ToString()
    {
        var dateStr = Date.ToString("dd-MM-yyyy HH:mm");
        var notesLine = string.IsNullOrEmpty(Notes) ? "" : $"\n  Notatki: {Notes}";
        return $"[{Id}] {dateStr} | {Status.ToUpper()}\n" +
               $"  Lekarz:  {Doctor.FirstName} {Doctor.LastName} ({Doctor.Specialization})\n" +
               $"  Pacjent: {Patient.FirstName} {Patient.LastName} (PESEL: {Patient.Pesel})" +
               notesLine;
    }
}
