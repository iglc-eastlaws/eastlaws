Use master

Go 

if not Exists (Select 1 from sys.databases where name = 'EastlawsData')
Begin 
	Create DataBase EastlawsData 
End


Go 
if not Exists (Select 1 from sys.databases where name = 'EastlawsUsers')
Begin 
	Create DataBase EastlawsUsers 
End


Go 




Use EastlawsUsers 

Go 

Create Table Applications 
(ID int Primary Key , Name nvarchar(64) , ApplicationType nvarchar(64) , Description nvarchar(512))

Go 

Insert Into Applications(ID , Name , ApplicationType , Description)
Values 
(0,N'Eastlaws.com' , N'WebApp' , N'Eastlaws.com Main website ')
,(1,N'EastlawsAcademy.com' , N'WebApp' , N'EastlawsAcademy.com  website ')
,(2,N'EastlawsMobileApp' , N'MobileApp' , N'Eastlaws.com MobileApp backend  ')
--,(3,N'IGLC EXE' , N'DesktopClient' , N'Eastlaws.com MobileApp backend  ')


Go 


Create Table Services
(ID int Primary Key , ApplicationID int null , Name nvarchar(64) , Description nvarchar(512), IsMultipleCountries bit , IsEnabled bit , CountryPriceM float, FullPriceM float )

Go 

Alter Table Services 
Add CONSTRAINT Services_FK FOREIGN KEY (ApplicationID )  References Applications(ID) on delete cascade on update cascade

Go

Insert Into Services(ID , ApplicationID , Name , Description , IsMultipleCountries , IsEnabled , CountryPriceM , FullPriceM )
Values 
(1 , 0 , 'Ahkam' , N'أحكام المحاكم العربية العليا' , 1 , 1 , 500 , 2000)
,(1 , 0 , 'Tashree3at' , N'التشريعات العربية' , 1 , 1 , 500 , 2000)
,(1 , 0 , 'Fatawa' , N'إدارات الفتوى والتشريع ' , 1 , 1 , 500 , 2000) -- فتاوى مجلس الدولة
,(1 , 0 , 'Etefa2eyat' , N'الإتفاقيات والمعاهدات الدولية' , 1 , 1 , 500 , 2000)

Go




Create Table UserSettings 
(ApplicationID int null ,  UserID nvarchar(255) , Name varchar(32) , Value Sql_Variant , LastChanged DateTime2(0) )

Go 



Go 

Create Clustered Index IX_UserSettings 
on UserSettings (ApplicationID , UserID , Name )

Go 


CREATE TABLE Errors(ID int IDENTITY Primary Key,ApplicationID int not null ,UserID nvarchar(255) ,theDate datetime ,URL nvarchar(1000) ,ExceptionType nvarchar(750)
 ,ExceptionMessage nvarchar(max) ,IpAddress nvarchar(18) ,ExtraInfo nvarchar(max) ,isSpecific bit ,BrowserInfo nvarchar(250) ,PreviousPage nvarchar(1500)
  ,RequestParams nvarchar(max) )

Go 



















Use EastlawsData 

Go 


Create Table Countries 
(ID int Primary Key , Name nvarchar(128) , EnName nvarchar(128) , FlagPic varchar(32) , IsMasterCountry bit , Abbrev nvarchar(64) , AbbrevSlang nvarchar(64))

Go 

Create Table CountriesServices
(ServiceID int , CountryID int , MyOrder int , PriceM float  )

Go 

Create Table AhkamMahakem
(ID int Primary Key , CountryID int not null , Name nvarchar(128) , EnName nvarchar(128) , MyOrder int )

Go 


Create Table Ahkam
(ID int Primary Key ,MahkamaID int not null , CountryID int  , CaseNo int  , CaseYear int ,OfficeYear int , OfficeSuffix varchar(8) , PageNo int, PartNo int  
,IfAgree int  , ImagesCount int , TashCount int  , TashMawadCount int , CaseDateDay int , CaseDateMonth int , CaseDateYear int , CaseDate date )


Go 

Create Table AhkamFakarat 
(ID int Primary Key , HokmID int , FakraNo int , Text nvarchar(max) , EnText nvarchar(max) ,MogazText nvarchar(max) , TashCount int  , TashMawadCount int   )

Go 



Create Table ServicesImages 
(ServiceID int not null , ItemID int not null , ImageID int not null , ImagePath nvarchar(1024) not null )



Go 






Create Table ServicesSortTypes
(ID int Primary Key , ServiceID int not null , Name nvarchar(64) , TableName varchar(64) , ColumnName varchar(64))


Go 

Insert Into ServicesSortTypes (ServiceID , Name,TableName,ColumnName)
 Values (1,'CaseNo' , 'Ahkam' , 'CaseNo')
 ,(1,'CaseYear' , 'Ahkam' , 'CaseYear')
 ,(1,'CaseDate' , 'Ahkam' , 'CaseDate')
 ,(1,'OfficeYear' , 'Ahkam' , 'OfficeYear')


Go 


Create Table ServicesSort
(ServiceID int not null ,SortType int ,ItemId int ,Value Sql_Variant  )

Go 

Create Table ServicesTextTypes 
(ID int Identity Primary Key ,ServiceID int not null , Name nvarchar(64) , TableName varchar(64) , ColumnName varchar(64) )

Go 

Create Table ServicesText 
()


