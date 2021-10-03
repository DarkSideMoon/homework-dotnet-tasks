CREATE USER repl@'%' IDENTIFIED WITH mysql_native_password BY 'slavepass';
GRANT REPLICATION SLAVE ON *.* TO repl@'%';

CREATE DATABASE test;

CREATE TABLE test.test (
    `id` INT UNSIGNED  NOT NULL PRIMARY KEY AUTO_INCREMENT,
    `name` VARCHAR(30) NOT NULL DEFAULT '',
    `extra` VARCHAR(30) NOT NULL DEFAULT ''
);