<?php
include 'includes/base.php';
render_header_centered("EDP - Secure Login");
?>

<div class="glass-card w-full max-w-md p-8 md:p-12 relative overflow-hidden group">
    <!-- Animated background element inside card -->
    <div class="absolute -top-24 -right-24 w-48 h-48 bg-cyan-500/20 rounded-full blur-3xl group-hover:bg-cyan-500/30 transition-all"></div>
    <div class="absolute -bottom-24 -left-24 w-48 h-48 bg-purple-500/20 rounded-full blur-3xl group-hover:bg-purple-500/30 transition-all"></div>

    <div class="text-center mb-8 relative">
        <h1 class="text-3xl font-bold tracking-tight text-white mb-2">ACCESS PORTAL</h1>
        <p class="text-slate-400 text-sm">BIO-DATA ENCRYPTION ACTIVE</p>
    </div>

    <form class="space-y-6 relative" action="dashboard.php" method="POST">
        <div>
            <label class="block text-xs font-bold uppercase tracking-widest text-slate-500 mb-2">USER IDENTIFIER</label>
            <div class="relative group">
                <i data-lucide="user" class="absolute left-3 top-3 w-5 h-5 text-slate-500 group-focus-within:text-cyan-400 transition-colors"></i>
                <input type="text" placeholder="ADMIN_USR_01" 
                    class="w-full bg-slate-900/50 border border-slate-700/50 rounded-xl py-3 pl-11 pr-4 focus:outline-none focus:border-cyan-500/50 focus:ring-1 focus:ring-cyan-500/20 transition-all text-slate-200">
            </div>
        </div>

        <div>
            <label class="block text-xs font-bold uppercase tracking-widest text-slate-500 mb-2">NEURAL KEY</label>
            <div class="relative group">
                <i data-lucide="lock" class="absolute left-3 top-3 w-5 h-5 text-slate-500 group-focus-within:text-cyan-400 transition-colors"></i>
                <input type="password" placeholder="••••••••" 
                    class="w-full bg-slate-900/50 border border-slate-700/50 rounded-xl py-3 pl-11 pr-4 focus:outline-none focus:border-cyan-500/50 focus:ring-1 focus:ring-cyan-500/20 transition-all text-slate-200">
            </div>
        </div>

        <div class="flex items-center justify-between text-xs">
            <label class="flex items-center space-x-2 cursor-pointer group">
                <input type="checkbox" class="w-4 h-4 rounded border-slate-700 bg-slate-900 text-cyan-500 focus:ring-cyan-500/20">
                <span class="text-slate-400 group-hover:text-slate-200 transition-colors">Remember Node</span>
            </label>
            <a href="recovery.php" class="text-cyan-400 hover:text-cyan-300 transition-colors underline-offset-4 hover:underline">Lost Neural Key?</a>
        </div>

        <button type="submit" 
            class="w-full py-4 px-6 bg-gradient-to-r from-cyan-600 to-purple-600 rounded-xl font-bold uppercase tracking-widest text-sm hover:from-cyan-500 hover:to-purple-500 shadow-lg shadow-cyan-900/20 hover:shadow-cyan-500/40 transition-all active:scale-[0.98]">
            INITIATE LINK
        </button>
    </form>

    <div class="mt-8 pt-8 border-t border-slate-800/50 flex justify-center space-x-4">
        <button class="p-2 rounded-lg bg-slate-900/50 border border-slate-700/50 hover:border-cyan-500/50 transition-all">
            <i data-lucide="fingerprint" class="w-6 h-6 text-slate-400 hover:text-cyan-400"></i>
        </button>
        <button class="p-2 rounded-lg bg-slate-900/50 border border-slate-700/50 hover:border-cyan-500/50 transition-all">
            <i data-lucide="scan-eye" class="w-6 h-6 text-slate-400 hover:text-cyan-400"></i>
        </button>
    </div>
</div>

<?php
render_footer_centered();
?>
