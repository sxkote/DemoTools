import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router'
import { store } from '@/configs/store.config'

const routes: Array<RouteRecordRaw> = [
    {
        path: "/",
        name: "Home",
        component: () => import(/* webpackChunkName: "about" */ '@/views/Home.View.vue')
    },
    {
        path: "/Login",
        name: "Login",
        component: () => import(/* webpackChunkName: "about" */ '@/views/Login.View.vue')
    },
    {
        path: "/Registration",
        name: "Registration",
        component: () => import(/* webpackChunkName: "about" */ '@/views/Registration.View.vue')
    },
    {
        path: "/Todo",
        name: "TodoLists",
        component: () => import(/* webpackChunkName: "about" */ '@/views/main/todo/TodoLists.View.vue'),
        meta: {
            requiresAuth: true
        }
    },
    {
        path: "/Todo/:id",
        name: "TodoList",
        component: () => import(/* webpackChunkName: "about" */ '@/views/main/todo/TodoList.View.vue'),
        meta: {
            requiresAuth: true
        }
    },
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
