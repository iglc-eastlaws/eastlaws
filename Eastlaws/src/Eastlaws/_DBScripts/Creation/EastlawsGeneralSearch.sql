
DROP DATABASE EastlawsGeneralSearch

GO 

if not Exists (Select 1 from sys.databases where name = 'EastlawsGeneralSearch')
Begin 
	Create DataBase EastlawsGeneralSearch 
End

Go

CREATE TABLE [dbo].[ProgName](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NULL,
	[myorder] [decimal](18, 0) NULL,
	[catgID] [int] NULL,
	[cagName] [nvarchar](100) NULL,
	[imgName] [nvarchar](100) NULL,
 CONSTRAINT [PK_ProgName] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


go

--- need to add rank when export data
-- Data_Text
-- cluster index in (progid + recid)
-- remove id column 
-- tash ProgID = 1   and ahkam progID = 2   
CREATE TABLE [dbo].[Data_1](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServiceID] [int] NULL,
	[ProgID] [int] NULL,
	[RecID] [int] NULL,
	[ParentID] [int] NULL,
	[Text] [nvarchar](max) NULL,
	[BinaryCheckSum] [int] NULL,
	[Year] [int] NULL,
	[Country] [int] NULL,
	[Ah_Makama] [int] NULL,
	[Tash_Type] [int] NULL,
	[Tash_No] [int] NULL,
 CONSTRAINT [PK_Data_1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


GO



