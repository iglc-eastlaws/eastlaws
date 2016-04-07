Use iglc
---- Etfakat---------------
DELETE FROM EastlawsData..Etfakat WHERE ID NOT IN (SELECT ID FROM dbo.ETF_Master)

GO 

UPDATE Etf 
SET  
	Etf.ID=M.ID,
	Etf.Text=M.Text,
	Etf.Date=M.Etf_Date
From EastlawsData..Etfakat Etf
JOIN dbo.ETF_Master M ON M.ID = Etf.ID
WHERE 
Binary_CheckSum(Etf.ID,Etf.Text,Etf.Date)
<>
Binary_CheckSum (M.ID,M.Text,Etf_Date) 

GO 

INSERT INTO EastlawsData..Etfakat(ID,Text,Date)
SELECT  ID,Text,Etf_Date
FROM ETF_Master M
WHERE ID NOT IN (SELECT ID FROM EastlawsData..Etfakat)

-----------
---- EtfFakarat---------------
DELETE FROM EastlawsData..EtfFakarat WHERE ID NOT IN (SELECT ID FROM dbo.ETF_SubMaster)

GO 

UPDATE Etf 
SET  
	Etf.ID=Sub.ID,
	Etf.EtfID=Sub.Master_ID,
	Etf.Text=txt.Text,
	Etf.EnText=txt.Engtext,
	Etf.Title=Sub.Text,
	Etf.MyOrder=Sub.myOrder
	
From EastlawsData..EtfFakarat Etf
JOIN dbo.ETF_SubMaster Sub ON Sub.ID = Etf.ID
INNER JOIN dbo.ETF_Text txt ON Sub.Master_ID=txt.Master_ID AND Sub.ID=txt.SubMaster_ID
WHERE 
Binary_CheckSum(Etf.ID,Etf.Text,Etf.EtfID,Etf.EnText,Etf.Title,Etf.MyOrder)
<>
Binary_CheckSum (Sub.ID,txt.Text,Sub.Master_ID,txt.Engtext,Sub.Text,Sub.myOrder) 

GO 

INSERT INTO EastlawsData..EtfFakarat(ID,EtfID,MyOrder,Title,Text,EnText)
SELECT  Sub.ID,Sub.Master_ID,Sub.myOrder,Sub.Text,txt.Text,txt.Engtext
FROM dbo.ETF_SubMaster Sub 
inner JOIN dbo.ETF_Text txt ON Sub.Master_ID=txt.Master_ID AND Sub.ID=txt.SubMaster_ID
WHERE Sub.ID NOT IN (SELECT ID FROM EastlawsData..EtfFakarat)

-----------
-----ETFClasses---------
Delete From EastlawsData..EtfClasses 
Where ParentID <>0

Insert InTo EastlawsData..EtfClasses  (ID,ParentID,Text,MyOrder)
	Select ID,1000,Name,myOrder From dbo.ETF_Type
	union
	Select ID,1001,Name,myOrder From [iglc].dbo.ETF_Type_2
	union
	Select ID,1002,Name,myOrder From [iglc].dbo.ETF_Type_3
	union
	Select ID,1003,Name,myOrder From [iglc].dbo.ETF_Type_4
--------------
-----EtfClassMaster

Delete From EastlawsData..EtfClassMaster

Insert InTo EastlawsData..EtfClassMaster (ClassID,EtfID)
	Select Type_ID,Master_ID From [iglc].dbo.ETF_TypeMaster
	Union
	Select Type_ID,Master_ID From [iglc].dbo.ETF_TypeMaster_2
	Union
	Select Type_ID,Master_ID From [iglc].dbo.ETF_TypeMaster_3
	Union	
	Select Type_ID,Master_ID From [iglc].dbo.ETF_TypeMaster_4
	
----------EtfTash----------
DELETE FROM EastlawsData..EtfTash WHERE ID NOT IN (SELECT ID FROM dbo.ETF_TashMawaad)
Go

UPDATE Etf 
SET  
	Etf.ID=M.ID,
	Etf.EtfID=M.ETF_ID,
	Etf.TashID=M.Master_ID
From EastlawsData..EtfTash Etf
JOIN dbo.ETF_TashMawaad M ON M.ID = Etf.ID
WHERE 
Binary_CheckSum(Etf.ID,Etf.EtfID,Etf.TashID)
<>
Binary_CheckSum (M.ID,M.ETF_ID,Master_ID) 

go
INSERT INTO EastlawsData..EtfTash(ID,EtfID,TashID)
SELECT  M.ID,M.ETF_ID,Master_ID
FROM dbo.ETF_TashMawaad M
WHERE ID NOT IN (SELECT ID FROM EastlawsData..EtfTash)
----------------------
----EtfCountries
DELETE FROM EastlawsData..EtfCountries
Insert InTo EastlawsData..EtfCountries (ID,Name,MyOrder)
	Select Distinct C.ID,Name,MyOrder From [iglc].dbo.ETF_Countries C 
	INNER JOIN iglc.dbo.ETF_Signtures S ON C.ID=S.ETF_CountryID
	INNER JOIN iglc.dbo.ETF_Master M ON S.Master_ID=M.ID
	Where S.ETF_Date IS NOT NULL
------
----EtfSignatories
DELETE FROM EastlawsData..EtfSignatories WHERE ID NOT IN (SELECT ID FROM dbo.ETF_Signtures)
Go

UPDATE Etf 
SET  
	Etf.ID=M.ID,
	Etf.EtfID=M.Master_ID,
	Etf.CountryID=M.ETF_CountryID,
	Etf.Date=M.ETF_Date
From EastlawsData..EtfSignatories Etf
JOIN dbo.ETF_Signtures M ON M.ID = Etf.ID
WHERE 
Binary_CheckSum(Etf.ID,Etf.EtfID,Etf.CountryID,Etf.Date)
<>
Binary_CheckSum (M.ID,M.Master_ID,M.ETF_CountryID,M.ETF_Date) 

go
INSERT INTO EastlawsData..EtfSignatories(ID,EtfID,CountryID,Date)
Select ID,Master_ID,ETF_CountryID,ETF_Date From dbo.ETF_Signtures
	where ETF_Date IS NOT NULL
and ID NOT IN (SELECT ID FROM EastlawsData..EtfSignatories)
