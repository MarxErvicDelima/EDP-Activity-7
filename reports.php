<?php
include 'includes/base.php';
include 'includes/db_edp.php';
render_header("EDP - Intelligence Reports");

$batches = getIntelligenceBatches($pdo);
?>

<div class="w-full max-w-7xl mx-auto grid grid-cols-1 md:grid-cols-4 gap-8">
    <!-- Sidebar / Filters -->
    <aside class="md:col-span-1 space-y-6">
        <div class="glass-card p-6">
            <h3 class="text-white font-black uppercase tracking-widest text-sm mb-6 flex items-center space-x-2">
                <i data-lucide="filter" class="w-4 h-4 text-cyan-400"></i>
                <span>Report Filters</span>
            </h3>
            
            <form class="space-y-6">
                <div>
                    <label class="block text-[10px] font-bold uppercase tracking-widest text-slate-500 mb-3">Temporal Range</label>
                    <select class="w-full bg-slate-900/50 border border-slate-700/50 rounded-xl py-3 px-4 text-xs font-bold uppercase tracking-widest text-slate-300 focus:outline-none focus:border-cyan-500/50 transition-all">
                        <option>LAST_24_HOURS</option>
                        <option>LAST_7_DAYS</option>
                        <option>LAST_30_CYCLES</option>
                        <option>CUSTOM_OFFSET</option>
                    </select>
                </div>

                <div>
                    <label class="block text-[10px] font-bold uppercase tracking-widest text-slate-500 mb-3">Data Priority</label>
                    <div class="space-y-3">
                        <label class="flex items-center space-x-3 cursor-pointer group">
                            <input type="checkbox" checked class="w-4 h-4 rounded border-slate-700 bg-slate-900 text-cyan-500 focus:ring-cyan-500/20">
                            <span class="text-xs uppercase tracking-widest text-slate-400 group-hover:text-slate-200 transition-colors">CRITICAL_INTEL</span>
                        </label>
                        <label class="flex items-center space-x-3 cursor-pointer group">
                            <input type="checkbox" class="w-4 h-4 rounded border-slate-700 bg-slate-900 text-purple-500 focus:ring-purple-500/20">
                            <span class="text-xs uppercase tracking-widest text-slate-400 group-hover:text-slate-200 transition-colors">STANDARD_LOGS</span>
                        </label>
                        <label class="flex items-center space-x-3 cursor-pointer group">
                            <input type="checkbox" class="w-4 h-4 rounded border-slate-700 bg-slate-900 text-amber-500 focus:ring-amber-500/20">
                            <span class="text-xs uppercase tracking-widest text-slate-400 group-hover:text-slate-200 transition-colors">SYSTEM_ALERTS</span>
                        </label>
                    </div>
                </div>

                <button type="button" class="w-full py-4 bg-gradient-to-r from-cyan-600/20 to-purple-600/20 border border-cyan-500/30 rounded-xl font-black uppercase tracking-widest text-[10px] text-cyan-400 hover:from-cyan-600 hover:to-purple-600 hover:text-white transition-all">
                    RE-GENERATE_REPORT
                </button>
            </form>
        </div>

        <div class="glass-card p-6 border-slate-800/50">
            <h4 class="text-slate-500 text-[10px] font-bold uppercase tracking-widest mb-4">Export Node</h4>
            <div class="grid grid-cols-2 gap-3">
                <button class="p-3 bg-slate-900/50 border border-slate-700/50 rounded-xl hover:border-cyan-500/50 transition-all flex flex-col items-center justify-center space-y-2 group">
                    <i data-lucide="file-json" class="w-5 h-5 text-slate-400 group-hover:text-cyan-400"></i>
                    <span class="text-[8px] font-bold uppercase tracking-widest text-slate-500">JSON</span>
                </button>
                <button class="p-3 bg-slate-900/50 border border-slate-700/50 rounded-xl hover:border-purple-500/50 transition-all flex flex-col items-center justify-center space-y-2 group">
                    <i data-lucide="file-text" class="w-5 h-5 text-slate-400 group-hover:text-purple-400"></i>
                    <span class="text-[8px] font-bold uppercase tracking-widest text-slate-500">CSV</span>
                </button>
            </div>
        </div>
    </aside>

    <!-- Main Content Section -->
    <div class="md:col-span-3 space-y-8 min-h-[600px] border border-cyan-500/20 p-6 rounded-3xl">
        <h2 class="text-white text-xl font-bold uppercase tracking-widest mb-6">Reports Data Stream</h2>
        <div class="flex items-center justify-between">
            <h2 class="text-2xl font-black text-white uppercase tracking-widest">Active Intelligence Batches</h2>
            <div class="text-[10px] font-bold uppercase tracking-widest text-slate-500 flex items-center space-x-2">
                <span class="w-2 h-2 bg-green-500 rounded-full animate-ping"></span>
                <span>Real-time Stream Active</span>
            </div>
        </div>

        <!-- Report Table -->
        <div class="glass-card overflow-hidden">
            <table class="w-full text-left border-collapse">
                <thead>
                    <tr class="bg-slate-900/50 border-b border-slate-800/50">
                        <th class="p-6 text-[10px] font-black uppercase tracking-widest text-slate-400">BATCH_ID</th>
                        <th class="p-6 text-[10px] font-black uppercase tracking-widest text-slate-400">INTEL_SOURCE</th>
                        <th class="p-6 text-[10px] font-black uppercase tracking-widest text-slate-400">TIMESTAMP</th>
                        <th class="p-6 text-[10px] font-black uppercase tracking-widest text-slate-400">PRIORITY</th>
                        <th class="p-6 text-[10px] font-black uppercase tracking-widest text-slate-400">ACTION</th>
                    </tr>
                </thead>
                <tbody class="divide-y divide-slate-800/50">
                    <?php 
                    $priorities = ['CRITICAL', 'HIGH', 'MEDIUM', 'LOW'];
                    $colors = ['text-red-400', 'text-amber-400', 'text-cyan-400', 'text-slate-400'];
                    $bg_colors = ['bg-red-500/10 border-red-500/30', 'bg-amber-500/10 border-amber-500/30', 'bg-cyan-500/10 border-cyan-500/30', 'bg-slate-500/10 border-slate-500/30'];
                    
                    foreach($batches as $batch): 
                        $p_idx = array_search($batch['priority'], $priorities);
                        if ($p_idx === false) $p_idx = 3;
                    ?>
                    <tr class="hover:bg-cyan-500/5 transition-colors group">
                        <td class="p-6">
                            <span class="text-xs font-mono text-cyan-500 font-bold uppercase tracking-widest"><?php echo $batch['batch_id']; ?></span>
                        </td>
                        <td class="p-6">
                            <div class="flex items-center space-x-3">
                                <div class="w-8 h-8 rounded-lg bg-slate-800 flex items-center justify-center group-hover:bg-slate-700 transition-colors">
                                    <i data-lucide="cpu" class="w-4 h-4 text-slate-400 group-hover:text-cyan-400"></i>
                                </div>
                                <span class="text-xs font-bold uppercase tracking-widest text-white"><?php echo $batch['source']; ?></span>
                            </div>
                        </td>
                        <td class="p-6">
                            <div class="text-[10px] font-bold uppercase tracking-widest text-slate-200"><?php echo date('Y-m-d', strtotime($batch['created_at'])); ?></div>
                            <div class="text-[10px] font-bold uppercase tracking-widest text-slate-500 mt-1"><?php echo date('H:i:s', strtotime($batch['created_at'])); ?> UTC</div>
                        </td>
                        <td class="p-6">
                            <span class="px-4 py-1.5 border rounded-full text-[8px] font-black uppercase tracking-widest <?php echo $bg_colors[$p_idx]; ?> <?php echo $colors[$p_idx]; ?>">
                                <?php echo $batch['priority']; ?>
                            </span>
                        </td>
                        <td class="p-6">
                            <div class="flex space-x-2">
                                <button class="p-2.5 bg-slate-800/50 hover:bg-cyan-500/20 rounded-xl text-slate-400 hover:text-cyan-400 transition-all border border-slate-700/50 hover:border-cyan-500/50">
                                    <i data-lucide="eye" class="w-4 h-4"></i>
                                </button>
                                <button class="p-2.5 bg-slate-800/50 hover:bg-purple-500/20 rounded-xl text-slate-400 hover:text-purple-400 transition-all border border-slate-700/50 hover:border-purple-500/50">
                                    <i data-lucide="download" class="w-4 h-4"></i>
                                </button>
                            </div>
                        </td>
                    </tr>
                    <?php endforeach; ?>
                </tbody>
            </table>
        </div>

        <!-- Pagination Mockup -->
        <div class="flex justify-center space-x-2">
            <button class="w-10 h-10 glass-card flex items-center justify-center text-cyan-400 border-cyan-500/30">1</button>
            <button class="w-10 h-10 glass-card flex items-center justify-center text-slate-500 hover:text-white transition-all">2</button>
            <button class="w-10 h-10 glass-card flex items-center justify-center text-slate-500 hover:text-white transition-all">3</button>
            <button class="w-10 h-10 glass-card flex items-center justify-center text-slate-500 hover:text-white transition-all">
                <i data-lucide="chevron-right" class="w-4 h-4"></i>
            </button>
        </div>
    </div>
</div>

<?php
render_footer();
?>
