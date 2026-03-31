namespace Common;

public static class ConnectionStrings
{
    public const string EventHubNamespaceConnectionString = "Endpoint=sb://localhost;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=EMULATOR_DEV_SAS_VALUE;UseDevelopmentEmulator=true;";
    public const string EventHubName1 = "eh1";
    public const string EventHubName2 = "eh2";
    public const string EventHubName3 = "eh3";
    public const string ConsumerGroup = "cgtest";
    public const string CheckpointBlobContainer = "local-test";
    public const string BlobServiceConnectionString = "UseDevelopmentStorage=true";
}
