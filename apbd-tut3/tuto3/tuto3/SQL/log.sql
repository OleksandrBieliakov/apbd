-- drop tables
drop table LogEntry;
drop table HttpMethod;

-- tables
CREATE TABLE LogEntry (
    IdLogEntry int  NOT NULL,
    EntryTime datetime  NOT NULL,
    IdMethod int NOT NULL,
    PathString nvarchar(1000),
    QueryString nvarchar(1000),
    BodyString nvarchar(4000),
    CONSTRAINT LogEntry_pk PRIMARY KEY  (IdLogEntry)
);

CREATE TABLE HttpMethod (
    IdMethod int  NOT NULL,
    MethodName nvarchar(20)  NOT NULL,
    CONSTRAINT HttpMethod_pk PRIMARY KEY  (IdMethod)
);

-- foreign keys
ALTER TABLE LogEntry ADD CONSTRAINT LogEntry_Method
    FOREIGN KEY (IdMethod)
    REFERENCES HttpMethod (IdMethod);

--insert values
insert into HttpMethod(IdMethod, MethodName)
values(1, 'GET');

insert into LogEntry(IdLogEntry, EntryTime, IdMethod, PathString, QueryString, BodyString)
values(1, CURRENT_TIMESTAMP, 1, 'api/log/test',  null,'TEST LOG ENTRY');
