﻿Use master

Go 


DROP DATABASE EastlawsData

GO 

DROP DATABASE EastlawsUsers

GO 






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
,(2,N'MobileAppEastlaws' , N'MobileApp' , N'Eastlaws.com MobileApp backend  ')
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
,(2 , 0 , 'Tashree3at' , N'التشريعات العربية' , 1 , 1 , 500 , 2000)
,(3 , 0 , 'Fatawa' , N'إدارات الفتوى والتشريع ' , 1 , 1 , 500 , 2000) -- فتاوى مجلس الدولة
,(4 , 0 , 'Etefa2eyat' , N'الإتفاقيات والمعاهدات الدولية' , 1 , 1 , 500 , 2000)

Go




Create Table UserSettings 
(ApplicationID int null ,  UserID nvarchar(255) , Name varchar(32) , Value Sql_Variant , LastChanged DateTime2(0) )

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
(ID int Primary Key NonClustered , HokmID int , FakraNo int , Text nvarchar(max) , EnText nvarchar(max) ,MogazText nvarchar(max) , TashCount int  , TashMawadCount int   )

Go 

Create Clustered Index IX_AhkamFakarat 
on AhkamFakarat (HokmID)




Create Table ServicesImages 
(ServiceID int not null , ItemID int not null , ImageID int not null , ImagePath nvarchar(1024) not null )



Go 





Create Table ServicesSortTypes
(ID int IDENTITY	 Primary Key , ServiceID int not null , Name nvarchar(64) , TableName varchar(64) , ColumnName varchar(64))


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

CREATE UNIQUE CLUSTERED INDEX IX_ServicesSort 
ON ServicesSort(ServiceID,SortType,ItemId,Value)

GO 




Create Table ServicesTextTypes 
(ID int Identity Primary Key ,ServiceID int not null,Name nvarchar(64),TableName varchar(64) , ColumnName varchar(64) , IsAutoGenerated bit , RankingModifier int 
,AutoGenerateQuery nvarchar(max) 
)

Go 
INSERT INTO ServicesTextTypes(ServiceID , Name , TableName , ColumnName , IsAutoGenerated , RankingModifier , AutoGenerateQuery)
Values 
(1,'FakaratText' , 'AhkamFakarat', 'Text' , 0 , 10 , null) 



Create Table ServicesText 
(ID int IDENTITY PRIMARY KEY NONCLUSTERED , ServiceID int not null , ServiceTypeID int not null , ItemID int , ItemParentID int , Text nvarchar(max), ItemNo int NULL , CheckSumVal int )

Go 

CREATE UNIQUE CLUSTERED INDEX IX_ServicesText
ON ServicesText(ServiceID , ServiceTypeID , ItemParentID, ItemID , ItemNo,CheckSumVal)


Go 






USE [EastlawsData]
GO
CREATE FULLTEXT CATALOG [DefCat]WITH ACCENT_SENSITIVITY = ON
AS DEFAULT
AUTHORIZATION [dbo]

GO
USE [EastlawsData]
GO
CREATE FULLTEXT INDEX ON [dbo].[ServicesText] KEY INDEX [PK__Services__3214EC2620C1E124] ON ([DefCat]) WITH (CHANGE_TRACKING AUTO)
GO
USE [EastlawsData]
GO
ALTER FULLTEXT INDEX ON [dbo].[ServicesText] ADD ([Text] LANGUAGE [Arabic])
GO
USE [EastlawsData]
GO
ALTER FULLTEXT INDEX ON [dbo].[ServicesText] ENABLE
GO
USE [EastlawsData]
GO
CREATE FULLTEXT INDEX ON [dbo].[AhkamFakarat] KEY INDEX [PK__AhkamFak__3214EC270BC6C43E] ON ([DefCat]) WITH (CHANGE_TRACKING AUTO)
GO
USE [EastlawsData]
GO
ALTER FULLTEXT INDEX ON [dbo].[AhkamFakarat] ADD ([Text] LANGUAGE [Arabic])
GO
USE [EastlawsData]
GO
ALTER FULLTEXT INDEX ON [dbo].[AhkamFakarat] ADD ([MogazText] LANGUAGE [Arabic])
GO
USE [EastlawsData]
GO
ALTER FULLTEXT INDEX ON [dbo].[AhkamFakarat] ENABLE
GO



Go 
 

Alter Proc ExportServicesSort 
@MyServiceID int = null
as 
Begin 
	--ServicesSort 
	TRUNCATE TABLE ServicesSort 


	DECLARE @ID int ;
	DECLARE @ServiceID int ;
	DECLARE @TableName nvarchar(64);
	DECLARE @ColumnName nvarchar(64);

	DECLARE db_cursor CURSOR FOR   
		SELECT ID , ServiceID , TableName , ColumnName FROM ServicesSortTypes sst
		Where (ServiceID = @ServiceID OR @ServiceID is null )

	OPEN db_cursor  
	FETCH NEXT FROM db_cursor INTO @ID , @ServiceID , @TableName , @ColumnName  

	WHILE (@@FETCH_STATUS = 0 )
	BEGIN  
			DECLARE @Query nvarchar(4000);
			SET @Query	 = N'Insert Into ServicesSort (ServiceID,SortType,ItemID,Value) '
			+ ' Select ' + str(@ServiceID) + ',' + str(@ID) + ',' + 'ID' + ',' + @ColumnName + ' From ' + @TableName 
			EXEC (@Query)

		   FETCH NEXT FROM db_cursor INTO @ID , @ServiceID , @TableName , @ColumnName    
	END  

	CLOSE db_cursor  
	DEALLOCATE db_cursor 


End


Go 
-- Not Complete (Needs to be fully dynamic ! ) 
Create Proc ExportServicesText 
@MyServiceID int = null 
as 
Begin 
	Select * From ServicesTextTypes
	Declare @ID int , @ServiceID int , @TableName nvarchar(64) , @ColumnName nvarchar(64) , @IsAutoGenerated bit  , @AutoGenerateQuery nvarchar(max) ;

	DECLARE db_cursor CURSOR FOR   
	SELECT ID , ServiceID , TableName , ColumnName , IsAutoGenerated , AutoGenerateQuery FROM ServicesTextTypes STT
	Where (ServiceID = @ServiceID OR @ServiceID is null )

	OPEN db_cursor  
	FETCH NEXT FROM db_cursor INTO @ID , @ServiceID , @TableName , @ColumnName  , @IsAutoGenerated , @AutoGenerateQuery

	WHILE (@@FETCH_STATUS = 0 )
	BEGIN  
		Declare @MyQuery nvarchar(max) 
		if(@IsAutoGenerated = 1)
		Begin 
			
		End
		Else
		Begin 
			--Select * From ServicesText

			Delete From ServicesText Where ServiceID = 1 And ServiceTypeID = 1 And 
			ItemID not in (Select ID From AhkamFakarat )

			Update D 
			Set D.ItemParentID = S.HokmID  , D.ItemNo = S.FakraNo , D.Text = S.Text
			, D.CheckSumVal = BINARY_CHECKSUM(S.ID,S.HokmID,S.Text ,S.FakraNo)
			From ServicesText  D 
			Join AhkamFakarat S on D.ServiceID = 1 And D.ServiceTypeID = 1 And D.ItemID = S.ID
			Where BINARY_CHECKSUM(S.ID,S.HokmID,S.Text ,S.FakraNo) <> D.CheckSumVal
			

			Insert Into ServicesText (ServiceID , ServiceTypeID , ItemID , ItemParentID , Text , ItemNo , CheckSumVal)
			Select 1 , 1 , ID, HokmID , Text , FakraNo , BINARY_CHECKSUM(ID,HokmID,Text ,FakraNo) From AhkamFakarat 
			Where ID not in (Select ItemID From ServicesText Where ServiceID = 1 And ServiceTypeID = 1 )


			
		End
		FETCH NEXT FROM db_cursor INTO @ID , @ServiceID , @TableName , @ColumnName  , @IsAutoGenerated , @AutoGenerateQuery
	End

End


Go 

Create View VW_Ahkam  
as 
Select A.* , AM.Name as MahkamaName , C.Name as CountryName From Ahkam A With(NoLock)
join AhkamMahakem AM With(NoLock) on A.MahkamaID = AM.ID
Join Countries C With(NoLock) on C.ID = A.CountryID 


Go 


-- Dummy 
Create Table Tashree3at 
(ID int Primary Key nonclustered   , TashTypeID int , TashNo int , TashYear int , TashDate Date , Title nvarchar(max) )

Go 


Create Table Tashree3atMawad 
(ID int Primary Key NonClustered , Tashree3ID int not null , TashFehresID int  , MadaNo int ,  MadaSuffix nvarchar(64) , Text nvarchar(max) , MyOrder Int , ImageID int      )



Select top 100 * From Tash_Master




-- General search
use EastlawsGeneralSearch