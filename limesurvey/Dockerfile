FROM php:7.2-cli

ENV ACCEPT_EULA=Y

# Microsoft SQL Server Prerequisites
RUN apt-get update \
    && apt-get install -y gnupg2 \
    && curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - \
    && curl https://packages.microsoft.com/config/debian/9/prod.list \
        > /etc/apt/sources.list.d/mssql-release.list \
    && apt-get install -y --no-install-recommends \
        locales \
        apt-transport-https \
    && echo "en_US.UTF-8 UTF-8" > /etc/locale.gen \
    && locale-gen \
    && apt-get update \
    && apt-get -y --no-install-recommends install \
        unixodbc-dev \
        msodbcsql17

RUN docker-php-ext-install mbstring pdo pdo_mysql \
    && pecl install sqlsrv pdo_sqlsrv xdebug \
    && docker-php-ext-enable sqlsrv pdo_sqlsrv xdebug

COPY . /var/www/html/
WORKDIR /var/www/html/
CMD [ "php", "-S", "0.0.0.0:80", "-f", "./index.php" ]

# docker network create limesurvey_network
# docker run --name limesurvey --network limesurvey_network -p 80:80 -d limesurvey
# docker exec -it limesurvey sh

# mssql

# docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=I.am.using.docker" --name limesurvey_sqlserver --network limesurvey_network -p 1433:1433 -d mcr.microsoft.com/mssql/server