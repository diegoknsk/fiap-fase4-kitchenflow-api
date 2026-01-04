using FastFood.KitchenFlow.Application.Ports;
using FastFood.KitchenFlow.Application.UseCases.DeliveryManagement;
using FastFood.KitchenFlow.Application.UseCases.PreparationManagement;
using FastFood.KitchenFlow.Infra.Persistence;
using FastFood.KitchenFlow.Infra.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(
                "http://localhost:5010",
                "https://localhost:5011",
                "http://localhost:3000",
                "http://localhost:5173",
                "http://localhost:4200"
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// Configure DbContext
builder.Services.AddDbContext<KitchenFlowDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Dependency Injection
builder.Services.AddScoped<IPreparationRepository, PreparationRepository>();
builder.Services.AddScoped<CreatePreparationUseCase>();
builder.Services.AddScoped<GetPreparationsUseCase>();
builder.Services.AddScoped<StartPreparationUseCase>();
builder.Services.AddScoped<FinishPreparationUseCase>();

// Delivery
builder.Services.AddScoped<IDeliveryRepository, DeliveryRepository>();
builder.Services.AddScoped<CreateDeliveryUseCase>();
builder.Services.AddScoped<GetReadyDeliveriesUseCase>();
builder.Services.AddScoped<FinalizeDeliveryUseCase>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
