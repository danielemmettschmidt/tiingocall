SELECT 	m.stock, 
		s.current_value / 1000 as "current_value", 
		s.quantity / 1000 as "quantity"
FROM `stockplanner`.`source` as s
JOIN `stockplanner`.`view_manifest` as m
ON (m.stock is not null and m.stock = s.stock)
order by 'stock' asc;