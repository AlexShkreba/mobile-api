USE MobileAppDB;
GO

ALTER TABLE [Tasks_By_Categories] WITH CHECK ADD CONSTRAINT [Tasks_By_Categories_fk0] FOREIGN KEY ([id_category]) REFERENCES [Categories]([id])
ON UPDATE CASCADE
GO
ALTER TABLE [Tasks_By_Categories] CHECK CONSTRAINT [Tasks_By_Categories_fk0]
GO
ALTER TABLE [Tasks_By_Categories] WITH CHECK ADD CONSTRAINT [Tasks_By_Categories_fk1] FOREIGN KEY ([id_task]) REFERENCES [Tasks]([id])
ON UPDATE CASCADE
GO
ALTER TABLE [Tasks_By_Categories] CHECK CONSTRAINT [Tasks_By_Categories_fk1]
GO
ALTER TABLE [Tasks_By_Categories] WITH CHECK ADD CONSTRAINT [Tasks_By_Categories_fk2] FOREIGN KEY ([id_user]) REFERENCES [Users]([id])
ON UPDATE CASCADE
GO
ALTER TABLE [Tasks_By_Categories] CHECK CONSTRAINT [Tasks_By_Categories_fk2]
GO