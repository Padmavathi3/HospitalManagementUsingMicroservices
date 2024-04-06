create database DoctorDb;
use DoctorDb;

create table Doctors(
DoctorID varchar(10) unique,
DoctorName varchar(50),
Specialization varchar(30),
Qualifications Varchar(60),
Schedule varchar(60));

