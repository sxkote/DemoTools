<template>
    <div class="container">
        <div class="row">
            <div class="offset-4 col-4">
                <Loader v-bind:isLoading="isLoading" title="changing password...">
                    <template v-slot>
                        <div class="panel panel-default border">
                            <div class="panel-body container">
                                <div class="row mb-4">
                                    <div class="col text-center">
                                        <h2 class="text-dark">Change Password</h2>
                                    </div>
                                </div>
                                <div class="row mb-4">
                                    <div class="col-12">
                                        <div class="form-group">
                                            <label class="control-label">Password</label>
                                            <input type="password" placeholder="Password" class="form-control" v-model="model.PasswordCurrent" required />
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
                                <div class="row">
                                    <div class="col-12">
                                        <div>
                                            <button class="btn btn-primary float-end" @click="changePassword">Change Password</button>
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
    import { IChangePasswordModel, IProfileService } from '../../../interfaces/main/profile.interface';
    import { ICommonService } from '../../../interfaces/interfaces';

    @Options({
        components: {
            Loader
        },
    })
    export default class ChangePasswordView extends Vue {
        private profileService: IProfileService;
        private commonService: ICommonService;

        @Inject(SYMBOLS.CONTAINER)
        private _container: Container;

        model: IChangePasswordModel = {
            PasswordCurrent: "",
            Password: "",
            PasswordConfirm: ""
        };
        isLoading: boolean = false;

        created(): void {
            this.commonService = this._container.get<ICommonService>(SYMBOLS.ICommonService);
            this.profileService = this._container.get<IProfileService>(SYMBOLS.IProfileService);
        }

        changePassword() {
            var $self = this;
            this.isLoading = true;
            this.profileService.changePassword(this.model)
                .then((data:any) => {
                    $self.commonService.webNotifyOk("Your password has been changed!");
                    $self.isLoading = false;
                    router.push({ name: 'Profile' });
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