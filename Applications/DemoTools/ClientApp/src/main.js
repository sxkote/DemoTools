import 'bootstrap/dist/css/bootstrap.css';
import '../public/styles/demo-tools.css';
import { createApp } from 'vue';
//import { BootstrapVue, IconsPlugin } from 'bootstrap-vue'
import { library } from '@fortawesome/fontawesome-svg-core';
import { faUserSecret } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
// Vue Toast Notification
import Toast, { POSITION } from "vue-toastification";
import "vue-toastification/dist/index.css";
library.add(faUserSecret);
//Vue.config.productionTip = false
import App from './App.vue';
import router from '@/configs/router.config';
import { store, key } from '@/configs/store.config';
//// Import Bootstrap an BootstrapVue CSS files (order is important)
//import 'bootstrap/dist/css/bootstrap.css'
//import 'bootstrap-vue/dist/bootstrap-vue.css'
// приложение VUE
const app = createApp(App);
const toastOptions = {
    timeout: 5000,
    position: POSITION.TOP_CENTER
};
app
    .use(store, key)
    .use(router)
    //.use(compositionApi)
    .use(Toast, toastOptions)
    .component('font-awesome-icon', FontAwesomeIcon)
    //.use(BootstrapVue)  // Make BootstrapVue available throughout your project
    //.use(IconsPlugin)   // Optionally install the BootstrapVue icon components plugin
    .mount('#app');
//# sourceMappingURL=main.js.map