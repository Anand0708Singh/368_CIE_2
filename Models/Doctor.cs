namespace _368_CIE_2.Models;

// Represents a doctor in the hospital appointment system.
public class Doctor
{
    // Stores the unique ID of the doctor.
    public int DoctorId { get; set; }

    // Stores the doctor's full name.
    public string DoctorName { get; set; } = string.Empty;

    // Stores the doctor's medical specialization.
    public string Specialization { get; set; } = string.Empty;

    // Stores the doctor's years of professional experience.
    public int Experience { get; set; }

    // Stores the doctor's consultation fee in Indian Rupees.
    public decimal ConsultationFee { get; set; }
}