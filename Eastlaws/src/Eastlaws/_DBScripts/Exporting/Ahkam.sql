USE iglc

GO 

SELECT * FROM EastlawsData.sys.tables t
ORDER BY name




--Countries 

DELETE FROM EastlawsData..Countries 
WHERE ID NOT IN (SELECT ID FROM dbo.Country c)

GO 

UPDATE D
SET D.Name = S.Name , D.EnName = S.EnText
From 
EastlawsData..Countries D 
JOIN dbo.Country  S ON S.ID = D.ID
WHERE Binary_Checksum(Cast(S.Name AS nvarchar(max)) , Cast( S.EnText AS nvarchar(max))) <> Binary_Checksum(D.Name , D.EnName	)

GO 

INSERT INTO EastlawsData..Countries(ID,Name,EnName,FlagPic,IsMasterCountry,Abbrev,AbbrevSlang)
SELECT ID, Name, EnText, Null , 0 , Null , Null From dbo.Country c
WHERE ID NOT IN (SELECT ID FROM EastlawsData..Countries )

GO 




--ServiceCountries 
TRUNCATE TABLE EastlawsData..CountriesServices

INSERT INTO EastlawsData..CountriesServices(ServiceID,CountryID,MyOrder,PriceM)
SELECT 1 , ID , myOrder, null From dbo.Country c WHERE c.Show_In_Ahkam = 1 
UNION ALL 
SELECT 2 , ID , myOrder, null From dbo.Country c WHERE c.Show_In_Tash = 1 
UNION ALL 
SELECT 3 , ID , myOrder, null From dbo.Country c WHERE c.Show_In_FatwaMG = 1 


GO 





--Mahakem 
DELETE FROM EastlawsData..AhkamMahakem WHERE ID NOT IN (
SELECT ID FROM dbo.AH_Ma7akem am
)

GO 

UPDATE D 
SET D.EnName = S.EnText , D.CountryID = S.Country_ID , D.MyOrder = S.MyOrder , D.Name  =S.Name
From 
dbo.AH_Ma7akem S 
JOIN EastlawsData..AhkamMahakem D  ON S.ID = D.ID
WHERE Binary_CheckSum(D.CountryID,D.Name,D.EnName,D.MyOrder) <>  Binary_CheckSum(S.Country_ID, Cast(S.Name AS nvarchar(max)), Cast(S.EnText AS nvarchar(max)),S.MyOrder)


GO 

INSERT INTO EastlawsData..AhkamMahakem(ID,CountryID,Name,EnName,MyOrder)
SELECT  ID, Country_ID, Name , EnText, MyOrder  FROM dbo.AH_Ma7akem am
WHERE ID NOT IN (SELECT ID FROM EastlawsData..AhkamMahakem )


GO 




--Ahkam
--Needs Some Checking !!! 

DELETE FROM EastlawsData..Ahkam WHERE ID NOT IN (SELECT ID FROM dbo.AH_Master am)

GO 

UPDATE D 
SET  D.MahkamaID = S.Ma7kama_ID ,D.CountryID = S.My_Coumtry_ID ,D.CaseNo = S.Case_No ,D.CaseYear = D.CaseYear
,D.OfficeYear = S.Office_Year , D.OfficeSuffix = S.Office_Sufix ,D.PageNo  = S.Page_No ,D.PartNo = S.Part_No ,D.IfAgree = S.IF_Agree ,D.CaseDate = S.Case_Date
From EastlawsData..Ahkam D 
JOIN dbo.AH_Master S ON S.ID = D.ID
WHERE 
Binary_CheckSum(D.MahkamaID,D.CountryID,D.CaseNo,D.CaseYear,D.OfficeYear, Cast(D.OfficeSuffix AS nvarchar(max) ),D.PageNo,D.PartNo,D.IfAgree,D.CaseDate)
<>
Binary_CheckSum (S.Ma7kama_ID,S.My_Coumtry_ID,S.Case_No,S.Case_Year,S.Office_Year,Cast( S.Office_Sufix as nvarchar(max)),S.Page_No,S.Part_No,S.IF_Agree,Cast( S.Case_Date AS Date)) 

GO 

INSERT INTO EastlawsData..Ahkam(ID,MahkamaID,CountryID,CaseNo,CaseYear,OfficeYear,OfficeSuffix,PageNo,PartNo,IfAgree,CaseDate)
SELECT  S.ID, Ma7kama_ID , My_Coumtry_ID, Case_No, Case_Year, Office_Year, Office_Sufix,  Page_No , Part_No, IF_Agree  ,Case_Date FROM dbo.AH_Master S
WHERE ID NOT IN (SELECT ID FROM EastlawsData..Ahkam)


GO 





--AhkamFakarat

DELETE FROM EastlawsData..AhkamFakarat WHERE ID  NOT IN 
(SELECT ID FROM dbo.AH_SubMaster S)

GO 

UPDATE D 
SET D.HokmID = S.Master_ID , D.FakraNo = S.Fakra_No , D.Text = S.Fakra_Text , D.EnText = S.EnText
From 
EastlawsData..AhkamFakarat D 
JOIN dbo.AH_SubMaster S ON S.ID = D.ID
WHERE 
Binary_CheckSum(D.HokmID,D.FakraNo,D.Text,D.EnText)
<> 
Binary_CheckSum(S.Master_ID,S.Fakra_No, Cast(S.Fakra_Text AS nvarchar(max)) ,Cast( S.EnText AS nvarchar(max)))

GO 


INSERT INTO EastlawsData..AhkamFakarat(ID,HokmID,FakraNo,Text,EnText)
SELECT ID , Master_ID , Fakra_No , Fakra_Text , EnText  FROM dbo.AH_SubMaster S 
WHERE ID NOT IN (SELECT ID FROM EastlawsData..AhkamFakarat)






Go 


Exec EastlawsData..ExportServicesSort


