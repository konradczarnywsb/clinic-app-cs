namespace clinic_app_cs;

class Patient : Person
{
    private string _pesel = "";

    public Patient(string firstName, string lastName, string phoneNumber, string pesel, string dateOfBirth)
        : base(firstName, lastName, phoneNumber)
    {
        Pesel = pesel;
        DateOfBirth = dateOfBirth;
        MedicalRecord = new MedicalRecord(Id);
    }

    public string Pesel
    {
        get => _pesel;
        set
        {
            if (value.Length != 11 || !value.All(char.IsDigit))
                throw new ArgumentException("PESEL musi składać się z dokładnie 11 cyfr.");
            _pesel = value;
        }
    }

    public string DateOfBirth { get; }
    public MedicalRecord MedicalRecord { get; }

    public override string GetInfo() =>
        $"Pacjent | PESEL: {Pesel} | Data ur.: {DateOfBirth}";

    public override string ToString() =>
        $"{base.ToString()} | {GetInfo()}";
}
