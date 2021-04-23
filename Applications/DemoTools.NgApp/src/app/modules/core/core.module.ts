import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule } from '@angular/forms';

import { LoginComponent } from './views/login/login.component';
import { AuthenticationService } from './services/authentication.service';
import {TokenInterceptor} from './services/token.interceptor';
import { RegistrationComponent } from './views/registration/registration.component';
import {ProfileService} from './services/profile.service';
import {RouterModule} from '@angular/router';
import { ProfileComponent } from './views/profile/profile.component';


@NgModule({
  declarations: [
    LoginComponent,
    RegistrationComponent,
    ProfileComponent
  ],
  imports: [
    FormsModule,
    BrowserModule,
    NgbModule,
    HttpClientModule,
    RouterModule
  ],
  providers: [
    AuthenticationService,
    ProfileService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    }
  ],
})
export class CoreModule { }
