-- Run this against sales_inventory_db to add the users table
-- ============================================================
USE sales_inventory_db;

CREATE TABLE IF NOT EXISTS users (
    user_id       INT AUTO_INCREMENT PRIMARY KEY,
    username      VARCHAR(50)  NOT NULL UNIQUE,
    email         VARCHAR(100) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    full_name     VARCHAR(100) NOT NULL,
    role          ENUM('admin','staff') NOT NULL DEFAULT 'staff',
    status        ENUM('active','inactive') NOT NULL DEFAULT 'active',
    created_at    DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at    DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- Default admin  (password: admin123)
INSERT IGNORE INTO users (username, email, password_hash, full_name, role, status)
VALUES ('admin', 'admin@brewandbite.com', SHA2('admin123',256), 'Administrator', 'admin', 'active');
