namespace clinic_app_cs;

abstract class Person
{
    private static int _nextId = 1;
    private string _phoneNumber = "";

    protected Person(string firstName, string lastName, string phoneNumber)
    {
        Id = _nextId++;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
    }

    public int Id { get; }
    public string FirstName { get; }
    public string LastName { get; }

    public string PhoneNumber
    {
        get => _phoneNumber;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Numer telefonu nie może być pusty.");
            _phoneNumber = value.Trim();
        }
    }

    public abstract string GetInfo();

    public override string ToString() =>
        $"[{Id}] {FirstName} {LastName} ({PhoneNumber})";
}
