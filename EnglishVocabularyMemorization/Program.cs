using EnglishVocabularyMemorization;
using EnglishVocabularyMemorization.Services;
using EnglishVocabularyMemorization.Services.ChatGpt.Helpers;
using EnglishVocabularyMemorization.Services.ChatGpt.Interfaces;
using EnglishVocabularyMemorization.Services.ChatGpt.Services;
using EnglishVocabularyMemorization.Services.Wordup;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddScoped<IChatGPTService, ChatGPTService>();
builder.Services.AddScoped<IWordupService, WordupService>();
builder.Services.AddScoped<ISpacedRepetitionService, SpacedRepetitionService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ChatGPTHelper>();
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

builder.Services.AddDbContextFactory<ApplicationContext>(options => options.UseMySql(builder.Configuration.GetConnectionString("Default"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("Default"))));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("corsapp");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
