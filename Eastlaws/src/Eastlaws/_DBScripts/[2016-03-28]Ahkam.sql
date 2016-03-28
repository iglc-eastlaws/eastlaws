
Use IGLCUsers 

Go 

Create Table Applications 
(ID int Primary Key , Name nvarchar(64) , ApplicationType nvarchar(64) , Description nvarchar(512))

Go 

Insert Into Applications(ID , Name , ApplicationType , Description)
Values 
(0,N'Eastlaws.com' , N'WebApp' , N'Eastlaws.com Main website ')
,(1,N'EastlawsAcademy.com' , N'WebApp' , N'EastlawsAcademy.com  website ')
,(2,N'Eastlaws.com MobileApp' , N'MobileApp' , N'Eastlaws.com MobileApp backend  ')
--,(3,N'IGLC EXE' , N'DesktopClient' , N'Eastlaws.com MobileApp backend  ')


Go 


Create Table Services
(ID int Primary Key , ApplicationID int null , Name nvarchar(64) , Description nvarchar(512), IsDirectoryService bit , CountryPriceM float, FullPriceM float )

Go 



Create Table UserSettings 
(ApplicationID int null ,  UserID nvarchar(255) , Name varchar(32) , Value Sql_Variant , LastChanged DateTime2(0) )


Go 

Create Clustered Index IX_UserSettings 
on UserSettings (ApplicationID , UserID , Name )

Go 






