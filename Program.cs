using Microsoft.Extensions.Options;
using MongoDB.Driver;
using teeze.Models;
using teeze.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.Configure<OnlineStoreDB>(
    builder.Configuration.GetSection(nameof(OnlineStoreDB)));

builder.Services.AddSingleton<IOnlineStoreDB>(sp =>
    sp.GetRequiredService<IOptions<OnlineStoreDB>>().Value);

builder.Services.AddSingleton<IMongoClient>(s =>
new MongoClient(builder.Configuration.GetValue<string>("OnlineStoreDB:ConnectionString")));

builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IProductServices, ProductServices>();
builder.Services.AddScoped<ICartServices, CartServices>();
builder.Services.AddScoped<IWishlistServices, WishlistServices>();
builder.Services.AddScoped<SignInModel>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.Urls.Add("http://192.168.0.104:7193");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
    app.UseHsts();  
}

app.UseCors("AllowAllOrigins");

app.UseAuthorization();

app.MapControllers();

app.Run();
