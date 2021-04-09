export class ProfileModel {
    PersonID: string = "";
    SubscriptionID: string = "";
    Login: string = "";
    Email: string = "";
    NameFirst: string = "";
    NameLast: string = "";
    Telephone: string = "";

    constructor(data?: any) {
        if (data != undefined && data != null) {
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