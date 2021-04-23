import {Injectable} from '@angular/core';
import {HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpResponse, HttpErrorResponse} from '@angular/common/http';
import {Observable, throwError} from 'rxjs';
import {tap} from 'rxjs/operators';

import {AuthenticationService} from './authentication.service';
import {SharedService} from '../../../shared/services/shared.service';

@Injectable({
  providedIn: 'root',
})
export class TokenInterceptor implements HttpInterceptor {
  constructor(public authenticationService: AuthenticationService, public sharedService: SharedService) {
  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = this.authenticationService.getToken();

    if (token && token.isValid()) {
      request = request.clone({
        setHeaders: {
          Authorization: `Token ${token.TokenID}`
        }
      });
    }
    return next.handle(request).pipe(tap((event: HttpEvent<any>) => {
      if (event instanceof HttpResponse) {
        // do stuff with response if you want
      }
    }, (err: any) => {
      if (err instanceof HttpErrorResponse) {
        if (err.status === 401) {
          // redirect to the login route
          // or show a modal
        }
      }

      if (err.error instanceof ErrorEvent) {
        // A client-side or network error occurred. Handle it accordingly.
        console.error('An error occurred:', err.error.message);
      } else {
        // The backend returned an unsuccessful response code.
        // The response body may contain clues as to what went wrong.
        if (err.status === 401) {
          this.sharedService.showToastMessage('User is not Authorized!', {
            classname: 'bg-danger text-light',
            delay: 8000,
            autohide: true,
            headertext: 'Api Error!'
          });
        } else {
          this.sharedService.showToastMessage(err.error, {
            classname: 'bg-danger text-light',
            delay: 8000,
            autohide: true,
            headertext: 'Api Error!'
          });
        }

        console.error(
          `WebApi status: ${err.status}, ` +
          `body was: ${err.error}`);
      }
    }));
  }
}
