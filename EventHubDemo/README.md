## Install Prerequisites
- install .NET 10 SDK
- install Docker Desktop

## Start Aspire project AppHost1

## Test Case 1: basic EventHubs producer and consumer
Produce messages with producers swagger page http://localhost:5064/swagger/index.html API endpoint `/Producer/basic`

Start projects `ReceiverSendOffsetReplica1` and `ReceiverSendOffsetReplica2` in Aspire to consume messages

## Test Case 2: use Kafka producer and consumer
Produce messages with producers API endpoint `/Producer/bykafka`

Start projects `ReceiverWithKafkaReplica1` and `ReceiverWithKafkaReplica2` in Aspire to consume messages

## Test Case 3: batch processor with multiple partitions
Produce messages with producers API endpoint `/Producer/multipartitions`

Start projects `ReceiverWithProcessorBatch` in Aspire to consume messages

## Test Case 4: only view messages not save offset
Produce messages with producers API endpoint `/Producer/basic`

Start projects `ReceiverNotSendOffset` in Aspire to view messages