#!/bin/bash

docker run --name mp-sql --network bridge -p 3306:3306 -e MYSQL_ROOT_PASSWORD=P@ssw0rd -e MYSQL_DATABASE="AppDb" -e TZ=Europe/Warsaw -d mariadb:10.1.37-bionic