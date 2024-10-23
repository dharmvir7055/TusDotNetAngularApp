using Microsoft.AspNetCore.Http.Features;
using System.Text;
using tusdotnet;
using tusdotnet.Helpers;
using tusdotnet.Interfaces;
using tusdotnet.Models;
using tusdotnet.Models.Configuration;
using tusdotnet.Stores;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(builder => builder
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin()
        .WithExposedHeaders(CorsHelper.GetExposedHeaders()));
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.Use((context, next) =>
{
   
    context.Features.Get<IHttpMaxRequestBodySizeFeature>().MaxRequestBodySize = null;
    return next.Invoke();
});
app.UseTus(httpContext => new DefaultTusConfiguration
{
    Store = new TusDiskStore(Directory.GetCurrentDirectory()),
    
    UrlPath = "/files",
    Events = new Events
    {
        OnFileCompleteAsync = async eventContext =>
        {
           
            ITusFile file = await eventContext.GetFileAsync();
           
            Dictionary<string, tusdotnet.Models.Metadata> metadata = await file.GetMetadataAsync(eventContext.CancellationToken);
            var result = await DoSomeProcessing(file, metadata, eventContext.CancellationToken).ConfigureAwait(false);
            if (!result)
            {
                //throw new MyProcessingException("Something went wrong during processing");
            }
        }
    }
});
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

 Task<bool> DoSomeProcessing(ITusFile file, Dictionary<string, Metadata> metadata, CancellationToken cancellationToken)
{
    var filename=metadata["filename"].GetString(Encoding.UTF8);//get file name
    System.IO.File.Move(file.Id, filename);//Rename file to original

    return Task.FromResult(true);
}