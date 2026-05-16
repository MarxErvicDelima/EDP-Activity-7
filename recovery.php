<?php
include 'includes/base.php';
render_header_centered("EDP - Neural Recovery");
?>

<div class="glass-card w-full max-w-md p-8 md:p-12 relative overflow-hidden group">
    <div class="absolute -top-24 -left-24 w-48 h-48 bg-purple-500/20 rounded-full blur-3xl group-hover:bg-purple-500/30 transition-all"></div>
    <div class="absolute -bottom-24 -right-24 w-48 h-48 bg-cyan-500/20 rounded-full blur-3xl group-hover:bg-cyan-500/30 transition-all"></div>

    <div class="text-center mb-8 relative">
        <div class="w-16 h-16 bg-gradient-to-tr from-cyan-500 to-purple-600 rounded-2xl flex items-center justify-center mx-auto mb-6 shadow-lg shadow-cyan-500/20">
            <i data-lucide="key-round" class="w-8 h-8 text-white"></i>
        </div>
        <h1 class="text-2xl font-bold tracking-tight text-white mb-2">NEURAL RECOVERY</h1>
        <p class="text-slate-400 text-sm">INITIATE RE-SYNC SEQUENCE</p>
    </div>

    <form class="space-y-6 relative" action="index.php" method="POST">
        <div>
            <label class="block text-xs font-bold uppercase tracking-widest text-slate-500 mb-2">RECOVERY IDENTIFIER</label>
            <div class="relative group">
                <i data-lucide="mail" class="absolute left-3 top-3 w-5 h-5 text-slate-500 group-focus-within:text-cyan-400 transition-colors"></i>
                <input type="email" placeholder="NEURAL_MAIL@NODE.NET" 
                    class="w-full bg-slate-900/50 border border-slate-700/50 rounded-xl py-3 pl-11 pr-4 focus:outline-none focus:border-cyan-500/50 focus:ring-1 focus:ring-cyan-500/20 transition-all text-slate-200">
            </div>
            <p class="text-[10px] text-slate-600 mt-2 uppercase tracking-tighter">Enter the mail associated with your neural node.</p>
        </div>

        <button type="submit" 
            class="w-full py-4 px-6 bg-slate-900 border border-slate-700/50 rounded-xl font-bold uppercase tracking-widest text-xs hover:bg-slate-800 hover:border-cyan-500/50 transition-all flex items-center justify-center space-x-2">
            <span>START SYNC</span>
            <i data-lucide="refresh-cw" class="w-4 h-4 text-cyan-400"></i>
        </button>

        <div class="text-center">
            <a href="index.php" class="text-slate-500 hover:text-white transition-colors text-xs uppercase tracking-widest flex items-center justify-center space-x-2">
                <i data-lucide="arrow-left" class="w-4 h-4"></i>
                <span>Back to Entry Node</span>
            </a>
        </div>
    </form>
</div>

<?php
render_footer_centered();
?>
