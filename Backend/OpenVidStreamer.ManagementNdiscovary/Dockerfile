# Use the official Consul image as a parent image
FROM consul:1.15.4

# Set the working directory in the container
WORKDIR /consul/config

# Copy the configuration file into the container at /consul/config
COPY config.json ./

# Expose the Consul ports (HTTP, HTTPS, GRPC, DNS, and Serf LAN and WAN)
EXPOSE 8500 8600 8600/udp 8300 8301 8301/udp 8302 8302/udp

# Use base command for Consul
CMD ["consul", "agent", "-config-dir=/consul/config"]
