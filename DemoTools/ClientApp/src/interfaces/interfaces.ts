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
    getToken(): IToken;
    setToken(token: IToken): void;
}


export interface ILogger {
    log(message: string): void;
}

