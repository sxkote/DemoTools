import { IToken } from 'src/app/interfaces/token.interface';

export interface IAuthenticationService {
  authenticate(login: string, password: string): Promise<IToken>;
  getToken(): IToken | null;
  setToken(token: IToken): void;
  logout(): void;
}
