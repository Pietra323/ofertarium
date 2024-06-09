using backend.Data.Repositories;
using backend.Data.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Dodaj usługi do kontenera DI
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

// Skonfiguruj middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();