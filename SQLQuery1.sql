-- Top-level parent account (e.g., Asset category)
INSERT INTO ChartOfAccount (Name, Email, PhoneNumber, ParentId, IsActive, CreatedAt, CreatedBy)
VALUES ('Assets', 'assets@coa.com', '01700000001', NULL, 1, GETDATE(), 'admin');

-- Child account under Assets
INSERT INTO ChartOfAccount (Name, Email, PhoneNumber, ParentId, IsActive, CreatedAt, CreatedBy)
VALUES ('Cash', 'cash@coa.com', '01700000002', 1, 1, GETDATE(), 'admin');

-- Another child under Assets
INSERT INTO ChartOfAccount (Name, Email, PhoneNumber, ParentId, IsActive, CreatedAt, CreatedBy)
VALUES ('Bank Account', 'bank@coa.com', '01700000003', 1, 1, GETDATE(), 'admin');

-- New parent category
INSERT INTO ChartOfAccount (Name, Email, PhoneNumber, ParentId, IsActive, CreatedAt, CreatedBy)
VALUES ('Expenses', 'expenses@coa.com', '01700000004', NULL, 1, GETDATE(), 'admin');

-- Child under Expenses
INSERT INTO ChartOfAccount (Name, Email, PhoneNumber, ParentId, IsActive, CreatedAt, CreatedBy)
VALUES ('Office Supplies', 'supplies@coa.com', '01700000005', 4, 1, GETDATE(), 'admin');

-- Another child under Expenses
INSERT INTO ChartOfAccount (Name, Email, PhoneNumber, ParentId, IsActive, CreatedAt, CreatedBy)
VALUES ('Utilities', 'utilities@coa.com', '01700000006', 4, 1, GETDATE(), 'admin');
