export interface IProfileModel {
  PersonID: string;
  SubscriptionID: string;
  Login: string;
  Email: string;
  NameFirst: string;
  NameLast: string;
  Telephone: string;
}

export interface IRegistrationModel {
  Login: string;
  Email: string;
  NameFirst: string;
  NameLast: string;
  Telephone: string;
  Password: string;
  PasswordConfirm: string;
}

export interface IPasswordRecoveryModel {
  Login: string;
  Password: string;
  PasswordConfirm: string;
}

export interface IChangePasswordModel {
  PasswordCurrent: string;
  Password: string;
  PasswordConfirm: string;
}


export class ProfileModel implements IProfileModel {
  PersonID = '';
  SubscriptionID = '';
  Login = '';
  Email = '';
  NameFirst = '';
  NameLast = '';
  Telephone = '';

  constructor(data?: any) {
    if (data !== undefined && data !== null) {
      this.PersonID = data.PersonID;
      this.SubscriptionID = data.SubscriptionID;
      this.Login = data.Login;
      this.Email = data.Email;
      this.NameFirst = data.NameFirst;
      this.NameLast = data.NameLast;
      this.Telephone = data.Telephone;
    }
  }
}
