import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router'
import Home from '@/views/Home.vue'
import { store } from '@/configs/store.config'

const routes: Array<RouteRecordRaw> = [
    {
        path: "/",
        name: "Home",
        component: Home
    },
    {
        path: "/Login",
        name: "Login",
        component: () => import(/* webpackChunkName: "about" */ '@/views/Login.vue')
    },
    {
        path: "/Todo",
        name: "TodoLists",
        component: () => import(/* webpackChunkName: "about" */ '@/views/TodoLists.View.vue'),
        meta: {
            requiresAuth: true
        }
    },
    {
        path: "/Todo/:id",
        name: "TodoList",
        component: () => import(/* webpackChunkName: "about" */ '@/views/TodoList.View.vue'),
        meta: {
            requiresAuth: true
        }
    },
    //{
    //    path: "/Counter",
    //    name: "Counter",
    //    component: () => import(/* webpackChunkName: "about" */ '../views/Counter.vue')
    //},
]

const router = createRouter({
    history: createWebHistory(process.env.BASE_URL),
    routes,
});

router.beforeEach((to, from, next) => {
    if (to.matched.some(record => record.meta.requiresAuth)) {
        if (store.getters.isAuthenticated) {
            next()
            return
        }
        next('/Login')
    } else {
        next()
    }
})

export default router
