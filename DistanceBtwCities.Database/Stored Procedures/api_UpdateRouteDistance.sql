CREATE PROCEDURE [dbo].[api_UpdateRouteDistance]
	@routeId bigint,
	@distance int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE dbo.geoRoute
		SET distance = @distance
	WHERE id = @routeId
END
