#!/bin/sh

# Start RabbitMQ server in the background
rabbitmq-server -detached

# Wait for RabbitMQ server to start
sleep 10

# Create a new user with the username and password provided via environment variables
rabbitmqctl add_user $RABBITMQ_USER $RABBITMQ_PASSWORD

# Set permissions for the new user
rabbitmqctl set_user_tags $RABBITMQ_USER administrator
rabbitmqctl set_permissions -p / $RABBITMQ_USER ".*" ".*" ".*"

# Stop the RabbitMQ server
rabbitmqctl stop

# Start RabbitMQ server in the foreground
rabbitmq-server
