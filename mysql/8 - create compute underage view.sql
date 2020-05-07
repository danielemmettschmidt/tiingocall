CREATE VIEW `compute_underage` AS
SELECT 	stock, 
		(target_percent - current_percent) as 'underage'
FROM stockplanner.compute_portfolio
WHERE((target_percent - current_percent) > 0)
ORDER BY 2 DESC;