import {Component, OnInit} from '@angular/core';
import {ProfileService} from '../../services/profile.service';
import {IRegistrationModel} from '../../classes/profile.class';
import {SharedService} from '../../../../shared/services/shared.service';
import {Router} from '@angular/router';

@Component({
  selector: 'dt-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent implements OnInit {

  isInited = false;
  activityID = '';
  pin = '';
  model: IRegistrationModel = {
    Login: '',
    Email: '',
    NameFirst: '',
    NameLast: '',
    Telephone: '',
    Password: '',
    PasswordConfirm: ''
  };

  constructor(private profileService: ProfileService,
              private sharedService: SharedService,
              private router: Router) {
  }

  ngOnInit(): void {
  }

  registrationInit(): void {
    this.profileService.registrationInit(this.model)
      .subscribe(result => {
        this.activityID = result;
        this.isInited = true;
        this.sharedService.showToastSuccess('Pin code was sent to your email. Please check mail to complete the registration!');
      });
  }

  registrationConfirm(): void {
    this.profileService.registrationConfirm(this.activityID, this.pin)
      .subscribe(result => {
        this.sharedService.showToastSuccess('Registration was completed successfully!');
        this.router.navigate(['/login']);
      });
  }
}
