IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Pages]') AND type in (N'U'))
BEGIN
DROP TABLE [Pages];
END

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SiteMap_Labels]') AND type in (N'U'))
BEGIN
DROP TABLE [SiteMap_Labels];
END

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SiteMap]') AND type in (N'U'))
BEGIN
DROP TABLE [SiteMap];
END

CREATE TABLE [dbo].[SiteMap](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[site_guid] VARCHAR(36) REFERENCES Subscriptions(guid),
	[url_hash] VARCHAR(64) NOT NULL,
	[url] [varchar](4096) NOT NULL,
	[parent] [varchar](255) NULL,
	[depth] [int] NULL DEFAULT ((0)),
	[position] [int] NULL DEFAULT ((0)),
	[redirect_to] [varchar](4096) NULL,
	[is_directory] [bit] NULL,
	[is_page] [bit] NULL,
	[is_redirect] [bit] NULL,
	[is_visible] [bit] NULL,
	PRIMARY KEY CLUSTERED([id]),
)
CREATE UNIQUE INDEX ON [SiteMap]([site_guid],[url_hash]);

CREATE TABLE [dbo].[SiteMap_Labels](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[guid] VARCHAR(36) NOT NULL,
	[sitemap_id] [int] NULL,
	[culture] [varchar](10) NULL,
	[label] [varchar](128) NULL,
	PRIMARY KEY CLUSTERED([id]),
);
CREATE UNIQUE INDEX idx_guid ON [SiteMap_Labels]([guid]);
CREATE UNIQUE INDEX idx_sitemaplable_id_culture ON [SiteMap_Labels]([sitemap_id],[culture]);

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
	[path] VARCHAR(4096),
	[path_hash] VARCHAR(64),
	PRIMARY KEY CLUSTERED([id]),
);
CREATE INDEX idx_page_path ON Pages([site_guid],[path_hash]);
CREATE UNIQUE INDEX idx_page_path_culture ON Pages([site_guid],[path_hash],[culture]);
CREATE INDEX idx_page_guid ON Pages([guid]);
