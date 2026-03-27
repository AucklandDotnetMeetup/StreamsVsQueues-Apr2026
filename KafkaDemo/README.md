## Sep 1 - Install Prerequisites
- install.NET 10 SDK
- install Docker Desktop
- install OpenJDK 22
- install Maven cli
- download opentelemetry-javaagent.jar to NewFeatures folder

## Sep 2 - Add default .Net CA cert to java trust keystore by executing these commands
- `dotnet dev-certs https -ep aspire-dev.crt --format PEM`
- `keytool -importcert -alias dotnet-aspire-dev -file aspire-dev.crt -keystore "%JAVA_HOME%\lib\security\cacerts" -storepass changeit`
- `keytool -list -keystore "%JAVA_HOME%\lib\security\cacerts" -alias dotnet-aspire-dev -storepass changeit`

## Sep 3 - Generate jar file for Aspire to include Java applications
- under ShareGroupReplica1 folder execure command `mvn clean package`
- under ShareGroupReplica2 folder execure command `mvn clean package`

