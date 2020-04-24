-- drop tables
drop table student;
drop table enrollment;
drop table studies;

-- tables
-- Table: Enrollment
CREATE TABLE Enrollment (
    IdEnrollment int  NOT NULL,
    Semester int  NOT NULL,
    IdStudy int  NOT NULL,
    StartDate date  NOT NULL,
    CONSTRAINT Enrollment_pk PRIMARY KEY  (IdEnrollment)
);

-- Table: Student
CREATE TABLE Student (
    IndexNumber nvarchar(100)  NOT NULL,
    Password nvarchar(100) NOT NULL,
    IdRole nvarchar(100) NOT NULL,
    Salt nvarchar(100),
    RefreshToken nvarchar(100),
    FirstName nvarchar(100)  NOT NULL,
    LastName nvarchar(100)  NOT NULL,
    BirthDate date  NOT NULL,
    IdEnrollment int NOT NULL,
    CONSTRAINT Student_pk PRIMARY KEY  (IndexNumber)
);


CREATE TABLE StudentRole (
    IdRole int NOT NULL,
    RoleName varchar(100) NOT NULL,
    CONSTRAINT Role_pk PRIMARY KEY  (IdRole)
);

-- Table: Studies
CREATE TABLE Studies (
    IdStudy int  NOT NULL,
    Name nvarchar(100)  NOT NULL,
    CONSTRAINT Studies_pk PRIMARY KEY  (IdStudy)
);

-- foreign keys
-- Reference: Enrollment_Studies (table: Enrollment)
ALTER TABLE Enrollment ADD CONSTRAINT Enrollment_Studies
    FOREIGN KEY (IdStudy)
    REFERENCES Studies (IdStudy);

-- Reference: Student_Enrollment (table: Student)
ALTER TABLE Student ADD CONSTRAINT Student_Enrollment
    FOREIGN KEY (IdEnrollment)
    REFERENCES Enrollment (IdEnrollment);
  
ALTER TABLE Student ADD CONSTRAINT Student_Role
    FOREIGN KEY (IdRole)
    REFERENCES StudentRole (IdRole);

--insert values
insert into studies(idstudy, name)
values(1, 'Computer Science'),
(2, 'Art');

insert into enrollment(idenrollment, idstudy, semester, startdate)
values(1, 1, 1, '01-10-2020'),
(2, 2, 2, '01-10-2020');

insert into StudentRole(IdRole, RoleName)
values(1, 'employee'),
(2, 'student');

insert into student(IndexNumber, password, IdRole, firstname, lastname, birthdate, idenrollment)
values('s18884', 'pass1', 1, 'Bob', 'Black', '02-02-1999', 1),
('s18885', 'pass2', 2, 'Rob', 'Brown', '05-03-1998', 1),
('s18886', 'pass3', 2, 'Mob', 'Green', '10-01-1997', 2);

select * from studies;
select * from enrollment;
select * from student;
select * from student s join studentRole sr on s.idRole = sr.idRole;
