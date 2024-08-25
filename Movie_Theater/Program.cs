using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Movie_Theater.Data;
using Movie_Theater.Hubs;
using Movie_Theater.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
//builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<Movie_TheaterContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Movie")));

// ���USignalR��ѪA��
builder.Services.AddSignalR();
//���\�e��
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("https://localhost:7176") // �������A���e�����ε{���� URL
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});

builder.Services.AddSession(options =>
{
    // �]�wSession�W��
    options.Cookie.Name = ".UserLogin.Session";
    // �]�w�۹�O�ɮɶ�(����)
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    // ��o�ӬO���n��
    options.Cookie.IsEssential = true;
    // ���i�H��JS���oCookie
    options.Cookie.HttpOnly = true;
    // ���w�u���HTTPS�ǰe
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// ���USession�������O
builder.Services.AddControllersWithViews(options =>
{
    //options.Filters.Add<SessionCheckFilter>(); // �������ιL�o��
});

// ���UHttpContext.Session(��Razor���i�H���oSession)
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
	//pattern: "{controller=Login}/{action=Login}/{id?}");
app.MapRazorPages();
app.MapHub<ChatHub>("/chathub"); // �t�m SignalR Hub �����I
app.Run();
