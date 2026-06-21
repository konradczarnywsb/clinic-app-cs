namespace clinic_app_cs;

class Clinic
{
    public List<Doctor> Doctors { get; } = [];
    public List<Patient> Patients { get; } = [];
    public List<Appointment> Appointments { get; } = [];

    public void AddDoctor(Doctor doctor) => Doctors.Add(doctor);
    public void AddPatient(Patient patient) => Patients.Add(patient);
    public void AddAppointment(Appointment appointment) => Appointments.Add(appointment);

    public Doctor? FindDoctorById(int id) => Doctors.FirstOrDefault(d => d.Id == id);
    public Patient? FindPatientById(int id) => Patients.FirstOrDefault(p => p.Id == id);
}
