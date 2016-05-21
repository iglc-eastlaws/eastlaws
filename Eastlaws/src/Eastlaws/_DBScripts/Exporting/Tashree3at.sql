use EastlawsData

Go 


Truncate Table EastlawsData..Tashree3at
Truncate Table EastlawsData..Tashree3atAhkam
Truncate Table EastlawsData..Tashree3atBassma
Truncate Table EastlawsData..Tashree3atFehres
Truncate Table EastlawsData..Tashree3atGarida
Truncate Table EastlawsData..Tashree3atMawad
Truncate Table EastlawsData..Tashree3atTypes


Use iglc

Go 

Insert Into  EastlawsData..Tashree3at 
	  (ID, CountryID, TypeID, BassmaID, GaridaID, TashNo, TashYear, TashDate, Name, EnName,  GaridaNumber, GaridaSuffix, MawadCount)
SELECT ID , My_Coumtry_ID , Type_ID , Bassma_ID , Garida_ID , Tash_No , Tash_Year , Tash_Date , Text , EnText ,Garida_No , Garida_suffix , Mawaad_Count   FROM Tash_Master

GO 

INSERT INTO EastlawsData..Tashree3atAhkam(ID,CountryID,TashID,MadaID,HokmID,FakraID,IsMana3y)
SELECT  at.ID , at.My_Coumtry_ID , at.Master_ID, at.Mada_ID , at.Ahkam_ID,  at.Fakra_ID, at.is_mana3y FROM dbo.AH_Tash at
--LEFT JOIN dbo.AH_SubMaster asm ON at.Ahkam_ID = asm.Master_ID AND asm.Fakra_No = at.Fakra_No

GO 

Insert Into EastlawsData..Tashree3atBassma (ID, CountryID, Name, EnName)
SELECT   ID ,isnull(My_Coumtry_ID , 0 ), Text,  EnText FROM dbo.Tash_Bassma tb

GO 


INSERT Into EastlawsData..Tashree3atFehres (ID, ParentID, Tashree3ID, Name, MyOrder)
SELECT ID, Parent_ID, Master_ID, Text, MyOrder From dbo.Tash_SubMaster tsm

GO 

INSERT INTO  EastlawsData..Tashree3atGarida(ID, CountryID, Name)
SELECT  ID, CountryID, Name FROM dbo.Tash_Garida tg

GO 


INSERT INTO EastlawsData..Tashree3atTypes (ID, CountryID, Name, EnName, MyOrder)
SELECT  ID, Country_ID, Name  , EnText , myOrder  FROM  dbo.Tash_Type tt

GO 

INSERT INTO  EastlawsData..Tashree3atMawad (ID, Tashree3ID, FehresID, MadaNo, MadaSuffix, Text, MyOrder, ImageID, EnText)
SELECT  ID,  Master_ID ,SubMaster_ID, Mada_No, Mada_Suffix, Text, myOrder, Image_ID, EnText  FROM dbo.Tash_Mawad tm

GO 

