using Microsoft.AspNetCore.Rewrite;
using SwanseaCompSci.LabManagementSystem.Core.Application;
using SwanseaCompSci.LabManagementSystem.Core.Application.Allocation;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence;
using SwanseaCompSci.LabManagementSystem.Infrastructure.Persistence;
using SwanseaCompSci.LabManagementSystem.Infrastructure.Shared;
using SwanseaCompSci.LabManagementSystem.Presentation.BlazorServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPresentation(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddApplicationAllocationServices();
builder.Services.AddPersistenceInfrastructure(builder.Configuration);
builder.Services.AddSharedInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRewriter(new RewriteOptions().Add(applyRule =>
{
    if (applyRule.HttpContext.Request.Path == "/MicrosoftIdentity/Account/SignedOut")
    {
        applyRule.HttpContext.Response.Redirect("https://www.swansea.ac.uk/");
    }
}));

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.MapControllers();

app.Services.CreateScope().ServiceProvider.GetRequiredService<IApplicationDbContext>().Migrate();

app.Run();