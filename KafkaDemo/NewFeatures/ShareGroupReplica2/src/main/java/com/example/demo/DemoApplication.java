package com.example.demo;

import java.time.Duration;
import java.util.Collections;
import java.util.Properties;

import org.apache.kafka.clients.consumer.AcknowledgeType;
import org.apache.kafka.clients.consumer.ConsumerConfig;
import org.apache.kafka.clients.consumer.ConsumerRecord;
import org.apache.kafka.clients.consumer.ConsumerRecords;
import org.apache.kafka.clients.consumer.KafkaShareConsumer;
import org.apache.kafka.common.serialization.StringDeserializer;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RestController;

@SpringBootApplication
@RestController
public class DemoApplication {

    private static final Logger logger = LoggerFactory.getLogger(DemoApplication.class);

    public static void main(String[] args) {
        SpringApplication.run(DemoApplication.class, args);

        Properties props = new Properties();
        props.put(ConsumerConfig.BOOTSTRAP_SERVERS_CONFIG, "localhost:9094");
        props.put(ConsumerConfig.GROUP_ID_CONFIG, "test-share-group");
        props.put(ConsumerConfig.KEY_DESERIALIZER_CLASS_CONFIG, StringDeserializer.class.getName());
        props.put(ConsumerConfig.VALUE_DESERIALIZER_CLASS_CONFIG, StringDeserializer.class.getName());

        props.put("share.acknowledgement.mode", "explicit");

        try (KafkaShareConsumer<String, String> consumer = new KafkaShareConsumer<>(props)) {
            consumer.subscribe(Collections.singletonList("test-share-group-topic"));
            while (true) {
                // Poll for records, implicit acknowledgement happens on next poll
                ConsumerRecords<String, String> records = consumer.poll(Duration.ofMillis(100));
                for (ConsumerRecord<String, String> record : records) {
                    try {
                        short currentAttempt = record.deliveryCount().orElse((short) 1);
                        logger.info("Attempts: {} Partition:{}, Offset: {}, Value: {}", currentAttempt, record.partition(), record.offset(), record.value());
                        Thread.sleep(100);
                        consumer.acknowledge(record, AcknowledgeType.ACCEPT);

                    } catch (Exception e) {
                        consumer.acknowledge(record, AcknowledgeType.RELEASE);
                        logger.info("Error processing record: " + e.getMessage());
                    }
                }
            }
        }
    }

    @GetMapping("/")
    public String hello() {
        return "Hello from Java 22!";
    }
}
