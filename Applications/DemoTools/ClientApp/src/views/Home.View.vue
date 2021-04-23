<template>
    <div class="home">
        <!--<img alt="Vue logo" src="../assets/logo.png">-->
        <Info title="Demo Tools by Ivan Litskevich" />
    </div>
</template>

<script lang="ts">
    import { Options, Vue } from 'vue-class-component';
    import Info from '@/components/Info.vue';

    @Options({
        components: {
            Info
        },
    })
    export default class Home extends Vue { }
</script>
