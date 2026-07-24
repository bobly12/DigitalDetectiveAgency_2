using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DigitalDetectiveAgency.Data;
using DigitalDetectiveAgency.Models.Entities;
using DigitalDetectiveAgency.Repositories.Interfaces;
using DigitalDetectiveAgency.Repositories.Implementations;
using DigitalDetectiveAgency.Services.Interfaces;
using DigitalDetectiveAgency.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// ======================================================
// DATABASE
// ======================================================

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// ======================================================
// IDENTITY
// ======================================================

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

// ======================================================
// REPOSITORIES
// ======================================================

builder.Services.AddScoped<ICaseRepository, CaseRepository>();
builder.Services.AddScoped<IBoardRepository, BoardRepository>();
builder.Services.AddScoped<IAccusationRepository, AccusationRepository>();
builder.Services.AddScoped<IEvidenceRepository, EvidenceRepository>();
builder.Services.AddScoped<ISuspectRepository, SuspectRepository>();
builder.Services.AddScoped<IWitnessRepository, WitnessRepository>();

// ======================================================
// APPLICATION SERVICES
// ======================================================

builder.Services.AddScoped<ICaseService, CaseService>();
builder.Services.AddScoped<IBoardService, BoardService>();
builder.Services.AddScoped<IAccusationService, AccusationService>();
builder.Services.AddScoped<IScoringService, ScoringService>();

// NEW SERVICE
builder.Services.AddScoped<IInvestigationProgressService, InvestigationProgressService>();

builder.Services.AddScoped<ICaseAssignmentSyncService, CaseAssignmentSyncService>();

builder.Services.AddScoped<IAdminCaseService, AdminCaseService>();
builder.Services.AddScoped<IAdminEvidenceService, AdminEvidenceService>();
builder.Services.AddScoped<IAdminSuspectService, AdminSuspectService>();
builder.Services.AddScoped<IAdminWitnessService, AdminWitnessService>();

// ======================================================
// BUILD APPLICATION
// ======================================================

var app = builder.Build();

// ======================================================
// SEED ADMIN ROLE
// ======================================================

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    // TEMP ONLY
    var adminUser = await userManager.FindByEmailAsync("your-test-account@example.com");

    if (adminUser != null &&
        !await userManager.IsInRoleAsync(adminUser, "Admin"))
    {
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}

// ======================================================
// HTTP PIPELINE
// ======================================================

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