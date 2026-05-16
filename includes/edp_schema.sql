-- Database Schema for EDP GUI Information System
CREATE DATABASE IF NOT EXISTS edp_system;
USE edp_system;

-- System Stats
CREATE TABLE IF NOT EXISTS system_stats (
    id INT AUTO_INCREMENT PRIMARY KEY,
    stat_name VARCHAR(100) NOT NULL,
    stat_value VARCHAR(50) NOT NULL,
    stat_trend VARCHAR(20),
    stat_icon VARCHAR(50),
    stat_color VARCHAR(20)
);

-- Throughput Data for Charts
CREATE TABLE IF NOT EXISTS throughput_logs (
    id INT AUTO_INCREMENT PRIMARY KEY,
    log_time TIME NOT NULL,
    data_rate DECIMAL(10,2) NOT NULL
);

-- System Logs
CREATE TABLE IF NOT EXISTS system_logs (
    id INT AUTO_INCREMENT PRIMARY KEY,
    log_title VARCHAR(255) NOT NULL,
    log_desc VARCHAR(255),
    log_type VARCHAR(50), -- e.g., 'sync', 'report', 'backup'
    log_time TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Intelligence Batches (Reports)
CREATE TABLE IF NOT EXISTS intelligence_batches (
    id INT AUTO_INCREMENT PRIMARY KEY,
    batch_id VARCHAR(20) NOT NULL UNIQUE,
    source VARCHAR(100) NOT NULL,
    priority VARCHAR(20) NOT NULL, -- CRITICAL, HIGH, MEDIUM, LOW
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Seed Data
INSERT INTO system_stats (stat_name, stat_value, stat_trend, stat_icon, stat_color) VALUES 
('Data Packets', '4.2 PB', '+12%', 'database', 'text-cyan-400'),
('Active Links', '1,284', 'STABLE', 'users', 'text-purple-400'),
('Breach Intel', '002', 'CLEARED', 'shield-alert', 'text-amber-400');

INSERT INTO throughput_logs (log_time, data_rate) VALUES 
('00:00:00', 45.2), ('02:00:00', 67.8), ('04:00:00', 89.1), 
('06:00:00', 34.5), ('08:00:00', 56.7), ('10:00:00', 92.3), 
('12:00:00', 78.4), ('14:00:00', 45.9), ('16:00:00', 88.2), 
('18:00:00', 95.5), ('20:00:00', 63.1), ('22:00:00', 41.8);

INSERT INTO system_logs (log_title, log_desc, log_type) VALUES 
('Sync Complete', 'Node_72 Connected', 'sync'),
('Report Generated', 'Intelligence Batch #41', 'report'),
('Backup Sequence', 'Encryption Layer 3 Active', 'backup');

INSERT INTO intelligence_batches (batch_id, source, priority) VALUES 
('#B-001X', 'NEURAL_CORE', 'CRITICAL'),
('#B-002X', 'EXTERNAL_NODE', 'HIGH'),
('#B-003X', 'SYSTEM_WATCHER', 'MEDIUM'),
('#B-004X', 'DB_SYNC', 'LOW'),
('#B-005X', 'GATEWAY_A', 'CRITICAL'),
('#B-006X', 'NODE_DELTA', 'MEDIUM'),
('#B-007X', 'NEURAL_CORE', 'HIGH'),
('#B-008X', 'SYSTEM_WATCHER', 'MEDIUM');
