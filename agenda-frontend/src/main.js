import { createApp } from 'vue'
import App from './App.vue'

// --- Configuração do PrimeVue (Começa aqui) ---
import PrimeVue from 'primevue/config';

// 1. Importa o "preset" do tema (Aura)
import Aura from '@primevue/themes/aura';

// 2. Importa os ícones
import 'primeicons/primeicons.css';

const app = createApp(App)

// 3. Diz ao Vue para USAR o PrimeVue e aplica o tema
app.use(PrimeVue, {
  theme: {
    preset: Aura
  }
});

app.mount('#app')