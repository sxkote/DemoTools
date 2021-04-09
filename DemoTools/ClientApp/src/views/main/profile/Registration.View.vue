<template>
    <div class="container">
        <div class="row">
            <div class="offset-4 col-4">
                <Loader v-bind:isLoading="isLoading" title="registration...">
                    <template v-slot>
                        <div class="panel panel-default border">
                            <div class="panel-body container">
                                <div class="row mb-4">
                                    <div class="col text-center">
                                        <h2 class="text-dark">Registration</h2>
                                    </div>
                                </div>
                                <div class="row mb-4">
                                    <div class="col-12">
                                        <div class="form-group">
                                            <label class="control-label">Email</label>
                                            <input type="email" placeholder="john-doe@gmail.com" class="form-control" v-model="model.Email" required />
                                        </div>
                                    </div>
                                </div>
                                <div class="row mb-4">
                                    <div class="col-12">
                                        <div class="form-group">
                                            <label class="control-label">First Name</label>
                                            <input type="text" placeholder="John" class="form-control" v-model="model.NameFirst" required />
                                        </div>
                                    </div>
                                </div>
                                <div class="row mb-4">
                                    <div class="col-12">
                                        <div class="form-group">
                                            <label class="control-label">Last Name</label>
                                            <input type="text" placeholder="Doe" class="form-control" v-model="model.NameLast" required />
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
                                            <button class="btn btn-primary float-end" @click="registrationConfirm">Complete Registration</button>
                                        </div>
                                        <div v-else>
                                            <button class="btn btn-primary float-end" @click="registrationInit">Start Registration</button>
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
    import { IRegistrationModel, IProfileService } from '../../../interfaces/main/profile.interface';
import { ICommonService } from '../../../interfaces/interfaces';

    @Options({
        components: {
            Loader
        },
    })
    export default class RegistrationView extends Vue {
        private profileService: IProfileService;
        private commonService: ICommonService;

        @Inject(SYMBOLS.CONTAINER)
        private _container: Container;

        activityID: string;
        pin: string = "";
        model: IRegistrationModel = {
            Login: "",
            Email: "",
            NameFirst: "",
            NameLast: "",
            Password: "",
            PasswordConfirm: ""
        };
        isLoading: boolean = false;
        isInited: boolean = false;

        created(): void {
            this.commonService = this._container.get<ICommonService>(SYMBOLS.ICommonService);
            this.profileService = this._container.get<IProfileService>(SYMBOLS.IProfileService);
        }

        registrationInit() {
            var $self = this;
            this.isLoading = true;
            this.profileService.registrationInit(this.model)
                .then((data: any) => {
                    $self.commonService.webNotifyOk("Confirmation e-mail was sent to " + $self.model.Email + ".")
                    $self.activityID = data;
                    $self.isLoading = false;
                    $self.isInited = true;
                })
                .catch(err => {
                    $self.commonService.webNotifyException(err);
                    $self.isLoading = false;
                });
        }

        registrationConfirm() {
            var $self = this;
            this.isLoading = true;
            this.profileService.registrationConfirm(this.activityID, this.pin)
                .then(() => {
                    $self.commonService.webNotifyOk("Thank you for registration!")
                    $self.isLoading = false;
                    router.push({ name: 'Login' });
                })
                .catch(err => {
                    $self.commonService.webNotifyException(err);
                    $self.isLoading = false;
                });
        }

    }
</script>

<style scoped>
    .container {
        margin-top: 50px;
    }

    .panel {
        width: 100%;
    }

        .panel .panel-body {
            margin-top: 20px;
            margin-bottom: 20px;
        }
</style>