import {Component, OnInit} from '@angular/core';
import {ProfileService} from '../../services/profile.service';
import {IChangePasswordModel, IProfileModel} from '../../classes/profile.class';
import {NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {SharedService} from '../../../../shared/services/shared.service';

@Component({
  selector: 'dt-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  profile: IProfileModel | null = null;
  modelChangePassword: IChangePasswordModel = {
    PasswordCurrent: '',
    Password: '',
    PasswordConfirm: ''
  };

  constructor(private profileService: ProfileService,
              private sharedService: SharedService,
              private modalService: NgbModal) {
  }

  ngOnInit(): void {
    this.profileService.getProfile()
      .subscribe(result => this.profile = result);
  }

  changePassword(content: any): void {
    this.modelChangePassword.PasswordCurrent = '';
    this.modelChangePassword.Password = '';
    this.modelChangePassword.PasswordConfirm = '';

    this.modalService.open(content, {backdropClass: 'light-blue-backdrop'}).result.then((result) => {
      if (result) {
        this.profileService.changePassword(this.modelChangePassword)
          .subscribe(() => {
            this.sharedService.showToastSuccess('Your password has been changed!');
          });
      }
    }, (reason) => {
    });
  }
}
