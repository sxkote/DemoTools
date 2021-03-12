import { inject, injectable } from "inversify";
import SYMBOLS from "@/configs/symbols";
import { Token } from '@/classes/token'
import { IToken, IAuthenticationService, ILogger } from '@/interfaces/interfaces'

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
    authenticate(login: string, password: string): IToken {
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
}

@injectable()
export class SXLogger1 implements ILogger {
    log(message: string): void {
        console.log('SXLogger-1: ' + message);
    }
}

@injectable()
export class SXLogger2 implements ILogger {
    log(message: string): void {
        console.log('SXLogger-2: ' + message);
    }
}

@injectable()
export class SXLogger3 implements ILogger {
    log(message: string): void {
        console.log('SXLogger-3: ' + message);
    }
}