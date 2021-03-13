<template>
    <div v-if="isLoading">
        <span>loading...</span>
    </div>
</template>




<script lang="ts">
    import { Options, Vue } from 'vue-class-component';

    @Options({
        props: {
            name: String,
            isLoading: Boolean
        }
    })
    export default class Loader extends Vue {
    }
</script>

