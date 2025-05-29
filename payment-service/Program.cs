using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.SQS;
using payment_service.Core.Application.Interfaces;
using payment_service.Core.Application.UseCases;
using payment_service.Infrastructure.Data.EventStore;
using payment_service.Infrastructure.Messaging;
using payment_service.Infrastructure.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSingleton<IPaymentUseCase, PaymentUseCase>();
builder.Services.AddSingleton<IEventPublisher, SqsPublisher>();
builder.Services.AddSingleton<IEventStore, EventRepository>();
builder.Services.AddHostedService<SqsConsumerService>();
builder.Services.AddHttpClient();
builder.Services.Configure<AppSettings>(builder.Configuration);
builder.Configuration.AddJsonFile("/app/appsettings.json", optional: true, reloadOnChange: true);
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

app.MapGet("/", () => "Payment Service");

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();