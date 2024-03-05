# Use the official MariaDB image from the Docker Hub
FROM mariadb:11.3

# Set environment variables for your database
ENV MYSQL_ROOT_PASSWORD=12345
ENV MYSQL_DATABASE=VideoLibrarry
ENV MYSQL_USER=admin
ENV MYSQL_PASSWORD=12345

# Expose port 3306 to access the database
EXPOSE 3306

# The default command that runs when the container starts
CMD ["mysqld"]
