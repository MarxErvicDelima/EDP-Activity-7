<?php
// includes/db_edp.php
// Database connection for EDP System

$host = 'localhost';
$db   = 'edp_system';
$user = 'root';
$pass = ''; // Default XAMPP password is empty
$charset = 'utf8mb4';

$dsn = "mysql:host=$host;dbname=$db;charset=$charset";
$options = [
    PDO::ATTR_ERRMODE            => PDO::ERRMODE_EXCEPTION,
    PDO::ATTR_DEFAULT_FETCH_MODE => PDO::FETCH_ASSOC,
    PDO::ATTR_EMULATE_PREPARES   => false,
];

try {
     $pdo = new PDO($dsn, $user, $pass, $options);
} catch (\PDOException $e) {
     // Fallback to mock data if DB not present
     $pdo = null;
}

// Function to get system stats
function getSystemStats($pdo) {
    if ($pdo) {
        try {
            $data = $pdo->query("SELECT * FROM system_stats")->fetchAll();
            if (!empty($data)) return $data;
        } catch (Exception $e) {}
    }
    // Mock Fallback
    return [
        ['stat_name' => 'Data Packets', 'stat_value' => '4.2 PB', 'stat_trend' => '+12%', 'stat_icon' => 'database', 'stat_color' => 'text-cyan-400'],
        ['stat_name' => 'Active Links', 'stat_value' => '1,284', 'stat_trend' => 'STABLE', 'stat_icon' => 'users', 'stat_color' => 'text-purple-400'],
        ['stat_name' => 'Breach Intel', 'stat_value' => '002', 'stat_trend' => 'CLEARED', 'stat_icon' => 'shield-alert', 'stat_color' => 'text-amber-400']
    ];
}

// Function to get throughput data
function getThroughputLogs($pdo) {
    if ($pdo) {
        try {
            $data = $pdo->query("SELECT * FROM throughput_logs")->fetchAll();
            if (!empty($data)) return $data;
        } catch (Exception $e) {}
    }
    // Mock Fallback
    $logs = [];
    $times = ['00:00', '02:00', '04:00', '06:00', '08:00', '10:00', '12:00', '14:00', '16:00', '18:00', '20:00', '22:00'];
    foreach ($times as $time) {
        $logs[] = ['log_time' => $time, 'data_rate' => rand(30, 95) . '.' . rand(0,9)];
    }
    return $logs;
}

// Function to get system logs
function getSystemLogs($pdo) {
    if ($pdo) {
        try {
            $data = $pdo->query("SELECT * FROM system_logs ORDER BY log_time DESC LIMIT 5")->fetchAll();
            if (!empty($data)) return $data;
        } catch (Exception $e) {}
    }
    // Mock Fallback
    return [
        ['log_title' => 'Sync Complete', 'log_desc' => 'Node_72 Connected', 'log_type' => 'sync'],
        ['log_title' => 'Report Generated', 'log_desc' => 'Intelligence Batch #41', 'log_type' => 'report'],
        ['log_title' => 'Backup Sequence', 'log_desc' => 'Encryption Layer 3 Active', 'log_type' => 'backup']
    ];
}

// Function to get intelligence batches (reports)
function getIntelligenceBatches($pdo) {
    if ($pdo) {
        try {
            $data = $pdo->query("SELECT * FROM intelligence_batches")->fetchAll();
            if (!empty($data)) return $data;
        } catch (Exception $e) {}
    }
    // Mock Fallback
    $batches = [];
    $sources = ['NEURAL_CORE', 'EXTERNAL_NODE', 'SYSTEM_WATCHER', 'DB_SYNC', 'GATEWAY_A', 'NODE_DELTA'];
    $priorities = ['CRITICAL', 'HIGH', 'MEDIUM', 'LOW'];
    for ($i=1; $i<=8; $i++) {
        $batches[] = [
            'batch_id' => '#B-' . str_pad($i, 3, '0', STR_PAD_LEFT) . 'X',
            'source' => $sources[rand(0, 5)],
            'priority' => $priorities[rand(0, 3)],
            'created_at' => date('Y-m-d H:i:s')
        ];
    }
    return $batches;
}
?>
