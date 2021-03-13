import { inject, injectable } from "inversify";
import SYMBOLS from "@/configs/symbols";
import { Token } from '@/classes/token'
import { IToken, IAuthenticationService, ILogger } from '@/interfaces/interfaces'
import axios from "axios";

@injectable()
export class AuthenticationService implements IAuthenticationService {
    private logger: ILogger;

    public constructor(
        @inject(SYMBOLS.ILogger) logger: ILogger
    ) {
        this.logger = logger;
    }

    getToken(): IToken {
        this.logger.log('try to get token');
        throw new Error('Method not implemented.')
    }
    setToken(token: IToken): void {
        this.logger.log('try to set token');
        throw new Error('Method not implemented.')
    }
    authenticate(login: string, password: string): Promise<IToken> {
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
            })
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
}

@injectable()
export class SXLogger implements ILogger {
    log(message: string): void {
        console.log('SXLogger: ' + message);
    }
}
