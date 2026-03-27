# steps to generate Avro schema classes
# manually execute these two commands in cmd under Common directory
1. dotnet tool install --global Apache.Avro.Tools
2. avrogen -s Schemas/Avro/UserAvro.asvc ./Schemas/Avro/Generated --skip-directories

# steps to generate Json schema classes
# manually execute these two commands in cmd under Common directory
1. dotnet tool install --global NSwag.ConsoleCore
2. nswag jsonschema2csclient /input:Schemas/Json/UserJson.json /output:Schemas/Json/UserJson.cs /name:UserJson /namespace:Common.Schemas.Json

# steps to generate Protobuf schema classes
1. put proto file under the folder
2. add Grpc.Tools nuget package in csproj file
3. include Protobuf files in csproj file 