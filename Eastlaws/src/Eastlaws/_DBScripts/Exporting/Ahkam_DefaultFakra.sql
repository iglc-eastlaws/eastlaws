Use EastlawsData

Go 



Update AhkamFakarat 
Set IsDefault = null


Update AhkamFakarat 
Set IsDefault = 1 Where FakraNo = 1 


Update AhkamFakarat 
Set IsDefault = 1 
Where HokmID in (
	Select HokmID  From AhkamFakarat
	Group By HokmID
	Having Count(IsDefault) = 0 
)
And FakraNo = -1 



Update AhkamFakarat 
Set IsDefault = 1 
Where HokmID in (
	Select HokmID  From AhkamFakarat
	Group By HokmID
	Having Count(IsDefault) = 0 
)
And FakraNo = 0





Update AhkamFakarat 
Set IsDefault = 1 
Where HokmID in (
	Select HokmID  From AhkamFakarat
	Group By HokmID
	Having Count(IsDefault) = 0 
)
And FakraNo = -3 


Update AhkamFakarat 
Set IsDefault = 1 
Where HokmID in (
	Select HokmID  From AhkamFakarat
	Group By HokmID
	Having Count(IsDefault) = 0 
)
And FakraNo = -2 


Update AhkamFakarat 
Set IsDefault = 1 
Where HokmID in (
	Select HokmID  From AhkamFakarat
	Group By HokmID
	Having Count(IsDefault) = 0 
)
And FakraNo = -50 


Update A 
Set A.DefaultFakraID  = F.ID
From   Ahkam  A 
Join AhkamFakarat F  on F.HokmID  = A.ID and F.IsDefault = 1 

