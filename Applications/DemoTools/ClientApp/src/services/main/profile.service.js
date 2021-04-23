import { __decorate } from "tslib";
import { injectable } from "inversify";
import SYMBOLS from "@/configs/symbols";
import axios from "axios";
import { ProfileModel } from "../../classes/main/profile.class";
let ProfileService = class ProfileService {
    getProfile() {
        return axios.get(SYMBOLS.APIURL + 'profile')
            .then(response => {
            return new ProfileModel(response.data);
        });
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
    passwordRecoveryInit(model) {
        return axios.post(SYMBOLS.APIURL + 'profile/password-recovery-init', model)
            .then(response => {
            return response.data;
        });
    }
    passwordRecoveryConfirm(activityID, pin) {
        return axios.post(SYMBOLS.APIURL + 'profile/password-recovery-confirm/' + activityID + '/' + pin);
    }
    changePassword(model) {
        return axios.post(SYMBOLS.APIURL + 'profile/password-change', model);
    }
};
ProfileService = __decorate([
    injectable()
], ProfileService);
export { ProfileService };
//# sourceMappingURL=profile.service.js.map