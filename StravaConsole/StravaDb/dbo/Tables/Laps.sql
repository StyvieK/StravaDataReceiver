USE [Strava]
GO

/****** Object:  Table [dbo].[Laps]    Script Date: 09/10/2018 11:06:00 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Laps](
	[ActivityId] [int] NOT NULL,
	[LapId] [int] NOT NULL,
	[Start] [datetime] NULL,
	[Id] [int] NULL,
	[ResourceState] [int] NULL,
	[Name] [varchar](50) NULL,
	[ElapsedTime] [int] NULL,
	[MovingTime] [int] NULL,
	[ElapsedTimespan] [int] NULL,
	[MovingTimespan] [int] NULL,
	[StartLocal] [datetime] NULL,
	[MaxHeartRate] [decimal](18, 2) NULL,
	[Distance] [decimal](18, 2) NULL,
	[StartIndex] [int] NULL,
	[EndIndex] [int] NULL,
	[TotalElevationGain] [decimal](18, 2) NULL,
	[AveSpeed] [decimal](18, 2) NULL,
	[MaxSpeed] [decimal](18, 2) NULL,
 CONSTRAINT [PK_Laps] PRIMARY KEY CLUSTERED 
(
	[ActivityId] ASC,
	[LapId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


