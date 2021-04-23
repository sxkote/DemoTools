import {SharedService} from './../../../shared/services/shared.service';

import {Injectable} from '@angular/core';
import {HttpClient, HttpErrorResponse} from '@angular/common/http';
import {Observable, of, throwError} from 'rxjs';
import {catchError, retry, tap} from 'rxjs/operators';
import {Token} from '../classes/token.class';
import {IToken} from 'src/app/interfaces/token.interface';


@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {
  private token: IToken | null = null;

  constructor(private http: HttpClient, private sharedService: SharedService) {
  }

  private get APIURL(): string {
    return this.sharedService.API_CORE_URL;
  }

  private applyToken(token: IToken | null, inLocalStorage: boolean, inHeaders: boolean): void {
    this.token = !!token ? new Token(token) : null;

    if (inLocalStorage) {
      if (!!this.token) {
        localStorage.setItem('token', JSON.stringify(this.token));
      } else {
        localStorage.removeItem('token');
      }
    }

    if (inHeaders) {
      if (!!token) {
        // axios.defaults.headers.common['Authorization'] = 'Token ' + token.TokenID;
      } else {
        // axios.defaults.headers.common['Authorization'] = 'Token';
      }
    }
  }


  public get Token(): IToken | null {
    return this.getToken();
  }

  getToken(): IToken | null {
    if (this.token && this.token.isValid()) {
      return this.token;
    }

    // read token data from local storage
    const json = localStorage.getItem('token');
    if (!json) {
      return null;
    }

    // convert token data to token
    const token = new Token(JSON.parse(json));
    if (!token || !token.isValid()) {
      return null;
    }

    // apply token from localStorage
    this.applyToken(token, false, true);

    return this.token;
  }

  authenticate(login: string, password: string): Observable<IToken> {
    const data = {
      Login: login,
      Password: password
    };

    return this.http.post<IToken>(this.APIURL + 'auth/login', data)
      .pipe(
        tap(result => this.applyToken(new Token(result), true, true))
      );
  }

  logout(): void {
    this.applyToken(null, true, true);
  }
}

