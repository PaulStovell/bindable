create table Foo (
	FooID uniqueidentifier not null,
	FooName nvarchar(max) not null
)
go

create procedure SaveFoo( 
	@fooName nvarchar(max)
)
as
begin
	insert into Foo(FooID, FooName) values (newid(), @fooName)
end
go