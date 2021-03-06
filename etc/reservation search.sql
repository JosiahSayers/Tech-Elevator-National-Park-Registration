DECLARE @fromDate Date;
DECLARE @toDate Date;
DECLARE @campgroundId INT;
SET @campgroundId = 130;
SET @fromDate = '2019-02-21';
set @toDate = '2019-02-23';


SELECT TOP 5 site.site_id, site.site_number, site.max_occupancy, site.accessible, site.max_rv_length, site.utilities, campground.daily_fee 
FROM site 
LEFT JOIN reservation ON reservation.site_id = site.site_id 
JOIN campground ON site.campground_id = campground.campground_id 
WHERE campground.campground_id = @campgroundId AND 
reservation.reservation_id IS NULL 
OR NOT(@fromDate > reservation.from_date AND @fromDate<reservation.to_date) 
AND NOT(@toDate > reservation.from_date AND @toDate<reservation.to_date) 
AND NOT(reservation.from_date > @fromDate AND reservation.from_date<@toDate) 
AND NOT(reservation.to_date > @fromDate AND reservation.to_date<@toDate);
