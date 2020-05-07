CREATE TABLE `stockplanner`.`source` (
  `stock` VARCHAR(6) NOT NULL,
  `current_value` INT NOT NULL,
  `quantity` INT NOT NULL,
  `write_date` DATETIME NOT NULL,
  PRIMARY KEY (`stock`));
