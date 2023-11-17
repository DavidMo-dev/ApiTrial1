using ApiTrial1.BS;
using ApiTrial1.Data.Entities;
using ApiTrial1.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(options => {
    options.UseMemberCasing();
}); ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApiTrialDbContext>(options => options.UseSqlServer("name=DefaultConnection"));
builder.Services.TryAddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();

BS.configuration = builder.Configuration;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseAuthorization();

    app.Use(async (context, next) =>
    {
        // Add context token
        if (!String.IsNullOrEmpty(context.Request.Headers["access-token"]))
        {
            context.Items["AccessToken"] = context.Request.Headers["access-token"];
        }
        context.Items["Environment"] = app.Environment;
        await next.Invoke();
    });
}

app.UseAuthorization();

app.MapControllers();

System.Web.HttpContext.Configure(app.Services.GetRequiredService<Microsoft.AspNetCore.Http.IHttpContextAccessor>());


//var bs = new BS();

//var recruiterRole = new ADM_Role(100, "Recruiter");
//var candidateRole = new ADM_Role(200, "Candidate");

//bs.ADM_Role.insert(recruiterRole);
//bs.ADM_Role.insert(candidateRole);

//var recruiter = new ADM_User();

//recruiter.Username = "RecruiterName";
//recruiter.PasswordHash = bs.ADM_User.getPasswordHash(recruiter.Username, "RecruiterPassword");
//recruiter.ADM_Role = recruiterRole;

//var candidate = new ADM_User();

//candidate.Username = "CandidateName";
//candidate.PasswordHash = bs.ADM_User.getPasswordHash(candidate.Username, "CandidatePassword");
//candidate.ADM_Role = candidateRole;

//bs.ADM_User.insert(recruiter);
//bs.ADM_User.insert(candidate);

//bs.save();


app.Run();







