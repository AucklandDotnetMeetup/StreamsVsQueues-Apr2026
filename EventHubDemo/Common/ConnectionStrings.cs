namespace Common;

public static class ConnectionStrings
{
    public const string EventHubNamespaceConnectionString = "Endpoint=sb://localhost;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=EMULATOR_DEV_SAS_VALUE;UseDevelopmentEmulator=true;";
    public const string EventHubName = "eh1";
    public const string EventHubNameForKafka = "eh2";
    public const string CheckpointBlobContainer = "local-test";
    public const string BlobServiceConnectionString = "UseDevelopmentStorage=true";
}
