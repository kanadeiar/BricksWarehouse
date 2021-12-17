
using Blazored.Toast;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureServices(services =>
{
    services.AddDbContext<WarehouseContext>(options => options.UseSqlite( builder.Configuration.GetConnectionString("DefaultConnection") ));

    services.AddScoped<IMapper<ProductType, ProductTypeDto>, DtoMapperService>();
    services.AddScoped<IMapper<ProductTypeDto, ProductType>, DtoMapperService>();
    services.AddScoped<IMapper<Place, PlaceDto>, DtoMapperService>();
    services.AddScoped<IMapper<PlaceDto, Place>, DtoMapperService>();
    services.AddScoped<IMapper<OutTask, OutTaskDto>, DtoMapperService>();
    services.AddScoped<IMapper<OutTaskDto, OutTask>, DtoMapperService>();

    services.AddScoped<IProductTypeData, DatabaseProductTypeData>();
    services.AddScoped<IPlaceData, DatabasePlaceData>();
    services.AddScoped<IOutTaskData, DatabaseOutTaskData>();
    services.AddScoped<FillingsInfoService>();
    services.AddScoped<CountsInfoService>();
    services.AddScoped<TaskDataService>();

    services.AddControllersWithViews().AddRazorRuntimeCompilation();
    services.AddRazorPages().AddRazorRuntimeCompilation();

    services.AddBlazoredToast();
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

app.MapControllerRoute("controllers", "controllers/{controller=Home}/{action=Index}/{id?}");
app.MapDefaultControllerRoute();
app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("online/{param?}", "/_Host");

app.Run();
