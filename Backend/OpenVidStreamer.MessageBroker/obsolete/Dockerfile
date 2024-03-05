# Use the official RabbitMQ image with management plugin
FROM rabbitmq:3-management

# Set environment variables
ENV RABBITMQ_USER=user
ENV RABBITMQ_PASSWORD=password

# Add script to create default user
ADD init.sh /init.sh

# Set permissions for the script
RUN chmod +x /init.sh

# Expose RabbitMQ ports
EXPOSE 5672 15672

# Set default user and password
CMD ["/init.sh"]
