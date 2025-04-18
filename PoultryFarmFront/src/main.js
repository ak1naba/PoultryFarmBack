import '/src/assets/styles/base.scss'
import '/src/assets/styles/form.scss'

import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './App.vue'
import router from './router'

const app = createApp(App)

app.use(createPinia())
app.use(router)

app.config.devtools = true;

app.mount('#app')
