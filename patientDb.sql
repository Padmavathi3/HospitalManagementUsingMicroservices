create database patientDb;
use patientDb;

create table Patients(
 PatientID varchar(10) unique,
 MedicalHistory varchar(70),
 Insurance varchar(70),
 Gender varchar(20),
 DOB date);