import { __decorate, __metadata } from "tslib";
import { injectable } from "inversify";
import SYMBOLS from "@/configs/symbols";
import axios from "axios";
let RegistrationService = class RegistrationService {
    constructor() {
    }
    registrationInit(model) {
        return axios.post(SYMBOLS.APIURL + 'profile/registration-init', model)
            .then(response => {
            return response.data;
        });
    }
    registrationConfirm(activityID, pin) {
        //var data = {
        //    ActivityID: activityID,
        //    Pin: pin
        //};
        return axios.post(SYMBOLS.APIURL + 'profile/registration-confirm/' + activityID + '/' + pin);
    }
};
RegistrationService = __decorate([
    injectable(),
    __metadata("design:paramtypes", [])
], RegistrationService);
export { RegistrationService };
//# sourceMappingURL=registration.service.js.map