<template>
    <div class="container">
        <h2 class="text-center">{{ title }}</h2>
        <h4>Used IT Stack</h4>
        <ul>
            <li>ASP.NET Core 5.0 API</li>
            <li>PostgreSQL</li>
            <li>Entity Framework Core</li>
            <li>Migrations</li>
            <li>Docker</li>
            <li>Git</li>
            <li>VUE 3.0 (Vuex)</li>
            <li>TypeScript</li>
            <li>JavaScript</li>
            <li>Bootstrap</li>
            <li>SASS, CSS 3</li>
            <li>HTML 5</li>
            <li>REST, JSON</li>
        </ul>
        <!--<div class="test">
            <div>
                <button class="btn btn-light" @click="toggleLoading()">toggle loading state</button>
            </div>
            <Loader title="loading..." v-bind:isLoading="isLoading">
                <template v-slot>
                    здесь данные после загрузки
                </template>
            </Loader>
        </div>-->
    </div>
</template>


<script lang="ts">
    import { Options, Vue } from 'vue-class-component';
    import Loader from '@/components/Loader.vue';

    @Options({
        props: {
            title: String
        },
        components: {
            Loader
        },
    })
    export default class Info extends Vue {
        title!: string
        isLoading: boolean = false;


        toggleLoading(): void {
            this.isLoading = !this.isLoading;
        }
    }
</script>
