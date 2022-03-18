use Mix.Config

config :kaffe,
consumer: [
    endpoints: [kafka: 9092],
    topics: ["tweets"],     # the topic(s) that will be consumed
    consumer_group: "connect-default",   # the consumer group for tracking offsets in Kafka
    message_handler: Gotcha
]