// Create the ASP.NET Core application builder.
var builder = WebApplication.CreateBuilder(args);


// Add MVC services so the application can use Controllers and Razor Views.
builder.Services.AddControllersWithViews();


// Register IMemoryCache for caching the doctor list in application memory.
builder.Services.AddMemoryCache();


// Register HttpClientFactory so controllers can make HTTP requests
// to external services such as JSONPlaceholder.
builder.Services.AddHttpClient();


// Register response caching so selected controller responses
// can be cached for a specified amount of time.
builder.Services.AddResponseCaching();


// Register Session so temporary user information can be stored
// between multiple HTTP requests.
builder.Services.AddSession(options =>
{
    // Keep session data available for 20 minutes without activity.
    options.IdleTimeout = TimeSpan.FromMinutes(20);

    // Prevent JavaScript from accessing the session cookie.
    options.Cookie.HttpOnly = true;

    // Allow the session cookie to work for normal application requests.
    options.Cookie.IsEssential = true;
});


// Build the application using the configured services.
var app = builder.Build();


// Enable the developer exception page during development.
// This provides useful error information while creating the project.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    // Use the standard error handler outside development.
    app.UseExceptionHandler("/Home/Error");

    // Enable HTTP Strict Transport Security in production.
    app.UseHsts();
}


// Enable response caching middleware.
app.UseResponseCaching();


// Enable HTTPS redirection.
// The launch profile defines HTTPS on port 5001.
app.UseHttpsRedirection();


// Enable serving CSS, JavaScript and other static files from wwwroot.
app.UseStaticFiles();


// Enable routing so MVC can match URLs with controller actions.
app.UseRouting();


// Enable Session before MVC controllers execute.
// This allows controllers to read and write session values.
app.UseSession();


// Enable authorization middleware.
// No authentication or authorization is configured for this project.
app.UseAuthorization();


// Configure the default MVC route.
// Example:
// /Doctor/Details?doctorId=1
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Doctor}/{action=Index}/{id?}");


// Start the web application.
app.Run();