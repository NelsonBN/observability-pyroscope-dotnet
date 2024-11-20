using System;
using System.Threading;
using Demo.Api;
using Demo.Api.Domain;
using Demo.Api.DTOs;
using Demo.Api.Infrastructure;
using Demo.Api.UseCases;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.DependencyInjection;


ContinuousProfiling.Setup();


var builder = WebApplication.CreateSlimBuilder(args);


builder.Services
    .AddTransient<GetUsersQuery>()
    .AddTransient<GetUserQuery>()
    .AddTransient<CreateUserCommand>()
    .AddTransient<UpdateUserCommand>()
    .AddTransient<DeleteUserCommand>();

builder.Services
    .AddSingleton<IUsersRepository, UsersRepository>();

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

builder.Services
    .Configure<RouteOptions>(options
        => options.ConstraintMap["regex"] = typeof(RegexInlineRouteConstraint));



var app = builder.Build();

app.UseSwagger()
   .UseSwaggerUI();

app.UseRouting();



app.MapGet("", (GetUsersQuery query, CancellationToken cancellationToken) =>
{
    var response = query.HandleAsync(cancellationToken);
    return Results.Ok(response);
}).WithOpenApi();

app.MapGet("{id:guid}", (GetUserQuery query, Guid id, CancellationToken cancellationToken) =>
{
    try
    {
        var response = query.HandleAsync(id, cancellationToken);
        return Results.Ok(response);
    }
    catch(UserNotFoundException exception)
    {
        return Results.NotFound(exception.Message);
    }
}).WithName("GetUser").WithOpenApi();

app.MapPost("", (CreateUserCommand command, UserRequest request, CancellationToken cancellationToken) =>
{
    var id = command.HandleAsync(request, cancellationToken);
    return Results.CreatedAtRoute(
        "GetUser",
        new { id },
        id);
}).WithOpenApi();

app.MapPut("{id:guid}", (UpdateUserCommand command, Guid id, UserRequest request, CancellationToken cancellationToken) =>
{
    try
    {
        command.HandleAsync(id, request, cancellationToken);
        return Results.NoContent();
    }
    catch(UserNotFoundException exception)
    {
        return Results.NotFound(exception.Message);
    }
}).WithOpenApi();


app.MapDelete("{id:guid}", (DeleteUserCommand command, Guid id, CancellationToken cancellationToken) =>
{
    try
    {
        command.HandleAsync(id, cancellationToken);
        return Results.NoContent();
    }
    catch(UserNotFoundException exception)
    {
        return Results.NotFound(exception.Message);
    }
}).WithOpenApi();


await app.RunAsync();
