create schema Membership;
go
create schema Wiki;
go

create table Membership.Member (
    Id int Identity(1,1) not null constraint PK_Member_Id primary key nonclustered,
    OpenId nvarchar(100) not null,
    EmailAddress nvarchar(255) not null,
    FullName nvarchar(255) not null,
    LastLogin datetime not null,
    IsActive bit not null
)
go

create table Membership.[Group] (
    Id int identity(1,1) not null constraint PK_MemberGroup_Id primary key clustered,
    GroupName nvarchar(80) not null constraint UX_MemberGroup_Name unique nonclustered
)
go

create table Membership.GroupMember (
    Id int Identity(1,1) not null constraint PK_MemberGroupMember_Id primary key clustered,
    MemberId int not null constraint FK_GroupMember_MemberId foreign key references Membership.Member(Id),
    GroupId int not null constraint FK_GroupMember_GroupId foreign key references Membership.[Group](Id)
)
go

create table Membership.Operation (
    Id int Identity(1,1) not null constraint PK_Operation_Id primary key clustered,
    OperationName nvarchar(80) not null constraint UX_Operation_Name unique nonclustered,
    Note nvarchar(max) not null,
    ParentId int not null constraint FK_Operation_ParentId foreign key references Membership.Operation(Id),
)
go

create table Membership.Permission (
    Id int Identity(1,1) not null constraint PK_Permission_Id primary key clustered,
    OperationId int not null constraint FK_Permission_OperationId foreign key references Membership.Operation(Id),
    GroupId int not null constraint FK_Permission_GroupId foreign key references Membership.[Group](Id),
    InRelationTo uniqueidentifier null,
    Allow bit not null,
)
go

create table Wiki.Wiki (
    Id int Identity(1,1) not null constraint PK_Wiki_Id primary key nonclustered,
    WikiName nvarchar(80) not null,
    Title nvarchar(255) not null,
    SecurityKey uniqueidentifier not null rowguidcol constraint UX_Wiki_SecurityKey unique nonclustered,
    IsActive bit not null
)
go

create table Wiki.Entry (
	Id int Identity(1,1) not null constraint PK_WikiEntry_Id primary key nonclustered,
    WikiId int not null constraint FK_WikiEntry_Wiki foreign key references Wiki.Wiki(Id),
    EntryName nvarchar(80) not null,
	Title nvarchar(255) not null,
	Summary nvarchar(max) not null,
    IsActive bit not null
)
go

create table Wiki.Revision (
	Id int Identity(1,1) not null constraint PK_WikiEntryRevision_Id primary key clustered,
    EntryId int not null constraint FK_WikiRevision_WikiEntry foreign key references Wiki.Entry(Id),
    MemberId int not null constraint FK_WikiRevision_Member foreign key references Membership.Member(Id),
    Body nvarchar(max) not null,
    RevisionComment nvarchar(max) not null,
    RevisionDateUtc datetime not null,
    ModerationStatus nvarchar(20) not null,	-- Awaiting, Passed, Rejected
    IsActive bit not null
)
go

create table Wiki.Comment (
    Id int identity(1,1) not null constraint PK_WikiComment_Id primary key nonclustered,
    EntryId int not null constraint FK_WikiComment_WikiEntry foreign key references Wiki.Entry(Id),
    MemberId int null constraint FK_WikiComment_Member foreign key references Membership.Member(Id),
    AuthorName nvarchar(255) not null, -- Used when the member is unknown
    AuthorEmail nvarchar(255) not null,
    AuthorIP nvarchar(50) not null,
    AuthorUrl nvarchar(255) not null,
    Comment nvarchar(max) not null,
    PostedDateUtc datetime not null,
    ModerationStatus nvarchar(20) not null, -- Either: Valid, Invalid, possibly others later
    History nvarchar(max) not null
)
go
