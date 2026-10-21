// Provides access to the Doctor model.
using _368_CIE_2.Models;

namespace _368_CIE_2.Extensions;

// Contains extension methods related to the Doctor model.
public static class DoctorExtensions
{
    // Calculates the total consultation charge by adding the fixed ₹100 service charge.
    public static decimal CalculateTotalCharge(this Doctor doctor)
    {
        // Add the ₹100 hospital service charge to the doctor's consultation fee.
        return doctor.ConsultationFee + 100;
    }
}
