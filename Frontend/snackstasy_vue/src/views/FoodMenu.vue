<template>
  <div class="landing" ref="landingRef">
    <!-- Animated background -->
    <canvas class="bg-canvas" ref="canvasRef"></canvas>
    <div class="noise"></div>

    <!-- NAV -->
    <nav class="nav">
      <div class="nav__logo">
        <span class="nav__logo-icon">⬡</span>
        <span class="nav__logo-text">TICKETSYS</span>
      </div>
      <div class="nav__links">
        <a href="#features" class="nav__link">Features</a>
        <a href="#stats" class="nav__link">Stats</a>
        <a href="#contact" class="nav__link">Kontakt</a>
      </div>
      <a href="/login" class="nav__cta">Login →</a>
    </nav>

    <!-- HERO -->
    <section class="hero">
      <div class="hero__eyebrow">
        <span class="pulse-dot"></span>
        System aktiv · Version 2.0
      </div>

      <h1 class="hero__title">
        <span class="hero__title-line" style="animation-delay: 0.1s">Tickets.</span>
        <span class="hero__title-line hero__title-line--accent" style="animation-delay: 0.25s">Blitzschnell.</span>
        <span class="hero__title-line" style="animation-delay: 0.4s">Sicher.</span>
      </h1>

      <p class="hero__sub">
        Das moderne Ticket-Management-System für Teams, die keine Zeit verschwenden wollen.
        QR-Scan, Echtzeit-Status, maximale Kontrolle.
      </p>

      <div class="hero__actions">
        <a href="/login" class="btn-primary">
          <span>Jetzt starten</span>
          <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M5 12h14M12 5l7 7-7 7"/></svg>
        </a>
        <a href="#features" class="btn-ghost">Features entdecken</a>
      </div>

      <div class="hero__badge-row">
        <div class="badge">⚡ Echtzeit</div>
        <div class="badge">🔒 Verschlüsselt</div>
        <div class="badge">📱 Mobile-First</div>
        <div class="badge">⬡ QR-Scan</div>
      </div>
    </section>

    <!-- MARQUEE -->
    <div class="marquee-wrap">
      <div class="marquee">
        <span v-for="i in 12" :key="i">TICKET SYSTEM &nbsp;·&nbsp; QR SCAN &nbsp;·&nbsp; SECURE ACCESS &nbsp;·&nbsp; REAL-TIME &nbsp;·&nbsp; </span>
      </div>
    </div>

    <!-- STATS -->
    <section class="stats" id="stats">
      <div class="stats__grid">
        <div class="stat-card" v-for="(s, i) in stats" :key="i" :style="`animation-delay: ${i * 0.1}s`">
          <div class="stat-card__num">{{ s.num }}</div>
          <div class="stat-card__label">{{ s.label }}</div>
          <div class="stat-card__bar"><div class="stat-card__fill" :style="`width: ${s.pct}%; animation-delay: ${i * 0.15 + 0.5}s`"></div></div>
        </div>
      </div>
    </section>

    <!-- FEATURES -->
    <section class="features" id="features">
      <div class="features__header">
        <p class="section-eyebrow">Was wir bieten</p>
        <h2 class="section-title">Gebaut für <em>Geschwindigkeit</em></h2>
      </div>

      <div class="features__grid">
        <div class="feat-card" v-for="(f, i) in features" :key="i" :style="`animation-delay: ${i * 0.08}s`">
          <div class="feat-card__icon">{{ f.icon }}</div>
          <h3 class="feat-card__title">{{ f.title }}</h3>
          <p class="feat-card__desc">{{ f.desc }}</p>
          <div class="feat-card__line"></div>
        </div>
      </div>
    </section>

    <!-- CTA BAND -->
    <section class="cta-band" id="contact">
      <div class="cta-band__inner">
        <h2 class="cta-band__title">Bereit zum Einloggen?</h2>
        <p class="cta-band__sub">Scanne deinen QR-Code oder gib deine Ticket-ID ein.</p>
        <a href="/login" class="btn-primary btn-primary--large">
          <span>Zum Login</span>
          <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M5 12h14M12 5l7 7-7 7"/></svg>
        </a>
      </div>
      <div class="cta-band__glow"></div>
    </section>

    <!-- FOOTER -->
    <footer class="footer">
      <span class="nav__logo-icon">⬡</span>
      <span>TICKETSYS &nbsp;·&nbsp; 2025 &nbsp;·&nbsp; Alle Rechte vorbehalten</span>
    </footer>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onBeforeUnmount } from 'vue'

const canvasRef = ref<HTMLCanvasElement | null>(null)
let animId = 0

const stats = [
  { num: '99.9%', label: 'Uptime', pct: 99 },
  { num: '<50ms', label: 'QR Scan Speed', pct: 95 },
  { num: '10k+', label: 'Tickets verwaltet', pct: 80 },
  { num: '256bit', label: 'Verschlüsselung', pct: 100 },
]

const features = [
  { icon: '⬡', title: 'QR-Code Scan', desc: 'Sekundenschnelles Einloggen per Smartphone-Kamera. Kein Tippen, kein Warten.' },
  { icon: '⚡', title: 'Echtzeit-Status', desc: 'Ticket-Status sofort sichtbar. Keine veralteten Daten, immer live.' },
  { icon: '🔒', title: 'Sichere Sessions', desc: 'Session-basierte Authentifizierung ohne dauerhafte Datenspeicherung.' },
  { icon: '📱', title: 'Mobile First', desc: 'Optimiert für jede Bildschirmgröße — vom Handy bis zum Desktop.' },
  { icon: '🎯', title: 'Einfache UX', desc: 'Zwei Felder, ein Klick. Maximale Effizienz durch minimales Interface.' },
  { icon: '⬡', title: 'Erweiterbar', desc: 'Modulare Architektur — jederzeit um neue Features erweiterbar.' },
]

onMounted(() => {
  const canvas = canvasRef.value
  if (!canvas) return
  const ctx = canvas.getContext('2d')!

  const resize = () => {
    canvas.width = window.innerWidth
    canvas.height = window.innerHeight
  }
  resize()
  window.addEventListener('resize', resize)

  // Particle grid
  const COLS = 24
  const ROWS = 14
  const pts: { x: number; y: number; ox: number; oy: number; vx: number; vy: number; a: number }[] = []

  for (let r = 0; r < ROWS; r++) {
    for (let c = 0; c < COLS; c++) {
      const x = (c / (COLS - 1)) * window.innerWidth
      const y = (r / (ROWS - 1)) * window.innerHeight
      pts.push({ x, y, ox: x, oy: y, vx: 0, vy: 0, a: Math.random() * Math.PI * 2 })
    }
  }

  let t = 0
  const draw = () => {
    t += 0.008
    ctx.clearRect(0, 0, canvas.width, canvas.height)

    pts.forEach((p) => {
      p.x = p.ox + Math.sin(p.a + t) * 18
      p.y = p.oy + Math.cos(p.a * 0.7 + t * 0.8) * 12

      ctx.beginPath()
      ctx.arc(p.x, p.y, 1.2, 0, Math.PI * 2)
      ctx.fillStyle = `rgba(0, 255, 180, ${0.12 + Math.sin(p.a + t) * 0.06})`
      ctx.fill()
    })

    // Draw connections
    for (let i = 0; i < pts.length; i++) {
      for (let j = i + 1; j < pts.length; j++) {
        const pi = pts[i]
        const pj = pts[j]
        if (!pi || !pj) continue
        const dx = pi.x - pj.x
        const dy = pi.y - pj.y
        const d = Math.sqrt(dx * dx + dy * dy)
        if (d < 90) {
          ctx.beginPath()
          ctx.moveTo(pi.x, pi.y)
          ctx.lineTo(pj.x, pj.y)
          ctx.strokeStyle = `rgba(0, 255, 180, ${(1 - d / 90) * 0.06})`
          ctx.lineWidth = 0.5
          ctx.stroke()
        }
      }
    }

    animId = requestAnimationFrame(draw)
  }
  draw()

  return () => window.removeEventListener('resize', resize)
})

onBeforeUnmount(() => cancelAnimationFrame(animId))
</script>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Bebas+Neue&family=Outfit:wght@300;400;500;600&display=swap');

/* ─── RESET & BASE ─── */
*, *::before, *::after { box-sizing: border-box; margin: 0; padding: 0; }

.landing {
  --c-bg: #040810;
  --c-surface: #080f1a;
  --c-border: rgba(0, 255, 180, 0.12);
  --c-accent: #00ffb4;
  --c-accent-dim: rgba(0, 255, 180, 0.15);
  --c-text: #e2eaf4;
  --c-muted: #4a6278;
  --font-display: 'Bebas Neue', sans-serif;
  --font-body: 'Outfit', sans-serif;

  background: var(--c-bg);
  color: var(--c-text);
  font-family: var(--font-body);
  min-height: 100dvh;
  overflow-x: hidden;
  position: relative;
}

/* ─── BACKGROUND ─── */
.bg-canvas {
  position: fixed;
  inset: 0;
  z-index: 0;
  opacity: 0.6;
  pointer-events: none;
}

.noise {
  position: fixed;
  inset: 0;
  z-index: 1;
  pointer-events: none;
  opacity: 0.025;
  background-image: url("data:image/svg+xml,%3Csvg viewBox='0 0 256 256' xmlns='http://www.w3.org/2000/svg'%3E%3Cfilter id='noise'%3E%3CfeTurbulence type='fractalNoise' baseFrequency='0.9' numOctaves='4' stitchTiles='stitch'/%3E%3C/filter%3E%3Crect width='100%25' height='100%25' filter='url(%23noise)'/%3E%3C/svg%3E");
  background-size: 200px 200px;
}

/* ─── NAV ─── */
.nav {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  z-index: 100;
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 1.25rem 3rem;
  background: rgba(4, 8, 16, 0.7);
  backdrop-filter: blur(16px);
  border-bottom: 1px solid var(--c-border);
}

.nav__logo {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  font-family: var(--font-display);
  font-size: 1.3rem;
  letter-spacing: 0.15em;
  color: var(--c-accent);
}

.nav__logo-icon {
  font-size: 1.4rem;
  animation: spin-slow 12s linear infinite;
  display: inline-block;
}

@keyframes spin-slow { to { transform: rotate(360deg); } }

.nav__links {
  display: flex;
  gap: 2.5rem;
}

.nav__link {
  font-size: 0.85rem;
  color: var(--c-muted);
  text-decoration: none;
  letter-spacing: 0.08em;
  text-transform: uppercase;
  transition: color 0.2s;
}
.nav__link:hover { color: var(--c-accent); }

.nav__cta {
  font-size: 0.82rem;
  font-weight: 600;
  letter-spacing: 0.1em;
  text-transform: uppercase;
  color: var(--c-bg);
  background: var(--c-accent);
  border-radius: 6px;
  padding: 0.5rem 1.2rem;
  text-decoration: none;
  transition: opacity 0.2s, transform 0.2s;
}
.nav__cta:hover { opacity: 0.85; transform: translateY(-1px); }

/* ─── HERO ─── */
.hero {
  position: relative;
  z-index: 10;
  min-height: 100dvh;
  display: flex;
  flex-direction: column;
  align-items: flex-start;
  justify-content: center;
  padding: 8rem 3rem 4rem;
  max-width: 900px;
}

.hero__eyebrow {
  display: flex;
  align-items: center;
  gap: 0.6rem;
  font-size: 0.78rem;
  letter-spacing: 0.2em;
  text-transform: uppercase;
  color: var(--c-accent);
  margin-bottom: 2rem;
  animation: fadeUp 0.6s ease both;
}

.pulse-dot {
  width: 8px;
  height: 8px;
  border-radius: 50%;
  background: var(--c-accent);
  animation: pulse 2s ease-in-out infinite;
}

@keyframes pulse {
  0%, 100% { box-shadow: 0 0 0 0 rgba(0, 255, 180, 0.4); }
  50%       { box-shadow: 0 0 0 8px rgba(0, 255, 180, 0); }
}

.hero__title {
  display: flex;
  flex-direction: column;
  font-family: var(--font-display);
  font-size: clamp(5rem, 13vw, 11rem);
  line-height: 0.9;
  letter-spacing: 0.02em;
  margin-bottom: 2rem;
}

.hero__title-line {
  display: block;
  opacity: 0;
  transform: translateY(40px);
  animation: fadeUp 0.7s cubic-bezier(0.16, 1, 0.3, 1) forwards;
}

.hero__title-line--accent {
  color: transparent;
  -webkit-text-stroke: 2px var(--c-accent);
  text-shadow: 0 0 60px rgba(0, 255, 180, 0.3);
}

@keyframes fadeUp {
  to { opacity: 1; transform: translateY(0); }
}

.hero__sub {
  font-size: 1.05rem;
  font-weight: 300;
  color: var(--c-muted);
  max-width: 520px;
  line-height: 1.7;
  margin-bottom: 2.5rem;
  opacity: 0;
  animation: fadeUp 0.7s 0.5s ease both;
}

.hero__actions {
  display: flex;
  align-items: center;
  gap: 1.25rem;
  margin-bottom: 3rem;
  opacity: 0;
  animation: fadeUp 0.7s 0.65s ease both;
}

/* ─── BUTTONS ─── */
.btn-primary {
  display: inline-flex;
  align-items: center;
  gap: 0.6rem;
  background: var(--c-accent);
  color: #030810;
  font-family: var(--font-body);
  font-weight: 600;
  font-size: 0.9rem;
  letter-spacing: 0.06em;
  padding: 0.85rem 1.8rem;
  border-radius: 8px;
  text-decoration: none;
  transition: all 0.25s ease;
  box-shadow: 0 0 30px rgba(0, 255, 180, 0.25);
}
.btn-primary svg { width: 16px; height: 16px; transition: transform 0.2s; }
.btn-primary:hover { box-shadow: 0 0 50px rgba(0, 255, 180, 0.45); transform: translateY(-2px); }
.btn-primary:hover svg { transform: translateX(4px); }
.btn-primary--large { font-size: 1rem; padding: 1rem 2.2rem; }

.btn-ghost {
  color: var(--c-muted);
  font-size: 0.88rem;
  letter-spacing: 0.06em;
  text-decoration: none;
  border-bottom: 1px solid var(--c-border);
  padding-bottom: 2px;
  transition: color 0.2s, border-color 0.2s;
}
.btn-ghost:hover { color: var(--c-text); border-color: var(--c-text); }

/* ─── BADGES ─── */
.hero__badge-row {
  display: flex;
  gap: 0.75rem;
  flex-wrap: wrap;
  opacity: 0;
  animation: fadeUp 0.7s 0.8s ease both;
}

.badge {
  font-size: 0.75rem;
  letter-spacing: 0.08em;
  padding: 0.4rem 0.9rem;
  border: 1px solid var(--c-border);
  border-radius: 100px;
  color: var(--c-muted);
  background: rgba(0, 255, 180, 0.04);
  backdrop-filter: blur(8px);
  transition: all 0.2s;
}
.badge:hover { border-color: var(--c-accent); color: var(--c-accent); }

/* ─── MARQUEE ─── */
.marquee-wrap {
  position: relative;
  z-index: 10;
  overflow: hidden;
  border-top: 1px solid var(--c-border);
  border-bottom: 1px solid var(--c-border);
  padding: 0.9rem 0;
  background: rgba(0, 255, 180, 0.025);
}

.marquee {
  display: flex;
  white-space: nowrap;
  animation: marquee 30s linear infinite;
  font-family: var(--font-display);
  font-size: 0.9rem;
  letter-spacing: 0.15em;
  color: var(--c-muted);
}

@keyframes marquee { from { transform: translateX(0); } to { transform: translateX(-50%); } }

/* ─── STATS ─── */
.stats {
  position: relative;
  z-index: 10;
  padding: 6rem 3rem;
}

.stats__grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 1.5rem;
  max-width: 1000px;
  margin: 0 auto;
}

.stat-card {
  background: var(--c-surface);
  border: 1px solid var(--c-border);
  border-radius: 16px;
  padding: 1.75rem;
  opacity: 0;
  animation: fadeUp 0.6s ease both;
  transition: border-color 0.3s, transform 0.3s;
}
.stat-card:hover { border-color: rgba(0, 255, 180, 0.3); transform: translateY(-4px); }

.stat-card__num {
  font-family: var(--font-display);
  font-size: 2.8rem;
  color: var(--c-accent);
  line-height: 1;
  margin-bottom: 0.4rem;
}

.stat-card__label {
  font-size: 0.78rem;
  letter-spacing: 0.12em;
  text-transform: uppercase;
  color: var(--c-muted);
  margin-bottom: 1rem;
}

.stat-card__bar {
  height: 2px;
  background: rgba(0, 255, 180, 0.1);
  border-radius: 2px;
  overflow: hidden;
}

.stat-card__fill {
  height: 100%;
  background: var(--c-accent);
  border-radius: 2px;
  width: 0;
  animation: barFill 1.2s cubic-bezier(0.16, 1, 0.3, 1) forwards;
  box-shadow: 0 0 10px rgba(0, 255, 180, 0.5);
}

@keyframes barFill { to { width: var(--target, 100%); } }

/* ─── FEATURES ─── */
.features {
  position: relative;
  z-index: 10;
  padding: 4rem 3rem 6rem;
}

.features__header {
  text-align: center;
  margin-bottom: 4rem;
}

.section-eyebrow {
  font-size: 0.75rem;
  letter-spacing: 0.25em;
  text-transform: uppercase;
  color: var(--c-accent);
  margin-bottom: 0.75rem;
}

.section-title {
  font-family: var(--font-display);
  font-size: clamp(2.5rem, 5vw, 4.5rem);
  line-height: 1;
  letter-spacing: 0.03em;
}

.section-title em {
  font-style: normal;
  color: transparent;
  -webkit-text-stroke: 2px var(--c-accent);
}

.features__grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(260px, 1fr));
  gap: 1.25rem;
  max-width: 1100px;
  margin: 0 auto;
}

.feat-card {
  position: relative;
  background: var(--c-surface);
  border: 1px solid var(--c-border);
  border-radius: 16px;
  padding: 2rem;
  overflow: hidden;
  opacity: 0;
  animation: fadeUp 0.6s ease both;
  transition: border-color 0.3s, transform 0.3s;
  cursor: default;
}

.feat-card::before {
  content: '';
  position: absolute;
  inset: 0;
  background: radial-gradient(circle at 50% 0%, rgba(0, 255, 180, 0.05) 0%, transparent 70%);
  opacity: 0;
  transition: opacity 0.3s;
}

.feat-card:hover { border-color: rgba(0, 255, 180, 0.25); transform: translateY(-6px); }
.feat-card:hover::before { opacity: 1; }

.feat-card__icon {
  font-size: 1.75rem;
  margin-bottom: 1rem;
  animation: spin-slow 20s linear infinite;
  display: inline-block;
}

.feat-card__title {
  font-family: var(--font-display);
  font-size: 1.5rem;
  letter-spacing: 0.05em;
  margin-bottom: 0.6rem;
  color: var(--c-text);
}

.feat-card__desc {
  font-size: 0.88rem;
  line-height: 1.65;
  color: var(--c-muted);
  font-weight: 300;
}

.feat-card__line {
  position: absolute;
  bottom: 0;
  left: 0;
  right: 0;
  height: 2px;
  background: linear-gradient(90deg, transparent, var(--c-accent), transparent);
  transform: scaleX(0);
  transition: transform 0.4s ease;
}
.feat-card:hover .feat-card__line { transform: scaleX(1); }

/* ─── CTA BAND ─── */
.cta-band {
  position: relative;
  z-index: 10;
  text-align: center;
  padding: 6rem 2rem;
  overflow: hidden;
}

.cta-band__glow {
  position: absolute;
  inset: 0;
  background: radial-gradient(ellipse at center, rgba(0, 255, 180, 0.08) 0%, transparent 70%);
  pointer-events: none;
}

.cta-band__inner {
  position: relative;
  z-index: 2;
}

.cta-band__title {
  font-family: var(--font-display);
  font-size: clamp(3rem, 7vw, 6rem);
  letter-spacing: 0.03em;
  line-height: 1;
  margin-bottom: 1rem;
}

.cta-band__sub {
  color: var(--c-muted);
  font-size: 1rem;
  font-weight: 300;
  margin-bottom: 2.5rem;
}

/* ─── FOOTER ─── */
.footer {
  position: relative;
  z-index: 10;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.75rem;
  padding: 2rem;
  border-top: 1px solid var(--c-border);
  font-size: 0.75rem;
  letter-spacing: 0.1em;
  color: var(--c-muted);
}

/* ─── MOBILE ─── */
@media (max-width: 768px) {
  .nav { padding: 1rem 1.25rem; }
  .nav__links { display: none; }

  .hero { padding: 7rem 1.5rem 3rem; }
  .hero__title { font-size: clamp(4rem, 18vw, 6rem); }

  .stats { padding: 4rem 1.5rem; }
  .features { padding: 3rem 1.5rem 5rem; }

  .features__grid { grid-template-columns: 1fr; }
  .stats__grid { grid-template-columns: 1fr 1fr; }

  .hero__actions { flex-direction: column; align-items: flex-start; }

  .cta-band { padding: 4rem 1.5rem; }
}

@media (max-width: 480px) {
  .stats__grid { grid-template-columns: 1fr; }
  .nav__cta { display: none; }
}
</style>