IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Registrations]') AND type in (N'U'))
BEGIN
DROP TABLE [Registrations];
END
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[User_Subscriptions]') AND type in (N'U'))
BEGIN
DROP TABLE [User_Subscriptions];
END
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Subscriptions]') AND type in (N'U'))
BEGIN
DROP TABLE [Subscriptions];
END
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Subscription_Plans]') AND type in (N'U'))
BEGIN
DROP TABLE [Subscription_Plans];
END
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
BEGIN
DROP TABLE [Users];
END


CREATE TABLE [Registrations] (
[id] INT IDENTITY(1,1) NOT NULL,
[guid] VARCHAR(36) NOT NULL,
[created] DATETIME NOT NULL,
[email] NVARCHAR(128) NOT NULL,
[is_complete] BIT DEFAULT(0),
[data] VARBINARY(MAX) NOT NULL,
PRIMARY KEY CLUSTERED([id]),
);
CREATE UNIQUE INDEX uq_registration_guid ON [Registrations](guid);

CREATE TABLE [Subscription_Plans] (
[id] INT IDENTITY(1,1) NOT NULL,
[sku] VARCHAR(25),
[name] VARCHAR(128) NOT NULL,
[price] DECIMAL NOT NULL,
[available] BIT DEFAULT(0),
PRIMARY KEY CLUSTERED([id]),
);

CREATE TABLE [Subscriptions] (
[id] INT IDENTITY(1,1) NOT NULL,
[subscription_type] INT NOT NULL,
[subscription_plan] INT NOT NULL,
[guid] VARCHAR(36) NOT NULL,
[primary_user_guid] VARCHAR(36) NOT NULL,
[subdomain] VARCHAR(256),
[custom_domain] VARCHAR(256),
[staging_domain] VARCHAR(256),
[signup_date] DATETIME DEFAULT(GETDATE()),
[expires] DATETIME,
[disabled] BIT DEFAULT(0),
[salesforce_addon] BIT DEFAULT(0),
[generic_addon] BIT DEFAULT(0),
PRIMARY KEY CLUSTERED([id]),
);
CREATE UNIQUE INDEX uq_subscriptions_guid ON [Subscriptions](guid);
CREATE UNIQUE INDEX uq_subscriptions_subdomain ON [Subscriptions](subdomain);
CREATE UNIQUE INDEX uq_subscriptions_customdomain ON [Subscriptions](custom_domain);
CREATE UNIQUE INDEX uq_subscriptions_stagingdomain ON [Subscriptions](staging_domain);

CREATE TABLE [Users] (
[id] INT IDENTITY(1,1) NOT NULL,
[created] DATETIME NOT NULL,	
[guid] VARCHAR(36) NOT NULL,
[username] NVARCHAR(64),
[email] NVARCHAR(128),
[firstname] NVARCHAR(128),
[lastname] NVARCHAR(128),
[company] NVARCHAR(128),
[address1] NVARCHAR(128),
[address2] NVARCHAR(128),
[city] NVARCHAR(128),
[state] NVARCHAR(10),
[zipcode] NVARCHAR(10),
[country] NVARCHAR(10)
PRIMARY KEY CLUSTERED ([id])
);
CREATE INDEX idx_users_guid ON [Users](guid);
CREATE INDEX idx_users_username ON [Users](username);

CREATE TABLE [User_Subscriptions] (
[user_id] INT NOT NULL REFERENCES Users(id),
[subscription_id] INT NOT NULL REFERENCES Subscriptions(id)
PRIMARY KEY CLUSTERED ([user_id],[subscription_id])
);

CREATE TABLE [Configurations] (
[id] INT IDENTITY(1,1) NOT NULL,
[name] VARCHAR(256) NOT NULL,
[value] NTEXT NOT NULL
PRIMARY KEY CLUSTERED ([id])
);
CREATE INDEX idx_configurations_key ON [Configurations](key);

/* Run from master db */
CREATE LOGIN gooeycms
    WITH PASSWORD = '12qwas#$ERDF';

/* run from gooeycms and aspnetdb */
CREATE USER gooeycms FOR LOGIN gooeycms;
EXEC sp_addrolemember N'db_owner', N'gooeycms'

/* setup the default configuration values */
insert into configurations values ('salesforce-price','50');
insert into configurations values ('default-cms-domain','.gooeycms.net');
insert into configurations values ('logging-enabled','false');
insert into configurations values ('subscription-processor','Gooeycms.Business.Subscription.Debug.DebugSubscriptionProcessor');