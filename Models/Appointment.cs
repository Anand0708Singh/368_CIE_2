using System;

// Represents one patient appointment in the hospital system.
namespace _368_CIE_2.Models
{
    public class Appointment
    {
        // Stores the patient's full name.
        public string PatientName { get; set; } = string.Empty;

        // Stores the selected doctor's ID.
        public int DoctorId { get; set; }

        // Stores the selected doctor's name.
        public string DoctorName { get; set; } = string.Empty;

        // Stores the doctor's specialization.
        public string Specialization { get; set; } = string.Empty;

        // Stores the appointment date and time.
        public DateTime AppointmentDate { get; set; }
    }
}