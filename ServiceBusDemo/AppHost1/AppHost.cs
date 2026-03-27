var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Emulator>("Emulator");
builder.AddProject<Projects.Producers>("Producers");
builder.AddProject<Projects.ReceiverReplica1>("ReceiverReplica1").WithExplicitStart();
builder.AddProject<Projects.ReceiverReplica2UseProcessor>("ReceiverReplica2UseProcessor").WithExplicitStart();
builder.AddProject<Projects.SessionReceiverReplica1>("SessionReceiverReplica1").WithExplicitStart();
builder.AddProject<Projects.SessionReceiverReplica2UseProcessor>("SessionReceiverReplica2UseProcessor").WithExplicitStart();
builder.AddProject<Projects.ScheduleReceiver>("ScheduleReceiver").WithExplicitStart();

builder.Build().Run();
