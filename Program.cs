using IndigoLabsAssigment.Services;
using IndigoLabsAssignment.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// when application needs ISpeechRecognitionService,
// create and provide an instance of AzureSpeechRecognitionService
builder.Services.AddScoped<ISpeechRecognitionService, AzureSpeechRecognitionService>();

var app = builder.Build();

// turn on swagger json generator and swagger UI
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();