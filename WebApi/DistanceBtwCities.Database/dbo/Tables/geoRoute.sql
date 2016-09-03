CREATE TABLE [dbo].[geoRoute] (
    [id]       BIGINT NOT NULL,
    [cityId1]  BIGINT NOT NULL,
    [cityId2]  BIGINT NOT NULL,
    [distance] INT    DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_geoRoute] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_geoRoute_geoCities1] FOREIGN KEY ([cityId1]) REFERENCES [dbo].[geoCities] ([id]),
    CONSTRAINT [FK_geoRoute_geoCities2] FOREIGN KEY ([cityId2]) REFERENCES [dbo].[geoCities] ([id])
);


GO
CREATE NONCLUSTERED INDEX [IX_geoRoute_distance]
    ON [dbo].[geoRoute]([distance] ASC)
    INCLUDE([id], [cityId1], [cityId2]);


GO
CREATE NONCLUSTERED INDEX [IX_geoRoute_cityId1_distance]
    ON [dbo].[geoRoute]([cityId1] ASC, [distance] ASC)
    INCLUDE([id], [cityId2]);


GO
CREATE NONCLUSTERED INDEX [IX_geoRoute_cityId2_distance]
    ON [dbo].[geoRoute]([cityId2] ASC, [distance] ASC)
    INCLUDE([id], [cityId1]);

