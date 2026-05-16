<?php
include 'includes/base.php';
include 'includes/db_edp.php';
render_header("EDP - Intelligence Dashboard");

// Fetch data from DB
$stats = getSystemStats($pdo);
$throughputLogs = getThroughputLogs($pdo);
$systemLogs = getSystemLogs($pdo);
?>

<div class="w-full max-w-7xl mx-auto grid grid-cols-1 md:grid-cols-4 gap-8">
    <!-- Sidebar -->
    <aside class="md:col-span-1 space-y-6">
        <div class="glass-card p-6 flex items-center space-x-4">
            <div class="w-12 h-12 bg-gradient-to-tr from-cyan-500 to-purple-600 rounded-xl flex items-center justify-center">
                <i data-lucide="user" class="text-white"></i>
            </div>
            <div>
                <p class="text-xs text-slate-500 uppercase font-bold tracking-widest">Operator</p>
                <h3 class="text-white font-black uppercase tracking-tight">USR_ADMIN_01</h3>
            </div>
        </div>

        <nav class="glass-card p-4 space-y-2">
            <a href="dashboard.php" class="flex items-center space-x-3 p-3 rounded-xl bg-cyan-500/10 text-cyan-400 font-bold border border-cyan-500/30">
                <i data-lucide="layout-dashboard" class="w-5 h-5"></i>
                <span class="text-xs uppercase tracking-widest">Dashboard</span>
            </a>
            <a href="reports.php" class="flex items-center space-x-3 p-3 rounded-xl hover:bg-slate-800/50 text-slate-400 hover:text-white transition-all">
                <i data-lucide="bar-chart-3" class="w-5 h-5"></i>
                <span class="text-xs uppercase tracking-widest">Reports</span>
            </a>
            <a href="about.php" class="flex items-center space-x-3 p-3 rounded-xl hover:bg-slate-800/50 text-slate-400 hover:text-white transition-all">
                <i data-lucide="info" class="w-5 h-5"></i>
                <span class="text-xs uppercase tracking-widest">System Info</span>
            </a>
            <div class="pt-4 border-t border-slate-800/50 mt-4">
                <a href="index.php" class="flex items-center space-x-3 p-3 rounded-xl hover:bg-red-500/10 text-red-500 transition-all">
                    <i data-lucide="log-out" class="w-5 h-5"></i>
                    <span class="text-xs uppercase tracking-widest">Terminate Link</span>
                </a>
            </div>
        </nav>

        <div class="glass-card p-6 border-cyan-500/20">
            <div class="flex items-center justify-between mb-4">
                <span class="text-xs uppercase tracking-widest text-slate-500 font-bold">Node Health</span>
                <span class="text-cyan-400 text-xs font-bold">98%</span>
            </div>
            <div class="w-full h-2 bg-slate-900 rounded-full overflow-hidden">
                <div class="w-[98%] h-full bg-gradient-to-r from-cyan-500 to-blue-500 animate-pulse"></div>
            </div>
        </div>
    </aside>

    <!-- Main Content Section -->
    <div class="md:col-span-3 space-y-8 min-h-[600px] border border-cyan-500/20 p-6 rounded-3xl">
        <h2 class="text-white text-xl font-bold">Main Dashboard Node</h2>
        <!-- Stats Grid -->
        <div class="grid grid-cols-1 sm:grid-cols-3 gap-6">
            <?php foreach($stats as $stat): ?>
            <div class="glass-card p-6 relative overflow-hidden group">
                <div class="absolute -right-4 -bottom-4 w-24 h-24 bg-cyan-500/10 rounded-full blur-2xl group-hover:bg-cyan-500/20 transition-all"></div>
                <div class="flex items-center justify-between mb-4">
                    <i data-lucide="<?php echo $stat['stat_icon']; ?>" class="w-8 h-8 <?php echo $stat['stat_color']; ?>"></i>
                    <span class="text-[10px] <?php echo $stat['stat_color']; ?> font-black tracking-widest uppercase">Live</span>
                </div>
                <h4 class="text-slate-500 text-xs font-bold uppercase tracking-widest mb-1"><?php echo $stat['stat_name']; ?></h4>
                <p class="text-3xl font-black text-white"><?php echo $stat['stat_value']; ?></p>
                <p class="text-[10px] <?php echo $stat['stat_color']; ?> mt-2 flex items-center">
                    <i data-lucide="<?php echo $stat['stat_trend'] == 'CLEARED' || $stat['stat_trend'] == 'STABLE' ? 'check-circle' : 'trending-up'; ?>" class="w-3 h-3 mr-1"></i> 
                    <?php echo $stat['stat_trend']; ?>
                </p>
            </div>
            <?php endforeach; ?>
        </div>

        <!-- Activity Feed / Chart Placeholder -->
        <div class="grid grid-cols-1 lg:grid-cols-3 gap-8">
            <div class="lg:col-span-2 glass-card p-8">
                <div class="flex items-center justify-between mb-8">
                    <h3 class="text-white font-black uppercase tracking-widest text-sm flex items-center space-x-2">
                        <i data-lucide="bar-chart-3" class="w-4 h-4 text-cyan-400"></i>
                        <span>Neural Throughput</span>
                    </h3>
                    <div class="flex space-x-2">
                        <button class="px-3 py-1 bg-cyan-500/10 text-cyan-400 text-[10px] font-bold rounded-lg border border-cyan-500/30 uppercase tracking-widest">24H</button>
                        <button class="px-3 py-1 hover:bg-slate-800 text-slate-500 text-[10px] font-bold rounded-lg transition-all uppercase tracking-widest">7D</button>
                    </div>
                </div>
                <div class="h-64 flex items-end space-x-4">
                    <!-- Animated Bar Chart Mockup -->
                    <?php 
                    foreach($throughputLogs as $index => $log): 
                        $height = (float)$log['data_rate'];
                        $delay = $index * 0.05;
                    ?>
                    <div class="flex-grow group relative h-full flex items-end">
                        <div class="bg-gradient-to-t from-cyan-600/40 to-cyan-400 rounded-t-lg w-full transition-all hover:to-cyan-200 hover:shadow-[0_0_15px_rgba(0,243,255,0.5)] cursor-pointer" 
                             style="height: <?php echo $height; ?>%; animation: grow-up 0.8s cubic-bezier(0.17, 0.67, 0.83, 0.67) <?php echo $delay; ?>s backwards;">
                            <div class="absolute -top-12 left-1/2 -translate-x-1/2 glass-card px-3 py-1.5 text-[10px] font-bold text-cyan-400 opacity-0 group-hover:opacity-100 transition-all pointer-events-none z-30 whitespace-nowrap border-cyan-500/50">
                                <?php echo $log['data_rate']; ?> GB/s
                                <div class="text-[8px] text-slate-500 text-center"><?php echo $log['log_time']; ?></div>
                            </div>
                        </div>
                    </div>
                    <?php endforeach; ?>
                </div>
                <div class="flex justify-between mt-6 text-[10px] text-slate-500 font-bold uppercase tracking-widest">
                    <span>00:00</span>
                    <span>06:00</span>
                    <span>12:00</span>
                    <span>18:00</span>
                    <span>23:59</span>
                </div>
            </div>

            <div class="glass-card p-8">
                <h3 class="text-white font-black uppercase tracking-widest text-sm mb-6 flex items-center space-x-2">
                    <i data-lucide="history" class="w-4 h-4 text-purple-400"></i>
                    <span>System Log</span>
                </h3>
                <div class="space-y-6">
                    <?php foreach($systemLogs as $log): 
                        $dotColor = 'bg-cyan-500';
                        $shadowColor = 'shadow-cyan-500/50';
                        if($log['log_type'] == 'report') { $dotColor = 'bg-purple-500'; $shadowColor = 'shadow-purple-500/50'; }
                        if($log['log_type'] == 'backup') { $dotColor = 'bg-amber-500'; $shadowColor = 'shadow-amber-500/50'; }
                    ?>
                    <div class="flex space-x-4 group">
                        <div class="relative">
                            <div class="w-2 h-2 <?php echo $dotColor; ?> rounded-full mt-2 group-hover:scale-150 transition-transform shadow-lg <?php echo $shadowColor; ?>"></div>
                            <div class="absolute top-4 left-1 w-px h-12 bg-slate-800"></div>
                        </div>
                        <div>
                            <p class="text-white text-xs font-bold uppercase tracking-wide"><?php echo $log['log_title']; ?></p>
                            <p class="text-slate-500 text-[10px] uppercase tracking-tighter mt-1"><?php echo $log['log_desc']; ?></p>
                        </div>
                    </div>
                    <?php endforeach; ?>
                </div>
                <button class="w-full mt-8 py-3 border border-slate-800 hover:border-cyan-500/50 rounded-xl text-[10px] font-bold uppercase tracking-widest text-slate-500 hover:text-cyan-400 transition-all">
                    View All Logs
                </button>
            </div>
        </div>
    </div>
</div>

<style>
    @keyframes grow-up {
        from { height: 0; opacity: 0; }
        to { opacity: 1; }
    }
</style>

<?php
render_footer();
?>
