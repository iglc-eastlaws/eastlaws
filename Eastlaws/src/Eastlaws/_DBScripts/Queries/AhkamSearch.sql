Select * From ServicesText

sp_helpindex ServicesText


Select * From ServicesSortTypes



--No Sort 
Select ST.ItemParentID 
From ServicesText ST WITH (NOLOCK)
Where ST.ServiceID = 1 And ST.ServiceTypeID = 1 
And Contains(Text , '(سرقة And بالإكراه) OR ("قتل عمد") OR ("محكمة النقض") OR (محكمة And الأسرة)')
Group By ST.ItemParentID 





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
Join ServicesSort SS on SS.ServiceID = ST.ServiceID And SS.SortType = 3 And SS.ItemId = ST.ItemParentID
Where ST.ServiceID = 1 And ST.ServiceTypeID = 1 
Group By ST.ItemParentID , SS.Value
Order By SS.Value Desc 









Declare @PageNo int = 1 , @PageSize int = 2000
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





