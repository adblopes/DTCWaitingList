CREATE TABLE Reasons (
	ReasonId int PRIMARY KEY NOT NULL IDENTITY,
	ReasonName nvarchar(20) NOT NULL)
GO

CREATE TABLE Days (
	DayId int PRIMARY KEY NOT NULL IDENTITY,
	NameOfDay nvarchar(10) NOT NULL)
GO

CREATE TABLE Times (
	TimeId int PRIMARY KEY NOT NULL IDENTITY,
	TimeOfDay nvarchar(10) NOT NULL)
GO

CREATE TABLE Patients (
	PatientId int PRIMARY KEY NOT NULL IDENTITY,
	FullName nvarchar(70) NOT NULL,
	Email nvarchar(255) NOT NULL,
	Phone nvarchar(15) NOT NULL,
	FullReason nvarchar(500) NULL,
	ReasonId int NOT NULL,
	IsClient bit NOT NULL,
	CreatedDate Date NOT NULL,
	FOREIGN KEY (ReasonId) REFERENCES dbo.Reasons(ReasonId))
GO

CREATE TABLE Patients_History (
	PatientId int PRIMARY KEY NOT NULL IDENTITY,
	FullName nvarchar(70) NOT NULL,
	Email nvarchar(255) NOT NULL,
	Phone nvarchar(15) NOT NULL,
	FullReason nvarchar(500) NULL,
	Reason int NOT NULL,
	IsClient bit NOT NULL,
	CreatedDate Date NOT NULL,
	DeletedDate Date NOT NULL)
GO

CREATE TABLE PatientDays (
	PatientId int NOT NULL,
	DayId int NOT NULL,
	PRIMARY KEY (PatientId, DayId),
	FOREIGN KEY (DayId) REFERENCES dbo.Days(DayId),
	FOREIGN KEY (PatientId) REFERENCES dbo.Patients(PatientId))
GO

CREATE TABLE PatientTimes (
	PatientId int NOT NULL,
	TimeId int NOT NULL,
	PRIMARY KEY (PatientId, TimeId),
	FOREIGN KEY (TimeId) REFERENCES dbo.Times(TimeId),
	FOREIGN KEY (PatientId) REFERENCES dbo.Patients(PatientId))
GO

CREATE TABLE ReasonVariants (
	VariantId int PRIMARY KEY NOT NULL IDENTITY,
	Term nvarchar(20) NOT NULL,
	ReasonId int NOT NULL,
	FOREIGN KEY (ReasonId) REFERENCES dbo.Reasons(ReasonId))
GO

INSERT INTO [dbo].[Times]
		   ([TimeOfDay])
	 VALUES
		   ('Any Time'),
		   ('Morning'),
		   ('Noon'),
		   ('Afternoon'),
		   ('Evening')
GO

INSERT INTO [dbo].[Days]
		   ([NameOfDay])
	 VALUES
		   ('Any Day'),
		   ('Monday'),
		   ('Tuesday'),
		   ('Wednesday'),
		   ('Thursday'),
		   ('Friday'),
		   ('Saturday'),
		   ('Sunday')
GO

INSERT INTO [dbo].[Reasons]
		   ([ReasonName])
	 VALUES
		   ('Checkup'),
		   ('Cleaning'),
		   ('Pain')
GO

INSERT INTO [dbo].[ReasonVariants]
		   ([Term]
		   ,[ReasonId])
	 VALUES
		   ('Check-up', 1),
		   ('Check up', 1),
		   ('Checkup', 1),
		   ('Cleaning', 2),
		   ('Detartage', 2),
		   ('Détartage', 2),
		   ('Détártáge', 2),
		   ('Détártage', 2),
		   ('Dètartage', 2),
		   ('Dètártáge', 2),
		   ('Dètártàge', 2),
		   ('Dètàrtàge', 2),
		   ('Detartage', 2),
		   ('d?tartrage', 2),
		   ('nettoyage', 2),
		   ('Pain', 3),
		   ('douleur', 3),
		   ('douleurs', 3)
GO

INSERT INTO [dbo].[Patients]
		   ([FullName]
		   ,[Email]
		   ,[Phone]
		   ,[FullReason]
		   ,[Reason]
		   ,[isClient]
		   ,[CreatedDate])
	 VALUES
		   ('test',
		   'test@email.com',
		   '123',
		   'cleanup',
		   2,
		   1,
		   GETDATE())
GO

INSERT INTO [dbo].[PatientDays]
		   ([PatientId]
		   ,[DayId])
	 VALUES
		   (1,1)
GO

INSERT INTO [dbo].[PatientDays]
		   ([PatientId]
		   ,[DayId])
	 VALUES
		   (1,2)
GO

INSERT INTO [dbo].[PatientTimes]
		   ([PatientId]
		   ,[TimeId])
	 VALUES
		   (1,1)
GO
