export class ProfileModel {
    constructor(data) {
        this.PersonID = "";
        this.SubscriptionID = "";
        this.Login = "";
        this.Email = "";
        this.NameFirst = "";
        this.NameLast = "";
        this.Telephone = "";
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
//# sourceMappingURL=profile.class.js.map