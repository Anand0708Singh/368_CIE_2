using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using _368_CIE_2.Extensions;
using _368_CIE_2.Models;

namespace _368_CIE_2.Controllers
{
    // Handles doctor information, appointments and external API requests.
    public class DoctorController : Controller
    {
        // Provides access to the application's in-memory cache.
        private readonly IMemoryCache _cache;

        // Provides HttpClient functionality for asynchronous API requests.
        private readonly HttpClient _httpClient;

        // Temporarily stores appointments while the application is running.
        private static readonly List<Appointment> AppointmentList = new();

        // Receives the required services from ASP.NET Core dependency injection.
        public DoctorController(
            IMemoryCache cache,
            IHttpClientFactory httpClientFactory)
        {
            // Store the memory cache service.
            _cache = cache;

            // Create an HttpClient for API communication.
            _httpClient = httpClientFactory.CreateClient();
        }


        // Displays the doctor list.
        public IActionResult Index()
        {
            // Try to retrieve the doctor list from memory cache.
            if (!_cache.TryGetValue("Doctors", out List<Doctor>? doctors) || doctors == null)
            {
                // Create the doctor list when it is not available in cache.
                doctors = GetDoctors();

                // Store the doctor list in memory for three minutes.
                _cache.Set(
                    "Doctors",
                    doctors,
                    TimeSpan.FromMinutes(3));
            }

            // Send the doctor list to the Razor view.
            return View(doctors);
        }


        // Displays details of a selected doctor.
        // DoctorId is received through the query string.
        [ResponseCache(Duration = 60)]
        public IActionResult Details(int doctorId)
        {
            // Get the doctor list from memory cache.
            if (!_cache.TryGetValue("Doctors", out List<Doctor>? doctors) || doctors == null)
            {
                // Create the list when it is not available in cache.
                doctors = GetDoctors();

                // Cache the list for three minutes.
                _cache.Set(
                    "Doctors",
                    doctors,
                    TimeSpan.FromMinutes(3));
            }

            // Find the doctor using the DoctorId from the query string.
            var doctor = doctors.FirstOrDefault(
                d => d.DoctorId == doctorId);

            // Return NotFound when the DoctorId is invalid.
            if (doctor == null)
            {
                return NotFound();
            }

            // Display the selected doctor.
            return View(doctor);
        }


        // Displays the appointment form.
        // DoctorId is passed through the query string.
        [HttpGet]
        public IActionResult Appointment(int doctorId)
        {
            // Get doctors from the memory cache.
            if (!_cache.TryGetValue("Doctors", out List<Doctor>? doctors) || doctors == null)
            {
                // Create the list when it is not cached.
                doctors = GetDoctors();

                // Cache the list for three minutes.
                _cache.Set(
                    "Doctors",
                    doctors,
                    TimeSpan.FromMinutes(3));
            }

            // Find the selected doctor.
            var doctor = doctors.FirstOrDefault(
                d => d.DoctorId == doctorId);

            // Return NotFound for an invalid DoctorId.
            if (doctor == null)
            {
                return NotFound();
            }

            // Send the doctor to the appointment form.
            return View(doctor);
        }


        // Receives the submitted appointment form.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Appointment(
            int doctorId,
            string patientName,
            DateTime appointmentDate)
        {
            // Get the doctors from the memory cache.
            if (!_cache.TryGetValue("Doctors", out List<Doctor>? doctors) || doctors == null)
            {
                // Recreate the list if it is not cached.
                doctors = GetDoctors();

                // Store the list in cache for three minutes.
                _cache.Set(
                    "Doctors",
                    doctors,
                    TimeSpan.FromMinutes(3));
            }

            // Find the doctor submitted through the hidden field.
            var doctor = doctors.FirstOrDefault(
                d => d.DoctorId == doctorId);

            // Stop the request if the DoctorId is invalid.
            if (doctor == null)
            {
                return NotFound();
            }

            // Store the patient's name in a browser cookie.
            Response.Cookies.Append(
                "PatientName",
                patientName);

            // Store the doctor's specialization in Session.
            HttpContext.Session.SetString(
                "Specialization",
                doctor.Specialization);

            // Create a new appointment object.
            var appointment = new Appointment
            {
                PatientName = patientName,
                DoctorId = doctor.DoctorId,
                DoctorName = doctor.DoctorName,
                Specialization = doctor.Specialization,
                AppointmentDate = appointmentDate
            };

            // Add the appointment to the temporary appointment list.
            AppointmentList.Add(appointment);

            // Redirect to the confirmation page.
            return RedirectToAction(
                nameof(Confirmation),
                new
                {
                    doctorId = doctor.DoctorId
                });
        }


        // Displays appointment confirmation information.
        public IActionResult Confirmation(int doctorId)
        {
            // Read the patient name from the browser cookie.
            ViewBag.PatientName =
                Request.Cookies["PatientName"]
                ?? "Unknown Patient";

            // Read the specialization from Session.
            ViewBag.Specialization =
                HttpContext.Session.GetString("Specialization")
                ?? "Not selected";

            // Find the most recent appointment for this doctor.
            var appointment = AppointmentList
                .LastOrDefault(a => a.DoctorId == doctorId);

            // Return NotFound when the appointment does not exist.
            if (appointment == null)
            {
                return NotFound();
            }

            // Send appointment information to the confirmation view.
            return View(appointment);
        }


        // Displays today's and upcoming appointments.
        public IActionResult Appointments()
        {
            // Store today's date for the Razor view.
            ViewBag.Today = DateTime.Today;

            // Sort appointments by appointment date and time.
            var appointments = AppointmentList
                .OrderBy(a => a.AppointmentDate)
                .ToList();

            // Display the appointment list.
            return View(appointments);
        }


        // Retrieves posts asynchronously from JSONPlaceholder.
        public async Task<IActionResult> Posts()
        {
            try
            {
                // Send an asynchronous GET request to JSONPlaceholder.
                var posts = await _httpClient.GetFromJsonAsync<List<Post>>(
                    "https://jsonplaceholder.typicode.com/posts");

                // Send the API results to the Razor view.
                return View(posts ?? new List<Post>());
            }
            catch
            {
                // Store an error message if the external API is unavailable.
                ViewBag.ErrorMessage =
                    "Unable to retrieve posts from the API.";

                // Return an empty list instead of crashing the application.
                return View(new List<Post>());
            }
        }


        // Creates the small hard-coded doctor list used by this practical.
        private List<Doctor> GetDoctors()
        {
            // Return four doctors with simple demonstration data.
            return new List<Doctor>
            {
                new Doctor
                {
                    DoctorId = 1,
                    DoctorName = "Dr. Raj Patel",
                    Specialization = "Cardiology",
                    Experience = 12,
                    ConsultationFee = 800
                },

                new Doctor
                {
                    DoctorId = 2,
                    DoctorName = "Dr. Priya Shah",
                    Specialization = "Dermatology",
                    Experience = 8,
                    ConsultationFee = 600
                },

                new Doctor
                {
                    DoctorId = 3,
                    DoctorName = "Dr. Amit Mehta",
                    Specialization = "Neurology",
                    Experience = 15,
                    ConsultationFee = 1000
                },

                new Doctor
                {
                    DoctorId = 4,
                    DoctorName = "Dr. Neha Joshi",
                    Specialization = "Pediatrics",
                    Experience = 10,
                    ConsultationFee = 700
                }
            };
        }
    }
}

