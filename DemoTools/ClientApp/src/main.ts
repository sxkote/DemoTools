import 'bootstrap/dist/css/bootstrap.css'
import { createApp } from 'vue'
import { createStore } from 'vuex'
import Axios from 'axios'

import App from './App.vue'
import router from './router'

const app = createApp(App);
const store = createStore({});
app.use(store).use(router).mount('#app')
