
CREATE PROCEDURE [dbo].[api_UpdateRouteDistance]
	@RouteId BIGINT,
	@Distance INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE dbo.GeoRoute
		SET Distance = @Distance
	WHERE Id = @RouteId
END
