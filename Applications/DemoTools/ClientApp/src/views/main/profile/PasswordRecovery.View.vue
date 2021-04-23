<template>
    <div class="container">
        <div class="row">
            <div class="offset-4 col-4">
                <Loader v-bind:isLoading="isLoading" title="password recovery...">
                    <template v-slot>
                        <div class="panel panel-default border">
                            <div class="panel-body container">
                                <div class="row mb-4">
                                    <div class="col text-center">
                                        <h2 class="text-dark">Password Recovery</h2>
                                    </div>
                                </div>
                                <div class="row mb-4">
                                    <div class="col-12">
                                        <div class="form-group">
                                            <label class="control-label">Login</label>
                                            <input type="text" placeholder="john-doe@gmail.com" class="form-control" v-model="model.Login" required />
                                        </div>
                                    </div>
                                </div>
                                <div class="row mb-4">
                                    <div class="col-12">
                                        <div class="form-group">
                                            <label class="control-label">Password</label>
                                            <input type="password" placeholder="Password" class="form-control" v-model="model.Password" required />
                                        </div>
                                    </div>
                                </div>
                                <div class="row mb-4">
                                    <div class="col-12">
                                        <div class="form-group">
                                            <label class="control-label">Confirm Password</label>
                                            <input type="password" placeholder="Password" class="form-control" v-model="model.PasswordConfirm" required />
                                        </div>
                                    </div>
                                </div>
                                <div class="row mb-4" v-if="isInited">
                                    <div class="col-12">
                                        <div class="form-group">
                                            <label class="control-label">PIN from e-mail</label>
                                            <input type="password" class="form-control" v-model="pin" required />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-12">
                                        <div v-if="isInited">
                                            <button class="btn btn-primary float-end" @click="passwordRecoveryConfirm">Complete Recovery</button>
                                        </div>
                                        <div v-else>
                                            <button class="btn btn-primary float-end" @click="passwordRecoveryInit">Start Recovery</button>
                                        </div>
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
    import Loader from '@/components/Loader.vue';
    import { IPasswordRecoveryModel, IProfileService } from '../../../interfaces/main/profile.interface';

    @Options({
        components: {
            Loader
        },
    })
    export default class PasswordRecoveryView extends Vue {
        private profileService: IProfileService;

        @Inject(SYMBOLS.CONTAINER)
        private _container: Container;

        activityID: string;
        pin: string = "";
        model: IPasswordRecoveryModel = {
            Login: "",
            Password: "",
            PasswordConfirm: ""
        };
        isLoading: boolean = false;
        isInited: boolean = false;

        created(): void {
            this.profileService = this._container.get<IProfileService>(SYMBOLS.IProfileService);
        }

        passwordRecoveryInit() {
            var $self = this;
            this.isLoading = true;
            this.profileService.passwordRecoveryInit(this.model)
                .then((data:any) => {
                    $self.activityID = data;
                    $self.isLoading = false;
                    $self.isInited = true;
                })
                .catch(err => {
                    $self.isLoading = false;
                });
        }

        passwordRecoveryConfirm() {
            var $self = this;
            this.isLoading = true;
            this.profileService.passwordRecoveryConfirm(this.activityID, this.pin)
                .then(() => {
                    $self.isLoading = false;
                    router.push({ name: 'Login' });
                })
                .catch(err => {
                    $self.isLoading = false;
                });
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