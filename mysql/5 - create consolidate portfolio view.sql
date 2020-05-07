CREATE VIEW `consolidate_portfolio` AS
SELECT 	m.stock, 
		s.current_value as "current_value", 
		s.quantity as "quantity",
        m.target_percentage as "target_percent"
FROM `stockplanner`.`source` as s
JOIN `stockplanner`.`manifest` as m
ON (m.stock is not null and m.stock = s.stock)
order by 'stock' asc;