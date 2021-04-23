import {IToken} from '../../../interfaces/token.interface';

export class Token implements IToken {
  readonly ROLE_ADMINISTRATOR = 'Administrator';

  TokenID = '';
  UserID = '';
  Login = '';
  Name = '';
  Avatar = '';
  Date: Date = new Date();
  Expire?: Date = new Date();
  Roles: Array<string> = [];
  Permissions: Array<string> = [];
  // Params: Array<ParamValue> = [];

  constructor(data?: any) {
      if (data !== undefined && data !== null) {
          this.TokenID = data.TokenID || '';
          this.UserID = data.UserID || '';
          this.Login = data.Login || '';
          this.Name = data.Name || '';
          this.Avatar = data.Avatar || '';
          this.Date = new Date(data.Date);
          this.Expire = data.Expire ? new Date(data.Expire) : undefined;
          this.Roles = data.Roles || [];
          this.Permissions = data.Permissions || [];

          // this.Params = data.Params ? data.Params.map(p => new ParamValue(p)) : [];
      }
  }

  isValid(): boolean {
      return !!this.TokenID && this.TokenID.length > 0 && !!this.Expire && this.Expire > new Date();
  }

  isInRole(role: string): boolean {
      if (!this.isValid()) {
          return false;
      }

      return this.Roles && this.Roles.length > 0 && this.Roles.indexOf(role) >= 0;
  }

  hasPermission(permission: string): boolean {
      if (!this.isValid()) {
          return false;
      }

      if (this.isAdmin()) {
          return true;
      }

      return this.Permissions && this.Permissions.length > 0 && this.Permissions.indexOf(permission) >= 0;
  }

  isAdmin(): boolean {
      return this.isInRole(this.ROLE_ADMINISTRATOR) || this.isInRole('Admin');
  }


  // setParam(name: string, value: any): void {
  //    var params = this.Params.filter(p => p.Name.toLowerCase() == name.toLowerCase());
  //    if (params && params.length)
  //        params.forEach(p => p.Value = value);
  //    else
  //        this.Params.push(new ParamValue(name, value));
  // }

  setPermissions(permissions: Array<string>): void {
      this.Permissions = permissions || [];
  }
}
