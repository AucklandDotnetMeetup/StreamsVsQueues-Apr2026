using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Services;

public enum KafkaSchemaTypes
{
    Avro,
    Protobuf,
    Json,
    None
}
