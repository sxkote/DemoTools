import { inject, injectable } from "inversify";
import SYMBOLS from "@/configs/symbols";
import { Token } from '@/classes/token'
import { IToken, IAuthenticationService, ILogger } from '@/interfaces/interfaces'
import axios from "axios";
import { store } from '@/configs/store.config'

@injectable()
export class AuthenticationService implements IAuthenticationService {
    private logger: ILogger;

    public constructor(
        @inject(SYMBOLS.ILogger) logger: ILogger
    ) {
        this.logger = logger;
    }

    private applyToken(token: IToken | null, inLocalStorage: boolean, inStore: boolean, inHeaders: boolean): void {
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

    getToken(): IToken | null {
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
    setToken(token: IToken): void {
        this.applyToken(token, true, true, true);
    }
    logout(): void {
        this.applyToken(null, true, true, true);
    }
    authenticate(login: string, password: string): Promise<IToken> {
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
            })
    }
}

@injectable()
export class SXLogger implements ILogger {
    log(message: string): void {
        console.log('SXLogger: ' + message);
    }
}
