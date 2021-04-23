import {AuthenticationService} from './../../services/authentication.service';
import {Component, OnInit} from '@angular/core';
import {IToken} from 'src/app/interfaces/token.interface';
import {Router} from '@angular/router';
import {SharedService} from '../../../../shared/services/shared.service';
import {IPasswordRecoveryModel} from '../../classes/profile.class';
import {ProfileService} from '../../services/profile.service';
import {NgbModal} from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'dt-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {

  token: IToken | undefined;

  modelAuth = {
    Login: '',
    Password: ''
  };

  isInited = false;
  pin = '';
  activityID = '';

  modelPasswordRecovery: IPasswordRecoveryModel = {
    Login: '',
    Password: '',
    PasswordConfirm: '',
  };

  constructor(private authenticationService: AuthenticationService,
              private profileService: ProfileService,
              private sharedService: SharedService,
              private router: Router,
              private modalService: NgbModal) {
  }

  ngOnInit(): void {
  }

  authenticate(): void {
    this.authenticationService.authenticate(this.modelAuth.Login, this.modelAuth.Password)
      .subscribe(token => {
        this.token = token;
        this.sharedService.showToastSuccess('Welcome back, ' + this.token.Name + '!');
        this.router.navigate(['/profile']);
      });
  }

  passwordRecovery(content: any): void {
    this.modalService.open(content, {backdropClass: 'light-blue-backdrop'}).result.then((result) => {
      if (result) {
        this.profileService.passwordRecoveryConfirm(this.activityID, this.pin)
          .subscribe(() => {
            this.sharedService.showToastSuccess('Your password has been recovered!');
          });
      }
    }, (reason) => {
    });
  }

  passwordRecoveryInit(): void {
    this.profileService.passwordRecoveryInit(this.modelPasswordRecovery)
      .subscribe(result => {
        this.activityID = result;
        this.isInited = true;
        this.sharedService.showToastSuccess('Pin code was sent to your email. Please check mail to complete password recovery!');
      });
  }
}
