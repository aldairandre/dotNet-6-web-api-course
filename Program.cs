using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration["Database:SqlServer"]);

var app = builder.Build();
var configuration = app.Configuration;
ProductRepository.Init(configuration);

app.MapGet("/", () => "Hello World!");

//Passing params via endpoint

//There are 3 way to pass params via URL

//1--Query
//api.app.com/user/datestart={date}&dateend={date}

app.MapGet("products",([FromQuery] string dateStart,[FromQuery] string dateEnd) => {
  return $"{dateStart} - {dateEnd}";
});

app.MapPost("/products",(ProductRequest productRequest,ApplicationDbContext context) => {
  var category = context.Categories.Where(c => c.Id == productRequest.CategoryId).First();
  var product = new Product {
    Code = productRequest.Code,
    Name = productRequest.Name,
    Description = productRequest.Description,
    Category = category
  };
  if(productRequest.Tags != null){
    product.Tags = new List<Tag>();
    foreach (var item in productRequest.Tags)
    {
      product.Tags.Add(new Tag { Name = item});
    }
  }
  context.Add(product);
  context.SaveChanges();
  return Results.Created($"/products/{product.Id}",product.Id);
}); 

//2--Route
//api.app.com/user/{code}

app.MapGet("products/{id}",( [FromRoute] int id,ApplicationDbContext context) => {
  var product = context.Products
    .Include(p => p.Category)
    .Include(p => p.Tags)
    .Where(p => p.Id == id).First();
  if (product != null) 
    return Results.Ok(product);
  return Results.NotFound();
});

app.MapPut("/products/{id}",([FromRoute] int id,[FromBody] ProductRequest productRequest,ApplicationDbContext context) => {
  var product = context.Products
    .Include(p => p.Category)
    .Include(p => p.Tags)
    .Where(p => p.Id == id).First();
  var category = context.Categories.Where(c => c.Id == productRequest.CategoryId).First();

  product.Code = productRequest.Code;
  product.Name = productRequest.Name;
  product.Description = productRequest.Description;
  product.Category = category;
  product.Tags = new List<Tag>();

  if(productRequest.Tags != null){
    product.Tags = new List<Tag>();
    foreach (var item in productRequest.Tags)
    {
      product.Tags.Add(new Tag { Name = item});
    }
  }
  context.SaveChanges();
  return Results.Ok();
  
});

app.MapDelete("/products/{id}",([FromRoute] int id,ApplicationDbContext context) => {
  var product = context.Products.Where(p => p.Id == id).First();
  if (product != null){
    context.Products.Remove(product);
    context.SaveChanges();
  }
  return Results.Ok();
});

//Using configuration file to show my database string configuration

app.MapGet("/config/database", (IConfiguration configuration)=> {
  return Results.Ok($"{configuration["Database:connection"]}/{configuration["Database:port"]}");
});

app.Run();
