Use EastlawsData 

Go 


Create Table FehresPrograms 
(ID int Primary Key , Name nvarchar(512) , AlterName nvarchar(1024) , IfShow bit default (0) , MyOrder int )

Go 

Create Table FehresProgCountry
(ID int Primary Key nonclustered , FehresPogramID int not null , CountryID int not null )

Go 

Create Unique Clustered  Index IX_FehresProgCountry
on FehresProgCountry(FehresPogramID ,CountryID)

Go 

-- Show and IF_Show Question Samer !
Create Table FehresCategories 
(ID int Primary Key nonclustered, Name nvarchar(1024)  , ProgCountryID int not null , MyOrder int , ShowInTash bit )

Go 

Create Table FehresItems 
(ID int Primary Key  nonclustered , ParentID int not null , FehresCategoryID  int not null, Name nvarchar(max)  , MyOrder int , ABC char(1) )

Go 

Create Unique Clustered Index IX_FehresItems 
on FehresItems (FehresCategoryID , ParentID , ID ) 

Go 

Create Table FehresItemsDetails 
(ID int Primary Key nonclustered ,  FehresItemID int not null , ServiceID int not null , ServiceItemID int not null ,  MyOrder int , StartColor int , EndColor int)

Go 

Create Clustered Index IX_FehresItemsDetails 
on FehresItemsDetails (FehresItemID , ServiceID , ServiceItemID)


Go 

Create Table ServicesFehresDetails 
(ServiceID int , ItemID int , SubItemID int , FehresItemID int , FehresCategoryID int , FehresProgramID int  )

Go 


Create Unique Clustered Index IX_ServicesFehresDetails 
on ServicesFehresDetails (ServiceID , ItemID , SubItemID , FehresItemID , FehresCategoryID , FehresProgramID)
With  IGNORE_DUP_KEY   

Go 

Create function dbo.GetServiceIDFromFehres(@ProgType int )
returns int 
as 
Begin 
	return Case @ProgType When 1 then 2 
	When 2 then 1 
	else null 
	end

End


Go 

Alter View VW_FehresMap 
as 
Select top 99999 FP.ID as ProgramID , FP.Name as ProgramName , FP.AlterName  , C.ID as CountryID , C.Name as CountryName , FPC.ID as FehresCountryProgID , FC.ID as CategoryID
 , FC.Name as CategoryName  From FehresPrograms FP
Join FehresProgCountry FPC on FP.ID = FPC.FehresPogramID 
Join Countries C on C.ID = FPC.CountryID 
Join FehresCategories FC on FC.ProgCountryID = FPC.ID
Order By FP.MyOrder , C.ID , FC.MyOrder


Go 



Select * From VW_FehresMap
Where ProgramName Like N'%ÏÝæÚ%'


Select * From FehresItems Where  FehresCategoryID In ( 235,475,578,559,636,134,594,595,633,138,647,604,605)

Select * From FehresItemsDetails 
Where FehresItemID In ( 382192,382207,382211,382216,382227,382230,382238,383000,400254,400255,400256,400294,400304,406910,408555,558191,553597,553599,553601,553610,553714,553736,555102,656012,673706,673707,673710,673711,673712,673713,673714,673715,710916,710917,710918,710919,710920,710921,710922,710923,734515,734520,734893,752455,752456,752457,752458,752459,752460,752461,752467,752468,752469,752470,752471,752472,752473,752722,753059,747355,747356,753587,742425,742503,742790,754991,744536,744537,744538,744539,744542,744543,744544,744545,744546,744547,744548,744549,744551,744553,744554,744555,769706,751906,788222,744556,797176,797209,789294,801192,801199,801228,801274,801495,801580,801698,801935,802071,786502,805258,798600,802454,802455,802458,802460,802484,802486,791198,802739,787048,787049,787050,802939,799207,795291,795292,795293,795294,795295,795296,795297,795298,795319,795320,795321,795322,795323,795324,795325,795326,795327,806252,803795,810119,799934,804005,800480,800494,800495,800496,800497,800498,800499,800500,800697,800698,800779,811192,819757,832081,817738,833724,842533,874801,881247,881248,881249)


And ServiceID = 1 

Select * From ServicesFehresDetails
Where FehresProgramID = 30



Select Max(FI.ID) as ID ,  FI.Name , Count(Distinct SFD.ItemID)  From ServicesFehresDetails SFD 
Join FehresItems FI on FI.ID = SFD.FehresItemID and SFD.FehresCategoryID = FI.FehresCategoryID
Join EastlawsUsers..QueryCacheRecords QCR on QCR.ItemID = SFD.ItemID
Where  SFD.FehresProgramID = 29  And SFD.ServiceID = 1  And QCR.MasterID = 56 
Group By FI.Name




Select SFD.ServiceID , SFD.ItemID , SFD.FehresItemID, SFD.FehresCategoryID  From 
ServicesFehresDetails SFD  Join EastlawsUsers..QueryCacheRecords QCR on QCR.ItemID = SFD.ItemID
Where SFD.FehresProgramID = 29 And SFD.ServiceID = 1 and QCR.MasterID = 56



Delete From ServicesFehresDetails 
Where FehresProgramID not in (29,30,9,16)




Create  Clustered Index IX_ServicesFehresDetails
on ServicesFehresDetails (ServiceID , FehresProgramID , ItemID , FehresCategoryID , FehresItemID)

Drop Index IX_ServicesFehresDetails on ServicesFehresDetails


sp_helpindex ServicesFehresDetails
sp_helpindex FehresItems

Select Top 100 * From FehresItems
Create Clustered Index IX_FehresItems on FehresItems (FehresCategoryID)
