using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Emulator>("Emulator");
builder.AddProject<Projects.Producers>("Producers");
//builder.AddProject<Projects.ReceiverNotSendOffset>("ReceiverNotSendOffset").WithExplicitStart();
builder.AddProject<Projects.ReceiverSendOffsetReplica1>("ReceiverSendOffsetReplica1").WithExplicitStart(); ;
builder.AddProject<Projects.ReceiverSendOffsetReplica2>("ReceiverSendOffsetReplica2").WithExplicitStart(); ;
//builder.AddProject<Projects.ReceiverWithKafkaReplica1>("ReceiverWithKafkaReplica1").WithExplicitStart(); ;
//builder.AddProject<Projects.ReceiverWithKafkaReplica2>("ReceiverWithKafkaReplica2").WithExplicitStart(); ;
//builder.AddProject<Projects.ReceiverWithProcessorBatch>("ReceiverWithProcessorBatch").WithExplicitStart(); ;

builder.Build().Run();
