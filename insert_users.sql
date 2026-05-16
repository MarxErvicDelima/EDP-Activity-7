USE edp_system;

INSERT INTO users (Username, FirstName, LastName, Email, PasswordHash, IsActive) VALUES 
('admin', 'Admin', 'User', 'admin@edp.local', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 1),
('user1', 'John', 'Doe', 'john@edp.local', '5f4dcc3b5aa765d61d8327deb882cf99', 1),
('user2', 'Jane', 'Smith', 'jane@edp.local', '5f4dcc3b5aa765d61d8327deb882cf99', 1);
