import { __decorate, __metadata, __param } from "tslib";
import { inject, injectable } from "inversify";
import SYMBOLS from "@/configs/symbols";
import { Token } from '@/classes/token';
import axios from "axios";
import { store } from '@/configs/store.config';
let AuthenticationService = class AuthenticationService {
    constructor(logger) {
        this.logger = logger;
    }
    applyToken(token, inLocalStorage, inStore, inHeaders) {
        if (inLocalStorage) {
            if (!!token)
                localStorage.setItem('token', JSON.stringify(token));
            else
                localStorage.removeItem('token');
        }
        if (inStore) {
            store.dispatch('setToken', { token: token });
        }
        if (inHeaders) {
            if (!!token)
                axios.defaults.headers.common['Authorization'] = 'Token ' + token.TokenID;
            else
                axios.defaults.headers.common['Authorization'] = 'Token';
        }
    }
    getToken() {
        var token = store.getters.token;
        if (token != null)
            return token;
        // get token from localStorage
        var json = localStorage.getItem('token');
        if (!json)
            return null;
        token = new Token(JSON.parse(json));
        if (!token || !token.isValid())
            return null;
        // apply token from localStorage
        this.applyToken(token, false, true, true);
        return token;
    }
    setToken(token) {
        this.applyToken(token, true, true, true);
    }
    logout() {
        this.applyToken(null, true, true, true);
    }
    authenticate(login, password) {
        var data = {
            Login: login,
            Password: password
        };
        var $srv = this;
        return axios.post(SYMBOLS.APIURL + 'auth/login', data)
            .then(resp => {
            var token = new Token(resp.data);
            $srv.setToken(token);
            return token;
        })
            .catch(err => {
            //commit('auth_error')
            //localStorage.removeItem('token')
            //return null;
            throw 'authorization error';
        });
    }
};
AuthenticationService = __decorate([
    injectable(),
    __param(0, inject(SYMBOLS.ILogger)),
    __metadata("design:paramtypes", [Object])
], AuthenticationService);
export { AuthenticationService };
let SXLogger = class SXLogger {
    log(message) {
        console.log('SXLogger: ' + message);
    }
};
SXLogger = __decorate([
    injectable()
], SXLogger);
export { SXLogger };
//# sourceMappingURL=authentication.service.js.map