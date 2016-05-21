Use EastlawsData

Go 



Create Table Tashree3atGarida 
(ID int Primary Key , CountryID int , Name nvarchar(512) )

Go 

Create Table Tashree3atBassma 
(ID int Primary Key , CountryID int not null , Name nvarchar(2048) , EnName nvarchar(2048))

Go 

Create Table Tashree3atTypes
(ID int Primary Key , CountryID int ,Name nvarchar(1024) , EnName nvarchar(1024)  , MyOrder int )

Go 



Create Table Tashree3at 
(ID int Primary Key  , CountryID int ,TypeID int , BassmaID int , GaridaID int ,TashNo int , TashYear int , TashDate Datetime
, Name nvarchar(max) , EnName nvarchar(max)  , NafazDate Date , EsdarDate Date , GaridaNumber nvarchar(100),  GaridaSuffix nvarchar(100)
,MawadCount int,  AhkamCount int , FakaratCount int  , ImagesCount int )


Go 


Create Table Tashree3atAhkam 
(ID int , CountryID int  , TashID int , MadaID int , HokmID int , FakraID int , IsMana3y bit)

Go 

Create Table Tashree3atFehres 
(ID int Primary Key , ParentID int , Tashree3ID int , Name nvarchar(max) , MyOrder int  )

Go 

Create Table Tashree3atMawad 
(ID int Primary Key , Tashree3ID int , FehresID int , MadaNo int  , MadaSuffix nvarchar(32) , Text nvarchar(max) , MyOrder int , ImageID int 
,EnText nvarchar(max) , AhkamCount int not null default(0)  , FakaratCount int not null default(0) )

Go 


Use iglc

Go 



Select top 9999 * From Tash_Master --
Select Top 9999 * From Tash_Type   --
Select Top 9999 * From Tash_Bassma --
Select Top 9999 * From Tash_Garida --
Select Top 9999 * From Tash_SubMaster --
Select Top 9999 * From Tash_Mawad -- 




