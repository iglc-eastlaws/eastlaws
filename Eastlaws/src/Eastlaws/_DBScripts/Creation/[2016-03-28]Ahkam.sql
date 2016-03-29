
Use IGLCUsers 

Go 

Create Table Applications 
(ID int Primary Key , Name nvarchar(64) , ApplicationType nvarchar(64) , Description nvarchar(512))

Go 

Insert Into Applications(ID , Name , ApplicationType , Description)
Values 
(0,N'Eastlaws.com' , N'WebApp' , N'Eastlaws.com Main website ')
,(1,N'EastlawsAcademy.com' , N'WebApp' , N'EastlawsAcademy.com  website ')
,(2,N'MobileAppEastlaws' , N'MobileApp' , N'Eastlaws.com MobileApp backend  ')
--,(3,N'IGLC EXE' , N'DesktopClient' , N'Eastlaws.com MobileApp backend  ')


Go 


Create Table Services
(ID int Primary Key , ApplicationID int null , Name nvarchar(64) , Description nvarchar(512), IsMultipleCountries bit , CountryPriceM float, FullPriceM float )

Go 



Create Table UserSettings 
(ApplicationID int null ,  UserID nvarchar(255) , Name varchar(32) , Value Sql_Variant , LastChanged DateTime2(0) )


Go 

Create Clustered Index IX_UserSettings 
on UserSettings (ApplicationID , UserID , Name )

Go 


CREATE TABLE Errors(ID int IDENTITY Primary Key ,UserID nvarchar(255) ,theDate datetime ,URL nvarchar(1000) ,ExceptionType nvarchar(750) ,ExceptionMessage nvarchar(max) ,
IpAddress nvarchar(18) ,ExtraInfo nvarchar(max) ,isSpecific bit ,BrowserInfo nvarchar(250) ,PreviousPage nvarchar(1500) ,RequestParams nvarchar(max) )

Go 

















Use IGLCData 

Go 


Create Table Countries 
(ID int Primary Key , Name nvarchar(128) , EnName nvarchar(128) , FlagPic varchar(32) , IsMasterCountry bit , Abbrev nvarchar(64) , AbbrevSlang nvarchar(64))

Go 

Create Table CountriesServicesAvailabality 
(ServiceID int , CountryID int , MyOrder int , PriceM float  )

Go 

Create Table Mahakem
(ID int Primary Key , CountryID int not null , Name nvarchar(128) , EnName nvarchar(128) , MyOrder int )

Go 


Create Table Ahkam
(ID int Primary Key ,MahkamaID int not null , CountryID int  , CaseNo int  , CaseYear int ,OfficeYear int , OfficeSuffix varchar(8) , PageNo int, PartNo int  
,IfAgree int  , ImagesCount int , TashCount int  , TashMawadCount int , CaseDateDay int , CaseDateMonth int , CaseDateYear int , CaseDate date )


Go 

Create Table AhkamFakarat 
(ID int Primary Key , HokmID int , FakraNo int , Text nvarchar(max) , EnText nvarchar(max) ,MogazText nvarchar(max) , TashCount int  , TashMawadCount int   )

Go 
















Create Table MasterSortTypes
(ID int Primary Key , Name nvarchar(64))

Go 

Insert Into MasterSortTypes (Name)
 Values ('CaseNo' )
 ,('CaseYear' )
 ,('CaseDate' )
 ,('OfficeYear' )


Go 


Create Table MasterSort
(ServiceID int not null ,SortType int ,ItemId int ,Value Sql_Variant  )

Go 

Create Table MasterTextTypes 
(ID int Identity Primary Key , Name nvarchar(64) )

Go 

Create Table MasterText 
()


