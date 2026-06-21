namespace clinic_app_cs;

class Doctor : Person
{
    public Doctor(string firstName, string lastName, string phoneNumber, string specialization, string licenseNumber)
        : base(firstName, lastName, phoneNumber)
    {
        Specialization = specialization;
        LicenseNumber = licenseNumber;
    }

    public string Specialization { get; }
    public string LicenseNumber { get; }

    public override string GetInfo() =>
        $"Lekarz | Specjalizacja: {Specialization} | PWZ: {LicenseNumber}";

    public override string ToString() =>
        $"{base.ToString()} | {GetInfo()}";
}
