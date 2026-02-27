import './assets/main.css'

import { createApp } from 'vue'
import PrimeVue from 'primevue/config'
import App from './App.vue'
import router from './router'
import Button from 'primevue/button'
import 'primeicons/primeicons.css'

const app = createApp(App)
app.use(PrimeVue)
app.use(router)
app.component('Button', Button)

app.mount('#app')
