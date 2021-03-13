<template>
    <div class="hello">
        <h1>{{ msg }}</h1>
        <p>
            For a guide and recipes on how to configure / customize this project,<br>
            check out the
            <a href="https://cli.vuejs.org" target="_blank" rel="noopener">vue-cli documentation</a>.
        </p>
        <Loader name="ajjjsadas" v-bind:isLoading="isLoading"></Loader>
        <button class="btn btn-light" @click="toggleLoading()">toggle loading</button>
    </div>
</template>

<script lang="ts">
    import { Options, Vue } from 'vue-class-component';
    import Loader from '@/components/Loader.vue';

    @Options({
        props: {
            msg: String
        },
        components: {
            Loader
        },
    })
    export default class HelloWorld extends Vue {
        msg!: string
        isLoading: boolean = false;


        toggleLoading(): void {
            this.isLoading = !this.isLoading;
        }
    }
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
    h3 {
        margin: 40px 0 0;
    }

    ul {
        list-style-type: none;
        padding: 0;
    }

    li {
        display: inline-block;
        margin: 0 10px;
    }

    a {
        color: #42b983;
    }
</style>
