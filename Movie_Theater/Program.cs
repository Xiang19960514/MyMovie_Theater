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

// 註冊SignalR聊天服務
builder.Services.AddSignalR();
//允許前端
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("https://localhost:7176") // 替換為你的前端應用程式的 URL
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});

builder.Services.AddSession(options =>
{
    // 設定Session名稱
    options.Cookie.Name = ".UserLogin.Session";
    // 設定相對逾時時間(分鐘)
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    // 表這個是必要的
    options.Cookie.IsEssential = true;
    // 不可以用JS取得Cookie
    options.Cookie.HttpOnly = true;
    // 限定只能用HTTPS傳送
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// 註冊Session驗證類別
builder.Services.AddControllersWithViews(options =>
{
    //options.Filters.Add<SessionCheckFilter>(); // 全局應用過濾器
});

// 註冊HttpContext.Session(讓Razor文件可以取得Session)
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
app.MapHub<ChatHub>("/chathub"); // 配置 SignalR Hub 的端點
app.Run();
