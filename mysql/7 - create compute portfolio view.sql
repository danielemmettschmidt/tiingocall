CREATE VIEW `compute_portfolio` AS
SELECT 	stock,
		TRUNCATE((current_value / quantity) * 10000,0) as price,
		quantity,
		current_value,
        target_percent,
        TRUNCATE((current_value / portfolio_sum) * 1000000,0) as current_percent
FROM stockplanner.sum_portfolio;