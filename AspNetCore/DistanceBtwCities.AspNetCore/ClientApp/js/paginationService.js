(function (ng, app)
{
    "use strict";
    app.service("paginationService", ["appConstants", function (appConstants)
    {
        var buildPaginationData = function(currentPageNum, allPagesNum)
        {
            var paginationHalfSize = Math.ceil(appConstants.paginationSize / 2);
            var startPageNum = Math.max(0, currentPageNum - paginationHalfSize);
            var stopPageNum = Math.min(allPagesNum + 1, startPageNum + appConstants.paginationSize);
            startPageNum = Math.min(startPageNum, stopPageNum - appConstants.paginationSize);
            startPageNum = Math.max(0, startPageNum);

            var paginationData = [];

            for (var i = startPageNum; i < stopPageNum; i++)
            {
                var paginationInfo = {
                    pageNumber: i,
                    isFirst: i == startPageNum,
                    isLast: i == stopPageNum,
                    isCurrent: i == currentPageNum
                };

                paginationData.push(paginationInfo);
            }

            return {
                firstPageNum: 0,
                firstPageEnabled: currentPageNum > 0,
                previousPageNum: Math.max(0, currentPageNum - appConstants.paginationSize),
                previousPageEnabled: currentPageNum > paginationHalfSize,
                lastPageNum: allPagesNum,
                lastPageEnabled: currentPageNum < allPagesNum,
                nextPageNum: Math.min(allPagesNum, currentPageNum + appConstants.paginationSize),
                nextPageEnabled: currentPageNum < (allPagesNum + 1) - paginationHalfSize,
                pages: paginationData
            };
        }

        return {
            buildPaginationData: buildPaginationData
        };
    }]);
})(angular, angular.module("distanceBtwCities"));