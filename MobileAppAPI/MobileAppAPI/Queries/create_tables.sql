USE MobileAppDB;
GO

CREATE TABLE [Users] (
	[id] integer IDENTITY(1,1) NOT NULL,
	[login] nvarchar(255) NOT NULL,
	[password] nvarchar(255) NOT NULL
  CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED
  (
  [id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)
)
GO
CREATE TABLE [Categories] (
	[id] integer IDENTITY(1,1) NOT NULL,
	[name] nvarchar(255) NOT NULL,
	[type] nvarchar(255) NOT NULL
  CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED
  (
  [id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)
)
GO
CREATE TABLE [Tasks] (
	[id] integer IDENTITY(1,1) NOT NULL,
	[name] nvarchar(255) NOT NULL,
	[date_finish] date,
	[time_notification] datetime,
	[priority] bit NOT NULL,
	[repetition] bit NOT NULL,
	[state] bit NOT NULL
  CONSTRAINT [PK_Tasks] PRIMARY KEY CLUSTERED
  (
  [id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)
)
GO
CREATE TABLE [Tasks_By_Categories] (
	[id] integer IDENTITY(1,1) NOT NULL,
	[id_user] integer NOT NULL,
	[id_category] integer NOT NULL,
	[id_task] integer
  CONSTRAINT [PK_Tasks_By_Categories] PRIMARY KEY CLUSTERED
  (
  [id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)
)
GO