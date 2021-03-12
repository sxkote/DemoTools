<template>
    <nav-menu></nav-menu>
    <router-view />
</template>


<script lang="ts">
    import { Options, Vue } from 'vue-class-component';
    import SYMBOLS from '@/configs/symbols'
    import container from '@/configs/dependency.config'
    import NavMenu from '@/components/NavMenu.vue';

    @Options({
        name: "App",
        components: {
            NavMenu
        },
        provide: {
            [SYMBOLS.CONTAINER]: container
        }
    })
    export default class App extends Vue { }
</script>


<style>
    #app {
        font-family: Avenir, Helvetica, Arial, sans-serif;
        -webkit-font-smoothing: antialiased;
        -moz-osx-font-smoothing: grayscale;
        text-align: center;
        color: #2c3e50;
    }
</style>

