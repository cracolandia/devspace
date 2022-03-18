var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(setup => 
    setup.AddDefaultPolicy(configure => 
        configure.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
    )
);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors();

app.MapControllers();

app.Run();
