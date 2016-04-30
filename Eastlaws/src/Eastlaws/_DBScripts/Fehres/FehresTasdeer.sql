Use iglc

Go 


Truncate Table EastlawsData..FehresPrograms
Go
Truncate Table EastlawsData..FehresProgCountry
Go
Truncate Table EastlawsData..FehresCategories
Go
Truncate Table EastlawsData..FehresItems
Go
Truncate Table EastlawsData..FehresItemsDetails
Go
Truncate Table EastlawsData..ServicesFehresDetails
Go

Insert Into  EastlawsData..FehresPrograms (ID , Name , AlterName , IfShow , MyOrder)
Select ID , Name , AlterName , IF_Show , MyOrder From FehresPrograms

Go 
Insert Into EastlawsData..FehresProgCountry (ID , FehresPogramID, CountryID)
Select ID , Master_ID , Country_ID From FaharessProgCountry

Go 

Insert Into EastlawsData..FehresCategories(  ID  , Name , ProgCountryID , MyOrder , ShowInTash )
Select ID , Name ,isnull(Master_ID , 0) , MyOrder , Show_In_Tash From General_Category

Go 

Insert Into  EastlawsData..FehresItems (ID , ParentID , FehresCategoryID , Name , MyOrder , ABC )
Select ID , Parent_ID , Master_ID , Text , MyOrder , ABC From General_Fehres

Go 

Insert EastlawsData..FehresItemsDetails (ID , FehresItemID , ServiceID ,ServiceItemID , MyOrder , StartColor , EndColor )
Select - ID , Fehres_ID ,EastlawsData.dbo.GetServiceIDFromFehres(Detail_Type) ,Detail_Rec_ID , MyOrder , Start_Color , End_Color   From General_Fehres_AH_Tash
Where EastlawsData.dbo.GetServiceIDFromFehres(Detail_Type) is not null 
Union All 
Select ID , Master_ID , 1, Rec_ID , MyOrder , Start_Color , End_Color From General_Fehres_Details

Go 


--Insert Into  EastlawsData..ServicesFehresDetails (ServiceID , ItemID , SubItemID , FehresItemID , FehresCategoryID , FehresProgramID)
--Select ServiceID , F.HokmID , FD.ServiceItemID , FD.FehresItemID  , FC.ID , Map.ProgramID    From EastlawsData..FehresItemsDetails FD
--Join EastlawsData..FehresItems FI on FI.ID = FD.FehresItemID
--Join EastlawsData..FehresCategories FC on FC.ID = FI.FehresCategoryID
--Join EastlawsData..VW_FehresMap Map on Map.CategoryID = FC.ID
--Join EastlawsData..AhkamFakarat F on F.ID = FD.ServiceItemID And FD.ServiceID = 1 


Go 

Select * From General_Fehres_Details
Where Master_ID in (Select ID From General_Fehres Where Master_ID In ( 236,474,558,133,137))

Select * From General_Fehres_AH_Tash
Where Fehres_ID in (Select ID From General_Fehres Where Master_ID In ( 236,474,558,133,137))


Select * From EastlawsData..VW_FehresMap
Where ProgramName Like '%ÏÝæÚ%'