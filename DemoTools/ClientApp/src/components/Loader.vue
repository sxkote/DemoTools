<template>
    <div v-if="isLoading" class="text-center">
        <div class="spinner-border" role="status">
        </div>
        <div>{{title}}</div>
    </div>
    <slot v-else></slot>
</template>


<script lang="ts">
    import { Options, Vue } from 'vue-class-component';

    @Options({
        props: {
            title: String,
            isLoading: Boolean
        },
    })
    export default class Loader extends Vue {
    }
</script>

