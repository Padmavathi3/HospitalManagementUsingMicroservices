create database userDb;
create table users(
UserId varchar(10) unique,
FirstName varchar(30),
LastName varchar(10),
Email varchar(60) unique,
Password varchar(40),
UserRole varchar(30),
IsApproved varchar(10));

select * from users;

