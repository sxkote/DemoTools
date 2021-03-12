import { __decorate, __metadata, __param } from "tslib";
import { inject, injectable } from "inversify";
import SYMBOLS from "@/configs/symbols";
import { Token } from '@/classes/token';
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
        this.logger.log('try to login: ' + login + ' & ' + password);
        var date = new Date();
        var expire = date;
        expire.setHours(date.getHours() + 4);
        return new Token({
            TokenID: 'test-token-id',
            UserID: 'test-user-id',
            Name: 'John Doe',
            Date: date,
            Expire: expire
        });
    }
};
AuthenticationService = __decorate([
    injectable(),
    __param(0, inject(SYMBOLS.ILogger)),
    __metadata("design:paramtypes", [Object])
], AuthenticationService);
export { AuthenticationService };
let SXLogger1 = class SXLogger1 {
    log(message) {
        console.log('SXLogger-1: ' + message);
    }
};
SXLogger1 = __decorate([
    injectable()
], SXLogger1);
export { SXLogger1 };
let SXLogger2 = class SXLogger2 {
    log(message) {
        console.log('SXLogger-2: ' + message);
    }
};
SXLogger2 = __decorate([
    injectable()
], SXLogger2);
export { SXLogger2 };
let SXLogger3 = class SXLogger3 {
    log(message) {
        console.log('SXLogger-3: ' + message);
    }
};
SXLogger3 = __decorate([
    injectable()
], SXLogger3);
export { SXLogger3 };
//# sourceMappingURL=authentication.service.js.map