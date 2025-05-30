using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.SQS;
using booking_service.Core.Application.Interfaces;
using booking_service.Core.Application.UseCases;
using booking_service.Infrastructure.Data.EventStore;
using booking_service.Infrastructure.ExternalServices;
using booking_service.Infrastructure.Messaging;
using booking_service.Infrastructure.Models;

var builder = WebApplication.CreateBuilder(args);

#region app services

builder.Services.AddControllers();
builder.Services.AddSingleton<IBookingUseCases, BookingUseCases>();
builder.Services.AddSingleton<IInventoryService, HttpPropertyServiceClient>();
builder.Services.AddSingleton<IEventPublisher, SqsPublisher>();
builder.Services.AddSingleton<IEventRepository, EventRepository>();
builder.Services.AddHostedService<SqsConsumerService>();
builder.Services.AddHttpClient();
builder.Services.Configure<AppSettings>(builder.Configuration);
builder.Configuration.AddJsonFile("/app/appsettings.json", optional: true, reloadOnChange: true);
#endregion app services

#region aws services

var awsOptions = builder.Configuration.GetAWSOptions();
var isRunningLocally = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("AWS_ROLE_ARN"));

// Nếu chạy local, sử dụng AccessKey và SecretKey từ appsettings.json
if (isRunningLocally)
{
    var region = builder.Configuration["AWS:Region"];
    var accessKey = builder.Configuration["AWS:AccessKey"];
    var secretKey = builder.Configuration["AWS:SecretKey"];
    var sqsConfig = new AmazonSQSConfig
    {
        ServiceURL = builder.Configuration["AWS:Endpoint"],
        AuthenticationRegion = region
    };

    var dynamoConfig = new AmazonDynamoDBConfig
    {
        ServiceURL = builder.Configuration["AWS:Endpoint"],
        AuthenticationRegion = region
    };

    builder.Services.AddSingleton<IAmazonSQS>(sp =>
        new AmazonSQSClient(accessKey, secretKey, sqsConfig));

    builder.Services.AddSingleton<IAmazonDynamoDB>(sp =>
        new AmazonDynamoDBClient(accessKey, secretKey, dynamoConfig));
    builder.Services.AddSingleton<IDynamoDBContext, DynamoDBContext>();
}
else
{
    builder.Services.AddDefaultAWSOptions(awsOptions);
    builder.Services.AddAWSService<IAmazonSQS>();
    builder.Services.AddAWSService<IAmazonDynamoDB>();
    builder.Services.AddSingleton<IDynamoDBContext, DynamoDBContext>();
}

#endregion aws services

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UsePathBase("/booking");
app.MapGet("/", () => "Booking Service");

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();