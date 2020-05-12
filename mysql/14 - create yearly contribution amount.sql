CREATE VIEW `display_yearly_contribution_amount` AS
SELECT write_date, yearly_contribution_amount FROM stockplanner.yearly_contribution ORDER BY 1 DESC LIMIT 1;