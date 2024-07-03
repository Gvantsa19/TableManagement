using TMS.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCustomDbContext(builder.Configuration)
                .AddCustomIdentity()
                .AddCustomJwtAuthentication(builder.Configuration)
                .AddCustomAuthorization()
                .AddCustomCors()
                .AddCustomServices()
                .AddCustomValidators()
                .AddCustomMediatR()
                .AddControllers()
                .Services.AddCustomSwagger();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseCors("MyPolicy");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();