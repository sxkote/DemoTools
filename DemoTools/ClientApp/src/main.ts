import 'bootstrap/dist/css/bootstrap.css'
import { createApp } from 'vue'

import App from './App.vue'
import router from '@/configs/router.config'

import { store, key } from '@/configs/store.config'


// приложение VUE
const app = createApp(App);

app.use(store, key).use(router).mount('#app')
