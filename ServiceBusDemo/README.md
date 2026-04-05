## Install Prerequisites
- install .NET 10 SDK
- install Docker Desktop

## Start Aspire project AppHost1

## Test Case 1: basic ServiceBus producer and consumer
Produce single message with producers swagger page http://localhost:5165/swagger/index.html API endpoint `/Producer/single`

Produce multiple messages with producers API endpoint `/Producer/bybatch`

Start projects `ReceiverReplica1` and `ReceiverReplica2UseProcessor` in Aspire to consume messages

## Test Case 2: enable Session in ServiceBus
Produce messages with producers API endpoint `/Producer/session`

Start projects `SessionReceiverReplica1` and `SessionReceiverReplica2UseProcessor` in Aspire to consume messages

## Test Case 3: send schedule messages in ServiceBus
Produce messages with producers API endpoint `/Producer/schedule`

Start projects `ScheduleReceiver` in Aspire to consume messages

## Test Case 4: only messages saved to DeadLetterQueue
Produce messages with producers API endpoint `/Producer/dlq`

Start projects `ReceiverForDeadLetterQueue` in Aspire to view messages in DLQ