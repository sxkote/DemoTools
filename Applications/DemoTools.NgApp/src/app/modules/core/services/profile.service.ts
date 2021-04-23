import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {map, tap} from 'rxjs/operators';

import {SharedService} from '../../../shared/services/shared.service';
import {IChangePasswordModel, IPasswordRecoveryModel, IRegistrationModel, ProfileModel} from '../classes/profile.class';



@Injectable({
  providedIn: 'root',
})
export class ProfileService {

  constructor(private http: HttpClient, private sharedService: SharedService) {
  }

  private get APIURL(): string {
    return this.sharedService.API_CORE_URL;
  }

  getProfile(): Observable<ProfileModel> {
    return this.http.get(this.APIURL + 'profile')
      .pipe(map(result => new ProfileModel(result)));
  }

  registrationInit(model: IRegistrationModel): Observable<any> {
    return this.http.post(this.APIURL + 'profile/registration-init', model);
  }

  registrationConfirm(activityID: string, pin: string): Observable<any> {
    return this.http.post(this.APIURL + 'profile/registration-confirm/' + activityID + '/' + pin, null);
  }

  passwordRecoveryInit(model: IPasswordRecoveryModel): Observable<any> {
    return this.http.post(this.APIURL + 'profile/password-recovery-init', model);
  }

  passwordRecoveryConfirm(activityID: string, pin: string): Observable<any> {
    return this.http.post(this.APIURL + 'profile/password-recovery-confirm/' + activityID + '/' + pin, null);
  }

  changePassword(model: IChangePasswordModel): Observable<any> {
    return this.http.post(this.APIURL + 'profile/password-change', model);
  }
}
