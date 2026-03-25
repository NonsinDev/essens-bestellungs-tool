<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import Button from 'primevue/button'
import InputText from 'primevue/inputtext'

const router = useRouter()

const username = ref('')
const password = ref('')
const errorMsg = ref('')
function handleLogin() {
  errorMsg.value = ''

  if (!username.value.trim() || !password.value.trim()) {
    errorMsg.value = 'Bitte Benutzername und Passwort eingeben.'
    return
  }

  router.push('/employee-home')
}
</script>

<template>
  <main class="employee-login-page">
    <section class="login-card">
      <p class="label">Intern</p>
      <h1>Mitarbeiter-Login</h1>

      <form class="login-form" @submit.prevent="handleLogin">
        <label class="field">
          <span>Benutzername</span>
          <InputText
            v-model="username"
            placeholder="Benutzername"
            autocomplete="username"
          />
        </label>

        <label class="field">
          <span>Passwort</span>
          <InputText
            v-model="password"
            type="password"
            placeholder="Passwort"
            autocomplete="current-password"
          />
        </label>

        <div v-if="errorMsg" class="error-msg">
          <i class="pi pi-exclamation-circle"></i>
          {{ errorMsg }}
        </div>

        <Button
          type="submit"
          label="Einloggen"
        />
      </form>
    </section>
  </main>
</template>

<style scoped>
.employee-login-page {
  min-height: 100vh;
  display: grid;
  place-items: center;
  padding: 24px;
  background: #f1f0ec;
}

.login-card {
  width: min(420px, 100%);
  padding: 32px;
  border: 1px solid #d7d4cd;
  border-radius: 18px;
  background: #ffffff;
  box-shadow: 0 16px 40px rgba(23, 23, 23, 0.06);
}

.label {
  margin: 0 0 10px;
  font-size: 12px;
  font-weight: 700;
  letter-spacing: 0.1em;
  text-transform: uppercase;
  color: #6b7280;
}

h1 {
  margin: 0 0 12px;
  color: #111827;
}

.intro {
  margin: 0 0 24px;
  color: #4b5563;
  line-height: 1.6;
}

.login-form {
  display: grid;
  gap: 16px;
}

.field {
  display: grid;
  gap: 8px;
  color: #374151;
  font-size: 14px;
}

.field :deep(input) {
  width: 100%;
  padding: 12px 14px;
  border-radius: 12px;
}

.notice {
  margin: 18px 0 0;
  color: #065f46;
  line-height: 1.5;
}
</style>