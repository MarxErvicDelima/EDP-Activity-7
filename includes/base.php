<?php
// base.php - Common Header/Footer content with futuristic 3D styling
function render_header($title = "EDP Information System") {
?>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title><?php echo $title; ?></title>
    <!-- Tailwind CSS CDN -->
    <script src="https://cdn.tailwindcss.com"></script>
    <!-- Lucide Icons -->
    <script src="https://unpkg.com/lucide@latest"></script>
    <!-- GSAP for animations -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/gsap/3.12.2/gsap.min.js"></script>
    <!-- Three.js for 3D backgrounds -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/three.js/r128/three.min.js"></script>
    
    <style>
        @import url('https://fonts.googleapis.com/css2?family=Space+Grotesk:wght@300;400;500;600;700&family=Syncopate:wght@400;700&display=swap');
        
        :root {
            --neon-blue: #00f3ff;
            --neon-purple: #bc13fe;
            --neon-magenta: #ff00ff;
            --glass-bg: rgba(15, 23, 42, 0.7);
            --glass-border: rgba(255, 255, 255, 0.1);
        }

        body {
            font-family: 'Space Grotesk', sans-serif;
            background: #020617;
            color: #f8fafc;
            overflow-x: hidden;
            overflow-y: auto;
            margin: 0;
            padding: 0;
            min-height: 100vh;
        }

        .syncopate { font-family: 'Syncopate', sans-serif; }

        .glass-card {
            background: var(--glass-bg);
            backdrop-filter: blur(12px);
            border: 1px solid var(--glass-border);
            border-radius: 24px;
            box-shadow: 0 8px 32px 0 rgba(0, 0, 0, 0.37);
            transition: all 0.3s ease;
        }

        .glass-card:hover {
            border-color: var(--neon-blue);
            box-shadow: 0 0 20px rgba(0, 243, 255, 0.2);
            transform: translateY(-5px);
        }

        .neon-text {
            text-shadow: 0 0 10px var(--neon-blue), 0 0 20px var(--neon-blue);
        }

        .neon-border {
            border: 1px solid var(--neon-blue);
            box-shadow: 0 0 10px var(--neon-blue);
        }

        .btn-futuristic {
            position: relative;
            overflow: hidden;
            transition: all 0.3s ease;
            background: linear-gradient(45deg, #3b82f6, #8b5cf6);
            border-radius: 12px;
        }

        .btn-futuristic:hover {
            box-shadow: 0 0 15px var(--neon-blue);
            transform: scale(1.05);
        }

        #bg-canvas {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            z-index: 0;
            pointer-events: none;
        }

        /* 3D Animations */
        @keyframes float {
            0% { transform: translateY(0px) rotate(0deg); }
            50% { transform: translateY(-20px) rotate(2deg); }
            100% { transform: translateY(0px) rotate(0deg); }
        }

        .floating { animation: float 6s ease-in-out infinite; }
        
        /* Ensure content visibility */
        nav { position: relative; z-index: 50; }
        main { position: relative; z-index: 40; }
        .glass-card { position: relative; z-index: 60; }
    </style>
</head>
<body class="min-h-screen flex flex-col">
    <canvas id="bg-canvas"></canvas>
    <nav class="p-6 flex justify-between items-center z-10">
        <div class="flex items-center space-x-2">
            <div class="w-10 h-10 bg-gradient-to-tr from-cyan-500 to-purple-600 rounded-lg flex items-center justify-center neon-border">
                <i data-lucide="cpu" class="text-white"></i>
            </div>
            <span class="syncopate font-bold text-xl tracking-tighter">EDP<span class="text-cyan-400">GUI</span></span>
        </div>
        <div class="space-x-8 text-sm font-medium hidden md:flex">
            <a href="dashboard.php" class="hover:text-cyan-400 transition-colors">DASHBOARD</a>
            <a href="reports.php" class="hover:text-cyan-400 transition-colors">REPORTS</a>
            <a href="about.php" class="hover:text-cyan-400 transition-colors">ABOUT</a>
            <a href="index.php" class="px-4 py-2 border border-cyan-500/50 rounded-full hover:bg-cyan-500/10 transition-all">LOGIN</a>
        </div>
    </nav>
    <main class="flex-grow p-6 w-full flex flex-col items-center">
<?php
}

function render_header_centered($title = "EDP Information System") {
    render_header($title);
?>
    <div class="flex-grow flex items-center justify-center w-full">
<?php
}

function render_footer_centered() {
?>
    </div>
<?php
    render_footer();
}

function render_footer() {
?>
    </main>
    <footer class="p-8 text-center text-slate-500 text-xs tracking-widest uppercase">
        &copy; 2026 EDP GUI SYSTEM // NEURAL INTERFACE READY
    </footer>

    <script>
        // Initialize Lucide Icons
        lucide.createIcons();

        // 3D Background with Three.js
        const scene = new THREE.Scene();
        const camera = new THREE.PerspectiveCamera(75, window.innerWidth / window.innerHeight, 0.1, 1000);
        const renderer = new THREE.WebGLRenderer({ canvas: document.getElementById('bg-canvas'), alpha: true });
        renderer.setSize(window.innerWidth, window.innerHeight);

        const particlesGeometry = new THREE.BufferGeometry();
        const particlesCount = 5000;
        const posArray = new Float32Array(particlesCount * 3);

        for(let i = 0; i < particlesCount * 3; i++) {
            posArray[i] = (Math.random() - 0.5) * 5;
        }

        particlesGeometry.setAttribute('position', new THREE.BufferAttribute(posArray, 3));
        const material = new THREE.PointsMaterial({
            size: 0.005,
            color: 0x00f3ff,
            transparent: true,
            opacity: 0.5
        });

        const particlesMesh = new THREE.Points(particlesGeometry, material);
        scene.add(particlesMesh);

        camera.position.z = 2;

        // Mouse Interactivity Variables
        let mouseX = 0;
        let mouseY = 0;

        document.addEventListener('mousemove', (event) => {
            mouseX = (event.clientX / window.innerWidth - 0.5) * 0.5;
            mouseY = (event.clientY / window.innerHeight - 0.5) * 0.5;
        });

        function animate() {
            requestAnimationFrame(animate);
            
            // Auto-rotation - background always moves
            particlesMesh.rotation.y += 0.001;
            particlesMesh.rotation.x += 0.0005;

            // Follow Mouse Movement (Smoothing with lerp-like effect)
            // It will only affect rotation when mouse is moving, but the base rotation is always there
            particlesMesh.rotation.y += (mouseX - particlesMesh.rotation.y) * 0.02;
            particlesMesh.rotation.x += (mouseY - particlesMesh.rotation.x) * 0.02;

            renderer.render(scene, camera);
        }
        animate();

        window.addEventListener('resize', () => {
            camera.aspect = window.innerWidth / window.innerHeight;
            camera.updateProjectionMatrix();
            renderer.setSize(window.innerWidth, window.innerHeight);
        });

        // GSAP Global Animations - Fixed to ensure visibility
        gsap.from(".glass-card", {
            duration: 1.2,
            opacity: 0,
            y: 30,
            ease: "power3.out",
            stagger: 0.1,
            clearProps: "all" // Clears GSAP styles after animation to prevent visibility issues
        });
    </script>
</body>
</html>
<?php
}
?>
