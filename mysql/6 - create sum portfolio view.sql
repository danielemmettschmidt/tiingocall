CREATE VIEW `sum_portfolio` AS
SELECT *,  (SELECT SUM(current_value) FROM stockplanner.consolidate_portfolio) as 'portfolio_sum'
FROM stockplanner.consolidate_portfolio;