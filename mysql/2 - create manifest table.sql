CREATE TABLE `stockplanner`.`manifest` (
  `stock` VARCHAR(6) NOT NULL,
  `target_percentage` INT NOT NULL,
  `write_date` DATETIME NOT NULL,
  PRIMARY KEY (`stock`));
