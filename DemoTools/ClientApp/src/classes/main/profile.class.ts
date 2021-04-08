export class RegistrationResponse {
    ID: string = "";

    constructor(data?: any) {
        if (data != undefined && data != null) {
            this.ID = data.ID || "";
        }
    }
}