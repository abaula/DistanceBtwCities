CREATE PROCEDURE [dbo].[api_GetDistancePageForCity]
	@cityId bigint,
	@maxDistance int,
	@offset int, 
	@rows int	
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @rowsCount int
	
	-- 1. Подсчитываем общее количество записей
	SELECT
		@rowsCount = COUNT(*)
	FROM [dbo].[geoRoute] R
		INNER JOIN dbo.geoCities C1
			ON C1.id = R.cityId1
		INNER JOIN dbo.geoCities C2
			ON C2.id = R.cityId2
	WHERE R.distance <= @maxDistance
		AND (C1.id = @cityId OR C2.id = @cityId)

	-- 2. Возврашаем страницу записей
	SELECT
		T.rowNumber,
		T.id AS geoRouteId,
		T.cityId1,
		T.cityName1,
		T.cityfullName1,
		T.cityLatitude1,
		T.cityLongitude1,
		T.cityId2,
		T.cityName2,
		T.cityfullName2,
		T.cityLatitude2,
		T.cityLongitude2,
		T.distance
	FROM
		(      
			SELECT 
				ROW_NUMBER() OVER (ORDER BY C1.name, C2.name) AS rowNumber,
				R.id,
				R.cityId1,
				C1.name AS cityName1,
				C1.fullname AS cityfullName1,
				C1.latitude AS cityLatitude1,
				C1.longitude AS cityLongitude1,
				R.cityId2,
				C2.name AS cityName2,
				C2.fullname AS cityfullName2,
				C2.latitude AS cityLatitude2,
				C2.longitude AS cityLongitude2,
				R.distance
			FROM
			-- создаём список дистанций "R", в котором совпадающий с запросом город будет на 1-й позиции из 2-х.
			(				
				SELECT
					id = ISNULL(A.id, B.id),
					cityId1 = ISNULL(A.cityId1, B.cityId2),
					cityId2 = ISNULL(A.cityId2, B.cityId1),
					distance = ISNULL(A.distance, B.distance)
				FROM
				(
					SELECT
						R1.id,
						R1.cityId1,
						R1.cityId2,
						R1.distance
					FROM [dbo].[geoRoute] R1
						INNER JOIN dbo.geoCities C1
							ON C1.id = R1.cityId1
					WHERE R1.distance <= @maxDistance
						AND C1.id = @cityId
				) A
				FULL OUTER JOIN
				(
					SELECT
						R2.id,
						R2.cityId1,
						R2.cityId2,
						R2.distance
					FROM [dbo].[geoRoute] R2
						INNER JOIN dbo.geoCities C2
							ON C2.id = R2.cityId2
					WHERE R2.distance <= @maxDistance
						AND C2.id = @cityId
				) B	
					ON B.id = A.id
			) R
				INNER JOIN dbo.geoCities C1
					ON C1.id = R.cityId1
				INNER JOIN dbo.geoCities C2
					ON C2.id = R.cityId2
		) AS T
	 WHERE T.rowNumber > @offset
		AND T.rowNumber <= (@offset + @rows)
		

	-- 3. Возвращаем общее количество записей
	RETURN @rowsCount
END
