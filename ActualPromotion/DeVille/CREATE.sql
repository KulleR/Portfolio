CREATE TABLE [dbo].[ServiceCategories] (
    [Id] INT IDENTITY (1, 1) NOT NULL,
    [Name] VARCHAR (127) NOT NULL,
    [Description] VARCHAR (MAX) NULL,
    [ImgCover] VARCHAR(255) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Services] (
    [Id] INT IDENTITY (1, 1) NOT NULL,
	[CategoryId] INT NOT NULL,
    [Name] VARCHAR (127) NOT NULL,
    [Description] VARCHAR (MAX) NULL,
    [VirtuosoPrice] MONEY NULL,
	[ExpertPrice] MONEY NULL,
	[HandymanPrice] MONEY NULL,
	[Price] MONEY NULL,
	[ImgCover] VARCHAR(255) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
	FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[ServiceCategories] ([Id])
);

CREATE TABLE [dbo].[Vacancies] (
    [Id] INT IDENTITY (1, 1) NOT NULL,
    [Title] VARCHAR (127) NOT NULL,
    [Description] VARCHAR (MAX) NULL,
    [ImgCover] VARCHAR(255) NOT NULL,
	[Demands] VARCHAR(255) NULL,
	[Duties] VARCHAR(255) NULL,
	[Ñondition] VARCHAR(255) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[ProductCategories] (
    [Id] INT IDENTITY (1, 1) NOT NULL,
    [Title] VARCHAR (127) NOT NULL,
    [Description] VARCHAR (MAX) NULL,
    [ImgCover] VARCHAR(255) NOT NULL,
	[Status] INT NOT NULL DEFAULT 5,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Products] (
    [Id] INT IDENTITY (1, 1) NOT NULL,
    [Title] VARCHAR (127) NOT NULL,
    [Description] VARCHAR (MAX) NULL,
    [ImgCover] VARCHAR(255) NOT NULL,
	[Article] VARCHAR(255) NULL,
	[Price] MONEY NULL,
	[CategoryId] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[KindsOfActivity] (
    [Id] INT IDENTITY (1, 1) NOT NULL,
	[Activity] VARCHAR(255) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Employees] (
    [Id] INT IDENTITY (1, 1) NOT NULL,
    [FullName] VARCHAR (255) NOT NULL,
	[ActivityId] INT NOT NULL,
	[Email] VARCHAR(255) NOT NULL,
    [Phone] VARCHAR (255) NULL,
	[Birthday] VARCHAR(255) NULL,
	[Photo] VARCHAR NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
	FOREIGN KEY ([ActivityId]) REFERENCES [dbo].[KindsOfActivity] ([Id])
);

CREATE TABLE [dbo].[News] (
    [Id] INT IDENTITY (1, 1) NOT NULL,
    [Title] VARCHAR (255) NOT NULL,
	[Text] VARCHAR(255) NOT NULL,
	[CreateDate] DATETIME NOT NULL,
    [ImgCover] VARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Reviews] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [CreateDate]    DATETIME      NOT NULL,
    [Title]         VARCHAR (255) NOT NULL,
    [Message]		VARCHAR (MAX) NOT NULL,
    [AuthorFullName] VARCHAR (255) NOT NULL,
	[AuthorPhoto] VARCHAR(511) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[OnlineAppointments] (
    [Id] INT IDENTITY (1, 1) NOT NULL,
	[ServiceId] INT NOT NULL,
    [CreateDate] DATETIME NOT NULL,
    [AuthorName] VARCHAR (255) NOT NULL,
    [Phone] VARCHAR (255) NOT NULL,
    [Email] VARCHAR (255) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
	FOREIGN KEY ([ServiceId]) REFERENCES [dbo].[Services] ([Id])
);

CREATE TABLE [dbo].[PhotoGalleries] (
    [Id] INT IDENTITY (1, 1) NOT NULL,
	[Name] VARCHAR(255) NOT NULL,
    [ImgCover] VARCHAR(255) NOT NULL,
	[Status] INT NOT NULL DEFAULT 5,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[PhotoAlbum] (
    [Id] INT IDENTITY (1, 1) NOT NULL,
	[Name] VARCHAR(255) NOT NULL,
    [ImgCover] VARCHAR(255) NOT NULL,
	[GalleryId] INT NOT NULL,
	[Status] INT NOT NULL DEFAULT 5,
    PRIMARY KEY CLUSTERED ([Id] ASC),
	FOREIGN KEY([GalleryId]) REFERENCES PhotoGalleries(Id)
);


CREATE TABLE [dbo].[Photos] (
    [Id] INT IDENTITY (1, 1) NOT NULL,
	[Name] VARCHAR(255) NOT NULL,
    [ImgUrl] VARCHAR(255) NOT NULL,
	[AlbumId] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
	FOREIGN KEY([AlbumId]) REFERENCES [PhotoAlbum](Id)
);

CREATE TABLE [dbo].[Options] (
    [Id] INT IDENTITY (1, 1) NOT NULL,
	[OptionName] VARCHAR(255) NOT NULL,
    [OptionValue] VARCHAR(255) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Posts] (
    [Id] INT IDENTITY (1, 1) NOT NULL,
	[CreateDate] DATETIME NOT NULL,
	[Content] TEXT NOT NULL,
	[Title] VARCHAR(511) NOT NULL,
	[Status] INT NOT NULL DEFAULT 5,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Users] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Email]    VARCHAR (127) NOT NULL,
    [Password] VARCHAR (127) NOT NULL,
	[RoleId]   INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
	FOREIGN KEY(RoleId) REFERENCES Roles(Id)
);

CREATE TABLE [dbo].[Roles] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [RoleName]     VARCHAR (127) NOT NULL,
    [CyrillicName] VARCHAR (127) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);