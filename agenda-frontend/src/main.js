import { createApp } from 'vue'
import App from './App.vue'
import PrimeVue from 'primevue/config';
import Aura from '@primevue/themes/aura';
import 'primeicons/primeicons.css';
import {ToastService} from 'primevue';

const app = createApp(App)

app.use(ToastService);

app.use(PrimeVue, {
  theme: {
    preset: Aura
  }
});

app.mount('#app')