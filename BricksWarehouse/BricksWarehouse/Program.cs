


var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureServices(services =>
{
    services.AddDbContext<WarehouseContext>(options => options.UseSqlite( builder.Configuration.GetConnectionString("DefaultConnection") ));

    services.AddScoped<IMapper<ProductTypeEditWebModel>, WebMapperService>();
    services.AddScoped<IMapper<ProductType>, WebMapperService>();

    services.AddScoped<IProductTypeData, DatabaseProductTypeData>();
    services.AddScoped<IPlaceData, DatabasePlaceData>();

    services.AddControllersWithViews().AddRazorRuntimeCompilation();
});
builder.Services.AddServerSideBlazor();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedData.SeedWarehouseContextTestData(services);
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStatusCodePagesWithRedirects("~/home/error/{0}");

app.UseStaticFiles();

app.UseRouting();

app.MapDefaultControllerRoute();
app.MapBlazorHub();

app.Run();
