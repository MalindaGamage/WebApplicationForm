using ApplicationForm;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using WebApplicationForm;

var builder = WebApplication.CreateBuilder(args);

// Add Swagger generation
builder.Services.AddSwaggerGen();

// Add controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllersWithViews();

// Initialize Cosmos DB client
var cosmosDbSettings = builder.Configuration.GetSection("CosmosDb");

// Ensure the CosmosClient is registered properly
builder.Services.AddSingleton<CosmosClient>(serviceProvider => {
    var settings = serviceProvider.GetRequiredService<IConfiguration>().GetSection("CosmosDb");
    return new CosmosClient(settings["AccountEndpoint"], settings["AccountKey"]);
});

// Parse container names
var containerNames = cosmosDbSettings.GetSection("Containers").GetChildren()
    .ToDictionary(x => x.Key, x => x.Value);

builder.Services.AddScoped<ICosmosDbService, CosmosDbService>(serviceProvider =>
{
    var client = serviceProvider.GetRequiredService<CosmosClient>();
    var databaseName = cosmosDbSettings["DatabaseName"];
    return new CosmosDbService(client, databaseName, containerNames);
});

// Register the ApplicationDetailsService with necessary parameters
builder.Services.AddScoped<IApplicationDetailsService, ApplicationDetailsService>(serviceProvider => {
    var client = serviceProvider.GetRequiredService<CosmosClient>();
    var databaseName = cosmosDbSettings["DatabaseName"];
    var containerName = containerNames["ApplicationContainer"];
    return new ApplicationDetailsService(client, databaseName, containerName);
});

//builder.Services.AddSingleton<ICosmosDbService>(InitializeCosmosClientInstanceAsync(cosmosDbSettings).GetAwaiter().GetResult());

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// Corrected method declaration
static async Task<ICosmosDbService> InitializeCosmosClientInstanceAsync(IConfigurationSection configurationSection)
{
    string databaseName = configurationSection["DatabaseName"];
    var containerSettings = configurationSection.GetSection("Containers").GetChildren()
        .ToDictionary(x => x.Key, x => x.Value);
    string accountEndpoint = configurationSection["AccountEndpoint"];
    string authKey = configurationSection["AccountKey"];

    if (string.IsNullOrEmpty(accountEndpoint) || string.IsNullOrEmpty(authKey))
    {
        throw new ArgumentException("Cosmos DB account endpoint or auth key is not provided.");
    }

    CosmosClient client = new CosmosClient(accountEndpoint, authKey);
    await client.CreateDatabaseIfNotExistsAsync(databaseName);

    foreach (var containerSetting in containerSettings)
    {
        var containerName = containerSetting.Key;
        var partitionKeyPath = containerSetting.Value;
        await client.GetDatabase(databaseName).CreateContainerIfNotExistsAsync(containerName, partitionKeyPath);
    }

    return new CosmosDbService(client, databaseName, containerSettings);
}