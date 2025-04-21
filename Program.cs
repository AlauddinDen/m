using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebApplication1.DTOs;
using WebApplication1.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>
    (op => op.UseSqlServer(builder.Configuration.GetConnectionString("con")));


var app = builder.Build();

app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//get
app.MapGet("/api/stu", ([FromServices] AppDbContext db) =>
{
    return db.Students.Include(a => a.Addresses).ToList();
}).WithName("GetStu").Produces<Student[]>(StatusCodes.Status200OK);

//delete
app.MapDelete("/api/stu/{id}", ([FromRoute] int id, [FromServices] AppDbContext db) =>
{
    var s = db.Students.Include(a => a.Addresses).FirstOrDefault(x => x.Id == id);
    db.Students.Remove(s);
    db.SaveChanges();
    return (s);

}).WithOpenApi().Produces(StatusCodes.Status204NoContent);

//post
app.MapPost("/api/stu", ([FromBody] StudentDto studentDto, [FromServices] AppDbContext db) =>
{
    var imageUrl = string.IsNullOrEmpty(studentDto.BaseImage64) ? null : studentDto.BaseImage64;

    var addresses = string.IsNullOrWhiteSpace(studentDto.AddressJson) ?
    new List<AddressDto>() : JsonSerializer.Deserialize<List<AddressDto>>(studentDto.AddressJson);


    var addStudent = new Student
    {
        Name = studentDto.Name,
        AdmissionDate = studentDto.AdmissionDate,
        IsActive = studentDto.IsActive,
        ImageUrl = imageUrl
    };
    db.Students.Add(addStudent);
    db.SaveChanges();

    var newStudent = db.Students.FirstOrDefault(s => s.Name == studentDto.Name);
    if (newStudent != null && addresses != null)
    {
        foreach (var a in addresses)
        {
            var address = new Address
            {
                StudentId = newStudent.Id,
                City = a.City,
                Street = a.Street
            };
            db.Addresses.Add(address);

        }
        db.SaveChanges();
    }
    return newStudent;

}).WithOpenApi().Produces<Student>(StatusCodes.Status201Created);




app.Run();

