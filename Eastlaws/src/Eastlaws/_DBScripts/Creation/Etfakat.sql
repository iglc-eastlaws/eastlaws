
Go

Create Table Etfakat
(
	ID int Primary Key ,
	Text nvarchar(max),
	Date date
)


Go 

Create Table EtfFakarat 
(
	ID int Primary Key,
	EtfID int ,
	MyOrder int ,
	Title nvarchar(max),
	Text nvarchar(max) ,
	EnText nvarchar(max) 
)

Go 

Go 

Create Table EtfSignatories
(
	ID int Primary Key,
	EtfID int,
	CountryID int,
	Date date
)

Go 
Create Table EtfClasses
(
	ID int not null,
	ParentID int not null,
	Text nvarchar(max),
	MyOrder int,
	primary key (ID, ParentID)
)
Go

Insert Into EtfClasses (ID , ParentID,Text,MyOrder)
 Values (1000,0 , N'«·„‰Ÿ„«  «·œÊ·Ì…' , 0)
 ,(1001,0 , N'«· ÊﬁÌ⁄' , 1)
 ,(1002,0 , N'«·‰Ê⁄' , 2)
 ,(1003,0 , N'«· ⁄«Ê‰ «·ﬁ÷«∆Ï «·œÊ·Ï' , '3')
 
 GO

 
Create Table EtfClassMaster
(
	ID int Primary Key IDENTITY(1,1),
	ClassID int,
	EtfID int
)
Go

Create Table EtfTash
(
	ID int Primary Key,
	EtfID int,
	TashID int
)



Create Table EtfCountries
(
	ID int Primary Key ,
	Name nvarchar(50),
	MyOrder int
)