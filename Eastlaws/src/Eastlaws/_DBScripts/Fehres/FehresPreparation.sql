Use iglc

Go 




Select * From FehresPrograms
Select * From FaharessProgCountry
Select * From General_Category
Select * From General_Fehres
-----------------------------------------------------------
Select top 100 * From General_Fehres_AH_Tash
Select top 100 * From General_Fehres_Details
Select * From General_Fehres_Link
Select * From General_Fehres_Link_ByDetail
-----------------------------------------------------------
Select * From General_Fehres_Rules
Select * From General_Fehres_Rules_Details
Select * From General_Fehres_Text_Details
Select * From General_Fehres_Text
Select * From General_FehresImages


Select Text  , Count(*) From General_Fehres
Where Text Like '%ﬁ’Ê—%'
Group By  Text
Having Count(*) > 1 
ORder By Count(*) desc 


Select GF.* , GC.Name , FP.Name , CL.Name From General_Fehres GF 
Join General_Category GC on GC.ID = GF.Master_ID
Join FaharessProgCountry FPG on FPG.ID = GC.Master_ID
Join FehresPrograms FP on FP.ID = FPG.Master_ID
Join Country CL on CL.ID = FPG.Country_ID
Where GF.Text = N'«·ﬁ’Ê— ›Ì «· ”»Ì»' 
