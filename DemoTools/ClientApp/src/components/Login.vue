<template>
    <div class="container">
        <div class="row">
            <div class="offset-4 col-4">
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
                                    <input type="text" placeholder="Login" class="form-control" v-model="login" required/>
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
                            <div class="col-sm-6">
                                <button class="btn btn-secondary" @click="register">Register</button>
                            </div>
                            <div class="col-sm-6">
                                <button class="btn btn-primary float-right" @click="authorize">Login</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>


<script>
    export default {
        name: "Login",
        data() {
            return {
                login: '',
                password: ''
            }
        },
        methods: {
            incrementCounter() {
                this.currentCount++;
            },
            authorize() {
                console.log(this.login + ' & ' + this.password);
            },
            register() {
                console.log('go to register page || register modal');
            },
        }
    }
</script>

<style scoped>
    .panel {
        width: 100%;
        /* max-width: 600px;*/
        margin-top: 150px;
    }

        .panel .panel-body {
            margin-top: 20px;
            margin-bottom: 20px;
        }
</style>