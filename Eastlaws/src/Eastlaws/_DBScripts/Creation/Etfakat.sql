
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
	ID int Primary Key,
	ParentID int,
	Text nvarchar(max),
	MyOrder int
)
Go

Insert Into EtfClasses (ID , ParentID,Text,MyOrder)
 Values (1000,0 , '«·„‰Ÿ„«  «·œÊ·Ì…' , 0)
 ,(1001,0 , '«· ÊﬁÌ⁄' , 1)
 ,(1002,0 , '«·‰Ê⁄' , 2)
 ,(1003,0 , '«· ⁄«Ê‰ «·ﬁ÷«∆Ï «·œÊ·Ï' , '3')
 
 GO

 
Create Table EtfClassMaster
(
	ID int Primary Key,
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