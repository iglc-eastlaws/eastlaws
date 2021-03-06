﻿Select * From ServicesText

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
			Group By ST.ItemParentID, A.CaseNo	
		) ResultsTableInner  
) ResultsTableOuter
Where Serial > ((@PageNo - 1 ) * @PageSize ) And Serial <= (@PageNo * @PageSize)

Select A.* From @ResultsPage RP 
join VW_Ahkam A WITH (NOLOCK)  On A.ID = RP.ItemID 
Order By RP.SortValue Desc 

















Create Index IX_AhkamFakarat_FakraNo 
on AhkamFakarat(FakraNo)







Go 

Select Top 100 * From VW_Ahkam




Declare @PageNo int = 1 , @PageSize int = 200
Declare @ResultsPage Table(ItemID int Primary Key  ,  SortValue Sql_Variant )
Insert Into @ResultsPage 
Select  ItemID , DefaultRank as SortValue From (
		Select Row_Number() Over (order By DefaultRank desc) as Serial , *  From 
		(
			Select QCR.ItemID , QCR.DefaultRank From EastlawsUsers..QueryCacheRecords QCR With (NoLock) Where QCR.MasterID = 2
		) ResultsTableInner  
) ResultsTableOuter
Where Serial > ((@PageNo - 1 ) * @PageSize ) And Serial <= (@PageNo * @PageSize)

Select A.* From @ResultsPage RP 
join VW_Ahkam A WITH (NOLOCK)  On A.ID = RP.ItemID 
Order By RP.SortValue Desc 


sp_helptext VW_Ahkam


CREATE Function dbo.GetHokmName(@CountryName nvarchar(max) ,   @Ma7kamName nvarchar(max) , @CaseNo int  , @CaseYear int , @CaseDate nvarchar(100) = null )    
returns nvarchar(4000)      
as     
Begin     
 if(@Ma7kamName = N'أحكام التحكيم التجاري الدولي' OR @Ma7kamName = N'محكمة إستئناف القاهرة' OR  @Ma7kamName = N'أحكام مركز القاهرة الإقليمي للتحكيم')  
   return isnull (@CountryName + ' - ', '') +  @Ma7kamName +  (isnull (N' الحكم رقم '  + Cast(@CaseNo as varchar) , ' ') ) +  (isnull (N' لسنة '  + Cast(@CaseYear as varchar) + N' قضائية ' , ' ') )    
   + isnull (N' بتاريخ ' +  @CaseDate + ' ',' ');   
  
  return isnull (@CountryName + ' - ', '') +  @Ma7kamName +  (isnull (N' الطعن رقم '  + Cast(@CaseNo as varchar) , ' ') ) +  (isnull (N' لسنة '  + Cast(@CaseYear as varchar) + N' قضائية ' , ' ') )       + isnull (N' بتاريخ ' +  @CaseDate + ' ',' ');    

     

End 







Alter View VW_Ahkam  
as 
Select A.* , AM.Name as MahkamaName , C.Name as CountryName  , dbo.GetHokmName(C.Name , AM.Name , A.CaseNo , A.CaseYear  , A.CaseDate ) as FullName
From Ahkam A With(NoLock)
join AhkamMahakem AM With(NoLock) on A.MahkamaID = AM.ID
Join Countries C With(NoLock) on C.ID = A.CountryID 


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

Select HokmID as ID  , 0 as DefaultRank  From AhkamFakarat  A 
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
Select A.ID as ID  ,  0 as TextRank From Ahkam A 
Where A.CaseNo Between 10 And 25
Order By A.CaseDate Desc , TextRank Desc 


Select A.ID as ID  ,  0 as DefaultRank From Ahkam A Where (1 = 1)
And CountryID = 1 


Select A.MahkamaID  , A.CountryID , A.CaseNo , A.CaseYear , A.OfficeYear , A.OfficeSuffix, A.PartNo  , A.PageNo , A.CaseDate ,A.IfAgree  From Ahkam  A
Where MahkamaID in ()

Select HokmID as ID , 0 as DefaultRank From AhkamFakarat
Where Contains(* , 'قتل')
Group By HokmID


SELECT display_term, source_term, occurrence FROM sys.dm_fts_parser(N'"محكمة"', 1025, 0, 0)
SELECT display_term, source_term, occurrence FROM sys.dm_fts_parser(N'FORMSOF( FREETEXT, "قتل حسن" )',1025 ,0 , 0)




Select * From fn_helpcollations()


DECLARE @SearchWord varchar(max) = N'"قتل عمد مع سبق الإصرار والترصد"'

SELECT * FROM sys.dm_fts_parser('FormsOf(FREETEXT, "قتل عمد مع سبق الإصرار والترصد")', 1025, default, 0) 
where display_term in 
( SELECT display_term FROM sys.dm_fts_parser('FORMSOF(INFLECTIONAL, "go to school")', 1033, null, 0) )

Select * From sys.sysfulltextcatalogs




select * from sys.dm_fts_parser('"قتل"',
1025,--Slovenian lcid
NULL,--No stop words
0)--Accent Insensitive


Go 



Create function dbo.GetAhkamFakraTitle(@FakraNo int)
returns nvarchar(100)
as 
Begin 
	return Case
				When @FakraNo = 0 then N'الهيئة'
				When @FakraNo = -1 then N'الوقائع'
				When @FakraNo = -2 then N'منطوق الحكم'
				When @FakraNo = -3 then N'حيثيات الحكم'
				When @FakraNo = -50 then N'الدستورية'
				When @FakraNo = 1 then N'المبادئ'
			Else 
				Cast(@FakraNo as nvarchar(100))
			End
End

Go 

Create Function dbo.GetAhkamFakraOrder(@FakraNo int )
returns int 
as 
Begin 
	return Case 				 
				When @FakraNo   = -1  then 10001 
				When @FakraNo   = -2  then 10002 
				When @FakraNo   = -3  then 10003
				When @FakraNo   = -50 then 10004

	Else 
		@FakraNo 
	End
End

Go 



Create View VW_AhkamFakarat 
as 
Select AF.* 
, dbo.GetAhkamFakraTitle(AF.FakraNo) as Title 
,dbo.GetAhkamFakraOrder(AF.FakraNo) as MyOrder 
From AhkamFakarat AF



Go 





