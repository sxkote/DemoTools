<template>
    <div class="container">
        <div class="row">
            <div class="offset-4 col-4">
                <Loader v-bind:isLoading="isLoading" title="authorization...">
                    <template v-slot>
                        <div class="panel panel-default border">
                            <div class="panel-body container">
                                <div class="row mb-4">
                                    <div class="col text-center">
                                        <h2 class="text-dark">Authorization</h2>
                                    </div>
                                </div>
                                <div class="row mb-4">
                                    <div class="col-12">
                                        <div class="form-group">
                                            <input type="text" placeholder="Login" class="form-control" v-model="login" required />
                                        </div>
                                    </div>
                                </div>
                                <div class="row mb-4">
                                    <div class="col-12">
                                        <div class="form-group">
                                            <input type="password" placeholder="Password" class="form-control" v-model="password" required />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-12">
                                        <button class="btn btn-secondary" @click="register">Register</button>
                                        <button class="btn btn-primary float-end" @click="authorize">Login</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </template>
                </Loader>
            </div>
        </div>
    </div>
</template>


<script lang="ts">
    import { Vue, Options } from 'vue-class-component';
    import { Inject } from 'vue-property-decorator';
    import { Container } from "inversify";
    import SYMBOLS from '@/configs/symbols';
    import router from '@/configs/router.config';
    import { IAuthenticationService } from '@/interfaces/interfaces';
    import Loader from '@/components/Loader.vue';

    @Options({
        components: {
            Loader
        },
    })
    export default class Login extends Vue {
        private authService: IAuthenticationService;

        @Inject(SYMBOLS.CONTAINER)
        private _container: Container;

        login: string = "";
        password: string = "";
        isLoading: boolean = false;

        created(): void {
            this.authService = this._container.get<IAuthenticationService>(SYMBOLS.IAuthenticationService);
        }

        authorize() {
            var $self = this;
            this.isLoading = true;
            this.authService.authenticate(this.login, this.password)
                .then(token => {
                    //$self.$store.dispatch('setToken', { token: token });
                    $self.isLoading = false;
                    router.push({ name: 'TodoLists'});
                })
                .catch(err => {
                    $self.isLoading = false;
                    console.log(err);
                });
        }

        register() {
            var value = this.$store.getters.userName;
            console.log('current token: ' + value);
        }
    }
</script>

<style scoped>
    .container {
        margin-top: 150px;
    }

    .panel {
        width: 100%;
    }

        .panel .panel-body {
            margin-top: 20px;
            margin-bottom: 20px;
        }
</style>