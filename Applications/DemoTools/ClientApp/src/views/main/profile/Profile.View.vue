<template>
    <div class="container">
        <h2>Profile</h2>

        <div v-if="isLoaded" class="mt-4">
            <div class="item">
                <div class="label">Subscription ID:</div>
                <div class="value">{{profile.SubscriptionID}}</div>
            </div>
            <div class="item">
                <div class="label">Login:</div>
                <div class="value">{{profile.Login}}</div>
            </div>
            <div class="item">
                <div class="label">Email:</div>
                <div class="value">{{profile.Email}}</div>
            </div>
            <div class="item">
                <div class="label">Name:</div>
                <div class="value">{{profile.NameFirst}} {{profile.NameLast}}</div>
            </div>
            <div class="item">
                <div class="label">Telephone:</div>
                <div class="value">{{profile.Telephone}}</div>
            </div>
        </div>

        <div class="mt-4">
            <router-link :to="{ name: 'ChangePassword' }" class="btn btn-secondary">Change Password</router-link>
        </div>
    </div>
</template>


<script lang="ts">
    import { Vue, Options } from 'vue-class-component';
    import { Inject } from 'vue-property-decorator';
    import { Container } from "inversify";
    import SYMBOLS from '@/configs/symbols';
    import { IProfileService } from '../../../interfaces/main/profile.interface';
    import { ProfileModel } from '../../../classes/main/profile.class';

    @Options({
        components: {
        },
    })
    export default class ProfileView extends Vue {

        private profileService: IProfileService;
        private isLoaded: boolean = false;
        private profile: ProfileModel;

        @Inject(SYMBOLS.CONTAINER)
        private _container: Container;

        created(): void {
            this.profileService = this._container.get<IProfileService>(SYMBOLS.IProfileService);
            this.reload();
        }

        reload(): void {
            var $self = this;
            this.profileService.getProfile()
                .then(profile => {
                    $self.profile = profile;
                    $self.isLoaded = true;
                });
        }

    }
</script>


<style scoped>
    .list-group-item > .btn-group {
        visibility: hidden;
    }

    .list-group-item:hover > .btn-group {
        visibility: visible;
    }

    div.item {
        margin-top: 10px;
        margin-bottom: 10px;
    }

    div.label {
        display: inline-block;
        width: 170px;
        font-weight: bold;
    }

    div.value {
        display: inline-block;
    }
</style>