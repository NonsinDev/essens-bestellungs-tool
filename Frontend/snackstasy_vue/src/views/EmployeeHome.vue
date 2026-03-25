<template>
  <main class="landing-page">
    <section class="landing-card">
      <p class="label">Snackstasy</p>
      <h1>Landingpage</h1>
      <p>
        Demo text: Snackstasy ist die zentrale Plattform fuer Tickets, Bestellungen und interne
        Arbeitsablaeufe. Diese Landingpage ist bewusst minimal gehalten und kann spaeter mit
        echten Inhalten erweitert werden.
      </p>
    </section>
  </main>
</template>

<style scoped>
.landing-page {
  min-height: 100vh;
  display: grid;
  place-items: center;
  padding: 24px;
  background: #f7f4ee;
}

.landing-card {
  width: min(680px, 100%);
  padding: 40px;
  border: 1px solid #d6d0c4;
  border-radius: 20px;
  background: #fffdf8;
  color: #2f2a24;
}

.label {
  margin: 0 0 12px;
  font-size: 12px;
  font-weight: 700;
  letter-spacing: 0.12em;
  text-transform: uppercase;
  color: #8a6d46;
}

h1 {
  margin: 0 0 16px;
  font-size: clamp(2rem, 5vw, 3.5rem);
  line-height: 1.05;
}

p {
  margin: 0;
  line-height: 1.7;
}
</style>