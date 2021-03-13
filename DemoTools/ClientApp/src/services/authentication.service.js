import { __decorate, __metadata, __param } from "tslib";
import { inject, injectable } from "inversify";
import SYMBOLS from "@/configs/symbols";
import { Token } from '@/classes/token';
import axios from "axios";
let AuthenticationService = class AuthenticationService {
    constructor(logger) {
        this.logger = logger;
    }
    getToken() {
        this.logger.log('try to get token');
        throw new Error('Method not implemented.');
    }
    setToken(token) {
        this.logger.log('try to set token');
        throw new Error('Method not implemented.');
    }
    authenticate(login, password) {
        var data = {
            Login: login,
            Password: password
        };
        return axios.post('http://localhost:59448/api/auth/login', data)
            .then(resp => {
            var token = new Token(resp.data);
            //localStorage.setItem('token', token)
            axios.defaults.headers.common['Authorization'] = 'Token ' + token.TokenID;
            return token;
        })
            .catch(err => {
            //commit('auth_error')
            //localStorage.removeItem('token')
            //return null;
            throw 'authorization error';
        });
        //this.logger.log('try to login: ' + login + ' & ' + password);
        //var date = new Date();
        //var expire = date;
        //expire.setHours(date.getHours() + 4);
        //return new Token({
        //    TokenID: 'test-token-id',
        //    UserID: 'test-user-id',
        //    Name: 'John Doe',
        //    Date: date,
        //    Expire: expire
        //});
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