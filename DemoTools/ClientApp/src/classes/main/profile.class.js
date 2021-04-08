export class RegistrationResponse {
    constructor(data) {
        this.ID = "";
        if (data != undefined && data != null) {
            this.ID = data.ID || "";
        }
    }
}
//# sourceMappingURL=profile.class.js.map