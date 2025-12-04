using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Syborg_WorkFlow.Api.Model;
using Syborg_WorkFlow.Api.Repositories;
using Syborg_WorkFlow.Api.Service;
using Syborg_WorkFlow.Api.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IWorkflowRepository, WorkflowRepository>();
builder.Services.AddScoped<IValidator<Workflow>, WorkflowValidator>();
builder.Services.AddScoped<WorkflowStepRepository>();
builder.Services.AddScoped<EnterpriseSolutionService>();
builder.Services.AddHttpClient<EnterpriseSolutionService>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<WorkflowValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
