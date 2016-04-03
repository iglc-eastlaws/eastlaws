Select * From ServicesText

sp_helpindex ServicesText


Select * From ServicesSortTypes



--No Sort 
Select *
From ServicesText ST WITH (NOLOCK)
Where ST.ServiceID = 1 And ST.ServiceTypeID = 1
And Contains(Text , '(سرقة And بالإكراه) ')
Order By ItemNo

Group By ST.ItemParentID 








Select * From ContainsTable(ServicesText , Text , '(سرقة And بالإكراه) OR ("قتل عمد") OR ("محكمة النقض") OR (محكمة And الأسرة)') TT 
Order By TT.[Rank] desc


--Default Sort = {Relevance} 
Select ST.ItemParentID , Sum(TT.[Rank]) as MyRank , Sum(TT.[Rank]) As SortValue 
From ContainsTable(ServicesText , Text , '(سرقة And بالإكراه) OR ("قتل عمد") OR ("محكمة النقض") OR (محكمة And الأسرة)') TT 
Join ServicesText ST on ST.ID = TT.[Key]
Where ST.ServiceID = 1 And ST.ServiceTypeID = 1 
Group By ST.ItemParentID 
Order By Sum(TT.[Rank]) Desc 



 
-- Custom Sort {1 = CaseNo , 2 = CaseYear , 3 = CaseDate , 4 = OfficeYear }
-- OfficeYear 
Select ST.ItemParentID , Sum(TT.[Rank]) as MyRank  , SS.Value as SortValue 
From ContainsTable(ServicesText , Text , '(سرقة And بالإكراه) OR ("قتل عمد") OR ("محكمة النقض") OR (محكمة And الأسرة)') TT 
Join ServicesText ST on ST.ID = TT.[Key]
Join ServicesSort SS on SS.ServiceID = ST.ServiceID And SS.SortType = 2 And SS.ItemId = ST.ItemParentID
Where ST.ServiceID = 1 And ST.ServiceTypeID = 1 
Group By ST.ItemParentID , SS.Value
Order By SS.Value Desc 


Select ST.ItemParentID , null as MyRank  , SS.Value as SortValue 
From ServicesText ST 
Join ServicesSort SS on SS.ServiceID = ST.ServiceID And SS.SortType = 2 And SS.ItemId = ST.ItemParentID
Where ST.ServiceID = 1 And ST.ServiceTypeID = 1 
 And Contains(ST.Text , '(سرقة And بالإكراه) OR ("قتل عمد") OR ("محكمة النقض") OR (محكمة And الأسرة)')
Group By ST.ItemParentID , SS.Value
Order By SS.Value Desc 








Declare @PageNo int = 1 , @PageSize int = 200
Declare @ResultsPage Table(ItemID int , MyRank int , SortValue Sql_Variant )
Insert Into @ResultsPage 
Select  ItemParentID , MyRank , SortValue From (
		Select Row_Number() Over (order By SortValue desc) as Serial , *  From 
		(
			Select ST.ItemParentID , Sum(TT.[Rank]) as MyRank  , SS.Value as SortValue 
			From ContainsTable(ServicesText , Text , '(سرقة And بالإكراه) OR ("قتل عمد") OR ("محكمة النقض") OR (محكمة And الأسرة) ') TT   
			Join ServicesText ST WITH (NOLOCK) on ST.ID = TT.[Key]
			Join ServicesSort SS WITH (NOLOCK) on SS.ServiceID = ST.ServiceID And SS.SortType = 1 And SS.ItemId = ST.ItemParentID				
			Where ST.ServiceID = 1 And ST.ServiceTypeID = 1 
			Group By ST.ItemParentID , SS.Value 		
		) ResultsTableInner  
) ResultsTableOuter
Where Serial > ((@PageNo - 1 ) * @PageSize ) And Serial <= (@PageNo * @PageSize)

Select A.* From @ResultsPage RP 
join VW_Ahkam A WITH (NOLOCK)  On A.ID = RP.ItemID 
Order By RP.SortValue Desc 








Go 


Create index IX_Ahkam_CaseNo 
on Ahkam(CaseNo)

DBCC FreeProcCache
DBCC  DropCleanBuffers
DBCC FREESESSIONCACHE

Declare @PageNo int = 1 , @PageSize int = 200
Declare @ResultsPage Table(ItemID int Primary Key  , MyRank int , SortValue Sql_Variant )
Insert Into @ResultsPage 
Select  ItemParentID , MyRank , SortValue From (
		Select Row_Number() Over (order By SortValue desc) as Serial , *  From 
		(
			Select ST.ItemParentID , Sum(TT.[Rank]) as MyRank  , A.CaseNo as SortValue 
			From ContainsTable(ServicesText , Text , '(سرقة And بالإكراه) OR ("قتل عمد") OR ("محكمة النقض") OR (محكمة And الأسرة) ') TT   
			Join ServicesText ST WITH (NOLOCK) on ST.ID = TT.[Key]
			Join Ahkam A on A.ID = ST.ItemParentID 		
			Where ST.ServiceID = 1 And ST.ServiceTypeID = 1 
			Group By ST.ItemParentID 	, A.CaseNo	
		) ResultsTableInner  
) ResultsTableOuter
Where Serial > ((@PageNo - 1 ) * @PageSize ) And Serial <= (@PageNo * @PageSize)

Select A.* From @ResultsPage RP 
join VW_Ahkam A WITH (NOLOCK)  On A.ID = RP.ItemID 
Order By RP.SortValue Desc 

















Create Index IX_AhkamFakarat_FakraNo 
on AhkamFakarat(FakraNo)













-- [2016-04-03]

-- General Search Default Sort ={Relevance}
Select F.HokmID as ID , Sum(CT.[Rank]) as TextRank From 
ContainsTable(AhkamFakarat,*,'قتل And عمد and سبق') CT 
Join AhkamFakarat F on F.ID = CT.[Key]
Group By F.HokmID 


-- General Search CaseYear + Relevance Sort 
Select F.HokmID as ID  ,  Sum(CT.[Rank]) as TextRank From 
ContainsTable(AhkamFakarat,*,'قتل And عمد and سبق') CT 
Join AhkamFakarat F on F.ID = CT.[Key]
Join Ahkam A on A.ID = F.HokmID
Group By F.HokmID , A.CaseDate
Order By A.CaseDate Desc , TextRank Desc 





-- Advanced Search
Select HokmID as ID  , 0 as TextRank From AhkamFakarat  A 
Where FakraNo  > 1 And Contains(A.Text , 'قتل and عمد ')
Group By A.HokmID

Intersect  -- Union 

Select HokmID as ID  , 0 as TextRank  From AhkamFakarat  A 
Where FakraNo  =  0 And Contains(A.Text , 'برئاسة')
Group By A.HokmID






-- Custom Search With Text 
Select F.HokmID as ID  ,  Sum(CT.[Rank]) as TextRank From 
ContainsTable(AhkamFakarat,*,'قتل And عمد and سبق') CT 
Join AhkamFakarat F on F.ID = CT.[Key]
Join Ahkam A on A.ID = F.HokmID
Where A.CaseNo Between 10 And 25
Group By F.HokmID , A.CaseDate
Order By A.CaseDate Desc , TextRank Desc 





-- Custom Search Without Text 
Select A.ID as ID  ,  0 as TextRank From
 Ahkam A 
Where A.CaseNo Between 10 And 25
Order By A.CaseDate Desc , TextRank Desc 