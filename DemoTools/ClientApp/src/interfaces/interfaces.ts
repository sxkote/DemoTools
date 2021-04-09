export interface IToken {

    TokenID: string;
    UserID: string;

    Login: string;
    Name: string;
    Avatar: string;
    Date: Date;
    Expire?: Date;

    Roles: Array<string>;
    Permissions: Array<string>;

    //Params: Array<ParamValue>;

    isValid(): boolean;
    isInRole(role: string): boolean;
    hasPermission(permission: string): boolean;
}

export interface IAuthenticationService {
    authenticate(login: string, password: string): Promise<IToken>;
    getToken(): IToken | null;
    setToken(token: IToken): void;
    logout(): void;
}


export interface ILogger {
    log(message: string): void;
}

export interface ICommonService {
    webNotify(message: string, type: string): void;
    webNotifyOk(message: string): void;
    webNotifyError(message: string): void;
    webNotifyInfo(message: string): void;
    webNotifyException(err: any): void;
}

