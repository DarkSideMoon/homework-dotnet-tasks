CREATE DATABASE IF NOT EXISTS homeworkdb;
CREATE USER IF NOT EXISTS 'homeworkuser'@'%' IDENTIFIED BY 'homeworkuser';
GRANT ALL PRIVILEGES ON homeworkdb.* TO 'homeworkuser'@'%';

USE homeworkdb;
CREATE TABLE IF NOT EXISTS `book`
(
    `AuthorFirstName` varchar(500) NOT NULL default '',
    `AuthorLastName` varchar(500) NOT NULL default '',
    `BookType` varchar(100) NOT NULL default '',
    `CountOfPages` int(10) NOT NULL default '0',
    `ISBN` varchar(100) NOT NULL default '',
    `Price` DOUBLE PRECISION(40,2) NOT NULL default '0',
    `Title` varchar(500) NOT NULL default '',
    `AuthorEmail` varchar(500) NOT NULL default '',
    `CreateDate` DATE NOT NULL default CURRENT_TIMESTAMP,
    `Id` INT NOT NULL AUTO_INCREMENT PRIMARY KEY
) ENGINE=InnoDB CHARACTER SET utf8 COLLATE utf8_general_ci;