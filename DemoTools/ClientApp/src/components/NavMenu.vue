<template>
    <header>
        <nav class="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
            <div class="container">
                <a class="navbar-brand">Demo Tools</a>
                <button class="navbar-toggler"
                        type="button"
                        data-toggle="collapse"
                        data-target=".navbar-collapse"
                        aria-label="Toggle navigation"
                        @click="toggle">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="navbar-collapse collapse d-sm-inline-flex flex-sm-row-reverse"
                     v-bind:class="{show: isExpanded}">
                    <ul class="navbar-nav flex-grow">
                        <li class="nav-item">
                            <router-link :to="{ name: 'Home' }" class="nav-link text-dark">Home</router-link>
                        </li>
                        <li class="nav-item" v-if="isAuthorized">
                            <router-link :to="{ name: 'TodoLists' }" class="nav-link text-dark">Todos</router-link>
                        </li>
                        <li class="nav-item" v-if="!userName">
                            <router-link :to="{ name: 'Login' }" class="nav-link text-dark">Login</router-link>
                        </li>
                        <li class="nav-item dropdown" v-else>
                            <a class="nav-link dropdown-toggle" id="navbarDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                {{userName}}
                            </a>
                            <div class="dropdown-menu" aria-labelledby="navbarDropdown">
                                <!--<a class="dropdown-item" href="#">Action</a>
                                <a class="dropdown-item" href="#">Another action</a>
                                <div class="dropdown-divider"></div>-->
                                <a class="dropdown-item" href="#" @click="logout()">Logout</a>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
    </header>
</template>


<style>
    a.navbar-brand {
        white-space: normal;
        text-align: center;
        word-break: break-all;
    }

    html {
        font-size: 14px;
    }

    @media (min-width: 768px) {
        html {
            font-size: 16px;
        }
    }

    .box-shadow {
        box-shadow: 0 .25rem .75rem rgba(0, 0, 0, .05);
    }
</style>

<script lang="ts">
    import { Vue } from 'vue-class-component';
    import { Inject } from 'vue-property-decorator';
    import { Container } from "inversify";
    import SYMBOLS from '@/configs/symbols';
    import { IAuthenticationService } from '../interfaces/interfaces';
    import router from '@/configs/router.config';

    export default class NavMenu extends Vue {
        @Inject(SYMBOLS.CONTAINER)
        private _container: Container;

        private authService: IAuthenticationService;

        isExpanded: boolean = false;

        created(): void {
            this.authService = this._container.get<IAuthenticationService>(SYMBOLS.IAuthenticationService);
        }

        collapse() {
            this.isExpanded = false;
        }
        toggle() {
            this.isExpanded = !this.isExpanded;
        }
        get userName(): string | null {
            var token = this.authService.getToken();
            if (!token || !token.isValid())
                return null;

            return token.Name;
        }

        get isAuthorized(): boolean {
            var token = this.authService.getToken();
            return !!token && token.isValid();
        }

        logout(): void {
            this.authService.logout();
            router.push({ name: 'Home' });
        }
    }
</script>