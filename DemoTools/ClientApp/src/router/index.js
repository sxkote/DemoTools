import { createWebHistory, createRouter } from "vue-router";
import Home from "@/components/Home.vue";
import Counter from "@/components/Counter.vue";
import Login from "@/components/Login.vue";
//import FetchData from "@/components/FetchData.vue";

const routes = [
    {
        path: "/",
        name: "Home",
        component: Login,
    },
    {
        path: "/Login",
        name: "Login",
        component: Login,
    },
    {
        path: "/Counter",
        name: "Counter",
        component: Counter,
    },
    //{
    //    path: "/FetchData",
    //    name: "FetchData",
    //    component: FetchData,
    //}
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

export default router;