USE iglc

GO 
---FehresPrograms----
DELETE FROM EastlawsData..FehresPrograms 
WHERE ID NOT IN (SELECT ID FROM dbo.FehresPrograms)

GO 

UPDATE F
SET F.Name = FI.Name , F.AlterName = FI.AlterName,F.IfShow=FI.IF_Show,F.MyOrder=FI.MyOrder
From 
EastlawsData..FehresPrograms  F 
JOIN dbo.FehresPrograms  FI ON F.ID = FI.ID
WHERE Binary_Checksum(Cast(FI.Name AS nvarchar(max)) , Cast(FI.AlterName AS nvarchar(max)),FI.IF_Show,FI.MyOrder) <> 
	  Binary_Checksum(Cast(F.Name AS nvarchar(max)) , Cast(F.AlterName AS nvarchar(max)),F.IFShow,F.MyOrder)

GO 

INSERT INTO EastlawsData..FehresPrograms (ID,Name,AlterName,IfShow,MyOrder)
SELECT ID,Name,AlterName,IF_Show,MyOrder From dbo.FehresPrograms  FI
WHERE ID NOT IN (SELECT ID FROM EastlawsData..FehresPrograms )

GO 
----------FehresProgCountry----------------------------
Truncate Table EastlawsData..FehresProgCountry
Go
Insert Into EastlawsData..FehresProgCountry (ID , FehresPogramID, CountryID)
Select ID , Master_ID , Country_ID From FaharessProgCountry

Go
-------------FehresCategories---------------------//FI.Show_In_Tash=2??
DELETE FROM EastlawsData..FehresCategories 
WHERE ID NOT IN (SELECT ID FROM dbo.General_Category)

GO 

UPDATE F
SET F.Name = FI.Name , F.ProgCountryID = isnull(FI.Master_ID , 0),F.MyOrder=FI.MyOrder,F.ShowInTash=FI.Show_In_Tash
 From 
EastlawsData..FehresCategories  F 
JOIN dbo.General_Category  FI ON F.ID = FI.ID
WHERE Binary_Checksum(Cast(FI.Name AS nvarchar(max)) ,isnull(FI.Master_ID , 0),FI.MyOrder,FI.Show_In_Tash) <> 
	  Binary_Checksum(Cast(F.Name AS nvarchar(max)) , F.ProgCountryID,F.MyOrder,F.ShowInTash)

GO 

INSERT INTO EastlawsData..FehresCategories (ID,Name,ProgCountryID,MyOrder,ShowInTash)
SELECT ID,Name,isnull(FI.Master_ID , 0),MyOrder,Show_In_Tash From dbo.General_Category  FI
WHERE ID NOT IN (SELECT ID FROM EastlawsData..FehresCategories )

GO 
------------------FehresItems----------------//4014 update

DELETE FROM EastlawsData..FehresItems 
WHERE ID NOT IN (SELECT ID FROM dbo.General_Fehres)

GO 

UPDATE F
SET F.ParentID=FI.Parent_ID,F.FehresCategoryID=FI.Master_ID,F.Name = FI.Text ,F.MyOrder=FI.MyOrder,F.ABC=N''+FI.ABC
From 
EastlawsData..FehresItems  F 
JOIN dbo.General_Fehres  FI ON F.ID = FI.ID
WHERE Binary_Checksum(FI.Parent_ID,FI.Master_ID,Cast(FI.Text AS nvarchar(max)) ,FI.MyOrder,FI.ABC) <> 
	  Binary_Checksum(F.ParentID,F.FehresCategoryID,Cast(F.Name AS nvarchar(max)),F.MyOrder,F.ABC)

GO 

INSERT INTO EastlawsData..FehresItems (ID,ParentID,FehresCategoryID,Name,MyOrder,ABC)
SELECT ID , Parent_ID , Master_ID , Text , MyOrder , ABC From dbo.General_Fehres  FI
WHERE ID NOT IN (SELECT ID FROM EastlawsData..FehresItems )
Go
---------------FehresItemsDetails---------
Truncate Table EastlawsData..FehresItemsDetails
Go


Insert EastlawsData..FehresItemsDetails (ID , FehresItemID , ServiceID ,ServiceItemID , MyOrder , StartColor , EndColor )
Select - ID , Fehres_ID ,EastlawsData.dbo.GetServiceIDFromFehres(Detail_Type) ,Detail_Rec_ID , MyOrder , Start_Color , End_Color   From General_Fehres_AH_Tash
Where EastlawsData.dbo.GetServiceIDFromFehres(Detail_Type) is not null 
Union All 
Select ID , Master_ID , 1, Rec_ID , MyOrder , Start_Color , End_Color From General_Fehres_Details
Where EastlawsData.dbo.GetServiceIDFromFehres(ProgType) is not null 
Go