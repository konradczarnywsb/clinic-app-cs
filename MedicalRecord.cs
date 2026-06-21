namespace clinic_app_cs;

class MedicalRecord
{
    private readonly List<string> _notes = [];

    public MedicalRecord(int patientId)
    {
        PatientId = patientId;
    }

    public int PatientId { get; }

    public void AddNote(string note)
    {
        if (string.IsNullOrWhiteSpace(note))
            throw new ArgumentException("Notatka nie może być pusta.");
        _notes.Add(note.Trim());
    }

    public IReadOnlyList<string> GetNotes() => _notes.AsReadOnly();

    public override string ToString()
    {
        if (_notes.Count == 0)
            return "Brak notatek w karcie pacjenta.";
        var lines = string.Join("\n  ", _notes.Select((n, i) => $"{i + 1}. {n}"));
        return $"Karta pacjenta [{PatientId}]:\n  {lines}";
    }
}
