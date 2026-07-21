using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DigitalDetectiveAgency.Data;
using DigitalDetectiveAgency.Models.Entities;
using DigitalDetectiveAgency.Repositories.Interfaces;
using DigitalDetectiveAgency.Repositories.Implementations;
using DigitalDetectiveAgency.Services.Interfaces;
using DigitalDetectiveAgency.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// 1. ADD SERVICES TO CONTAINER FIRST
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// We combine the Identity setup into ONE call and include AddRoles here.
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()          
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

// Register Repositories and Services
builder.Services.AddScoped<ICaseRepository, CaseRepository>();
builder.Services.AddScoped<ICaseService, CaseService>();
builder.Services.AddScoped<IBoardRepository, BoardRepository>();
builder.Services.AddScoped<IBoardService, BoardService>();
builder.Services.AddScoped<IAccusationRepository, AccusationRepository>();
builder.Services.AddScoped<IAccusationService, AccusationService>();
builder.Services.AddScoped<IScoringService, ScoringService>();
builder.Services.AddScoped<IEvidenceRepository, EvidenceRepository>();
builder.Services.AddScoped<ISuspectRepository, SuspectRepository>();
builder.Services.AddScoped<IWitnessRepository, WitnessRepository>();
builder.Services.AddScoped<ICaseAssignmentSyncService, CaseAssignmentSyncService>();
builder.Services.AddScoped<IAdminCaseService, AdminCaseService>();
builder.Services.AddScoped<IAdminEvidenceService, AdminEvidenceService>();
builder.Services.AddScoped<IAdminSuspectService, AdminSuspectService>();
builder.Services.AddScoped<IAdminWitnessService, AdminWitnessService>();

// 2. BUILD THE APP
var app = builder.Build();

// 3. NOW WE CAN USE 'app' FOR SEEDING
// Seed the Admin role and promote a test account (one-time dev convenience)
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    // TEMP: replace with your own test account email, then remove this block after first run
    var adminUser = await userManager.FindByEmailAsync("your-test-account@example.com");
    if (adminUser != null && !await userManager.IsInRoleAsync(adminUser, "Admin"))
    {
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}

// 4. CONFIGURE THE HTTP REQUEST PIPELINE
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

// Authentication MUST be registered before Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
    .WithStaticAssets();

app.Run();