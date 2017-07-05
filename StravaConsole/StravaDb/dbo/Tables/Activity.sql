CREATE TABLE [dbo].[Activity] (
    [ActivityId]       INT             NOT NULL,
    [Name]             VARCHAR (255)   NULL,
    [Date]             DATE            NULL,
    [Distance]         DECIMAL (18, 2) NULL,
    [ActivityType]     VARCHAR (50)    NULL,
    [GearId]           VARCHAR (50)    NULL,
    [AverageCadence]   DECIMAL (18, 2) NULL,
    [AverageHeartRate] DECIMAL (18, 2) NULL,
    [TimeStarted]      DATETIME        NULL,
    [TimeEnded]        DATETIME        NULL,
    [ElevationGain]    DECIMAL (18, 2) NULL,
    CONSTRAINT [PK_Activity] PRIMARY KEY CLUSTERED ([ActivityId] ASC)
);

