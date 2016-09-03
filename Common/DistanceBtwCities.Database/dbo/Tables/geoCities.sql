CREATE TABLE [dbo].[geoCities] (
    [id]         BIGINT         NOT NULL,
    [latitude]   INT            NOT NULL,
    [longitude]  INT            NOT NULL,
    [name]       NVARCHAR (255) NOT NULL,
    [district]   NVARCHAR (255) NOT NULL,
    [region]     NVARCHAR (255) NOT NULL,
    [suffix]     NVARCHAR (255) NOT NULL,
    [cladr_code] NCHAR (13)     NOT NULL,
    [postcode]   NCHAR (10)     NULL,
    [fullname]   NVARCHAR (500) NOT NULL,
    CONSTRAINT [PK_geoCities] PRIMARY KEY CLUSTERED ([id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_geoCities_name]
    ON [dbo].[geoCities]([name] ASC)
    INCLUDE([id]);

