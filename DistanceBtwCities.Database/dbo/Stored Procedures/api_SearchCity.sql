                 
CREATE PROCEDURE [dbo].[api_SearchCity]
	@query NVARCHAR(255)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		id, 
		latitude, 
		longitude, 
		name, 
		district,
		region,
		suffix,
		cladr_code,
		postcode,
		fullname
	FROM dbo.geoCities
	WHERE name LIKE @query + '%'
		-- ищем только среди городов
		AND suffix = 'Город'
END

