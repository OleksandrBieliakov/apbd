drop procedure if exists PromoteStudents;

go;

create procedure PromoteStudents @StudiesName varchar(100), @Semester int
as
begin
	begin tran;
	declare @IdEnrollment int = (select IdEnrollment from Studies s join Enrollment e 
								on s.IdStudy=e.IdStudy 
								where Name=@StudiesName and Semester=@Semester);
	if @IdEnrollment is Null
		begin
			raiserror('Enrollment does not exist', 1, 1);
		end;

	declare @IdEnrollmentNext int = (select IdEnrollment from Studies s join Enrollment e 
									on s.IdStudy=e.IdStudy 
									where Name=@StudiesName and Semester=@Semester+1);
	if @IdEnrollmentNext is Null
		begin
			set @IdEnrollmentNext = (select max(IdEnrollment)+1 from Enrollment);
			declare @IdStudy int = (select IdStudy from Studies where name=@StudiesName);
			insert into Enrollment(idenrollment, idstudy, semester, startdate)
			values(@IdEnrollmentNext, @IdStudy, @Semester+1, getdate());
		end;
	update Student set IdEnrollment=@IdEnrollmentNext where IdEnrollment=@IdEnrollment;

	select IdEnrollment, name, semester, startdate
	from Enrollment e join Studies s
	on e.idstudy= s.idstudy
	where idenrollment=@IdEnrollmentNext;

	commit;
end;

