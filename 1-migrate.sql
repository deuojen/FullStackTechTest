create table speciality (
    Id INT PRIMARY KEY auto_increment,
    Name VARCHAR(50)
);
insert into speciality (Name) values ('Anaesthetics');
insert into speciality (Name) values ('Cardiology');
insert into speciality (Name) values ('Dermatology');
insert into speciality (Name) values ('Emergency Medicine');
insert into speciality (Name) values ('General Practice (GP)');
insert into speciality (Name) values ('Neurology');
insert into speciality (Name) values ('Obstetrics and Gynaecology');
insert into speciality (Name) values ('Ophthalmology');
insert into speciality (Name) values ('Orthopaedic Surgery');
insert into speciality (Name) values ('Psychiatry');

create table person_specialities (
    Id INT PRIMARY KEY auto_increment,
    PersonId INT,
    SpecialityId INT
);
