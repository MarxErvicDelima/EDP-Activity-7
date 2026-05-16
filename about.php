<?php
include 'includes/base.php';
render_header_centered("EDP - System Intel");
?>

<div class="max-w-6xl w-full grid grid-cols-1 lg:grid-cols-2 gap-12 items-center">
    <!-- Left Column: 3D Visualization Placeholder -->
    <div class="relative group hidden lg:block">
        <div class="absolute -top-32 -left-32 w-64 h-64 bg-cyan-500/20 rounded-full blur-3xl group-hover:bg-cyan-500/30 transition-all"></div>
        <div class="absolute -bottom-32 -right-32 w-64 h-64 bg-purple-500/20 rounded-full blur-3xl group-hover:bg-purple-500/30 transition-all"></div>
        
        <div class="glass-card p-12 aspect-square flex items-center justify-center relative overflow-hidden">
            <div id="intel-cube" class="w-48 h-48 bg-gradient-to-tr from-cyan-500 to-purple-600 rounded-3xl floating shadow-2xl shadow-cyan-500/40 relative z-10 flex items-center justify-center">
                <i data-lucide="cpu" class="w-24 h-24 text-white"></i>
                <div class="absolute -inset-4 border border-cyan-500/50 rounded-[40px] animate-ping opacity-20"></div>
                <div class="absolute -inset-8 border border-purple-500/50 rounded-[50px] animate-pulse opacity-10"></div>
            </div>
            
            <!-- Floating Data Tags -->
            <div class="absolute top-10 right-10 glass-card p-3 text-[10px] tracking-widest uppercase text-cyan-400 border-cyan-500/30 animate-bounce">SYSTEM_V2.6.1</div>
            <div class="absolute bottom-20 left-10 glass-card p-3 text-[10px] tracking-widest uppercase text-purple-400 border-purple-500/30 animate-pulse">NEURAL_READY</div>
        </div>
    </div>

    <!-- Right Column: Content -->
    <div class="space-y-8 relative">
        <div class="inline-block px-4 py-1 rounded-full bg-cyan-500/10 border border-cyan-500/30 text-cyan-400 text-xs font-bold tracking-widest uppercase mb-4">
            System Intelligence
        </div>
        <h1 class="text-5xl font-black tracking-tighter text-white leading-tight">EDP INFORMATION SYSTEM</h1>
        <p class="text-slate-400 text-lg leading-relaxed max-w-xl">
            A next-generation neural interface designed for high-throughput data processing and real-time intelligence gathering. Engineered for the future of decentralized information management.
        </p>

        <div class="grid grid-cols-2 gap-6">
            <div class="glass-card p-6 border-slate-700/50 hover:border-cyan-500/50">
                <i data-lucide="shield-check" class="w-8 h-8 text-cyan-400 mb-4"></i>
                <h3 class="text-white font-bold mb-2 uppercase tracking-wide">SECURE_CORE</h3>
                <p class="text-slate-500 text-sm">Advanced encryption layers protecting every data node.</p>
            </div>
            <div class="glass-card p-6 border-slate-700/50 hover:border-purple-500/50">
                <i data-lucide="zap" class="w-8 h-8 text-purple-400 mb-4"></i>
                <h3 class="text-white font-bold mb-2 uppercase tracking-wide">ULTRA_SYNC</h3>
                <p class="text-slate-500 text-sm">Sub-millisecond synchronization across all active portals.</p>
            </div>
        </div>

        <div class="flex space-x-4">
            <a href="index.php" class="px-8 py-4 bg-gradient-to-r from-cyan-600 to-purple-600 rounded-xl font-bold uppercase tracking-widest text-sm hover:from-cyan-500 hover:to-purple-500 transition-all flex items-center space-x-3 group">
                <span>Access Node</span>
                <i data-lucide="arrow-right" class="w-4 h-4 group-hover:translate-x-1 transition-transform"></i>
            </a>
            <button class="px-8 py-4 glass-card border-slate-700/50 hover:border-slate-500 font-bold uppercase tracking-widest text-sm transition-all">
                Intel Specs
            </button>
        </div>
    </div>
</div>

<script>
    // Specific animation for the about page cube
    gsap.to("#intel-cube", {
        rotationY: 360,
        duration: 20,
        repeat: -1,
        ease: "none"
    });
</script>

<?php
render_footer_centered();
?>
