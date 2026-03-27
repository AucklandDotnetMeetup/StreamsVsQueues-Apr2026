var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Emulator>("Emulator");
builder.AddProject<Projects.Producers>("Producers");
builder.AddProject<Projects.ConsumerWithoutSchemaReplica1>("ConsumerWithoutSchemaReplica1").WithExplicitStart();
builder.AddProject<Projects.ConsumerWithoutSchemaReplica2>("ConsumerWithoutSchemaReplica2").WithExplicitStart();
builder.AddProject<Projects.ConsumerWithAvroSchema>("ConsumerWithAvroSchema").WithExplicitStart();
builder.AddProject<Projects.ConsumerWithJsonSchema>("ConsumerWithJsonSchema").WithExplicitStart();
builder.AddProject<Projects.ConsumerWithProtobufSchema>("ConsumerWithProtobufSchema").WithExplicitStart();

builder.AddSpringApp(
    name: "Java-ShareGroupReplica1",
    workingDirectory: "../NewFeatures/ShareGroupReplica1",
    new JavaAppExecutableResourceOptions
    {
        Port = 12001,
        ApplicationName = "target/demo-0.0.1-SNAPSHOT.jar",
        OtelAgentPath = "../"
    }).WithExplicitStart();

builder.AddSpringApp(
    name: "Java-ShareGroupReplica2",
    workingDirectory: "../NewFeatures/ShareGroupReplica2",
    new JavaAppExecutableResourceOptions
    {
        Port = 12002,
        ApplicationName = "target/demo-0.0.1-SNAPSHOT.jar",
        OtelAgentPath = "../"
    }).WithExplicitStart();

builder.Build().Run();
