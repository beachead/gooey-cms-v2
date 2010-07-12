IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Pages]') AND type in (N'U'))
BEGIN
DROP TABLE [Pages];
END

CREATE TABLE Pages (
	[id] INT IDENTITY(1,1) NOT NULL,
	[guid] VARCHAR(36) NOT NULL,
	[site_guid] VARCHAR(36) REFERENCES Subscriptions(guid),
	[culture] [varchar](10) NULL,
	[date_saved] [datetime] NULL,
	[author] [varchar](128) NULL,
	[is_approved] [bit] NULL,
	[approved_by] [varchar](128) NULL,
	[title] [nvarchar](255) NOT NULL,
	[description] [nvarchar](1024) NULL,
	[keywords] [nvarchar](1024) NULL,
	[bodyload] [varchar](1000) NULL,
	[template] VARCHAR(36),
	[path] VARCHAR(2048),
);
CREATE INDEX idx_page_path ON Pages([site_guid],[path]);
CREATE INDEX idx_page_path_culture ON Pages([site_guid],[path],[culture]);
CREATE INDEX idx_page_guid ON Pages([guid]);
