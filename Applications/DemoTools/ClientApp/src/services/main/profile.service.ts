import { injectable } from "inversify";
import SYMBOLS from "@/configs/symbols";
import axios from "axios";
import { IRegistrationModel, IPasswordRecoveryModel, IProfileService, IChangePasswordModel } from "../../interfaces/main/profile.interface";
import { ProfileModel } from "../../classes/main/profile.class";

@injectable()
export class ProfileService implements IProfileService {

    getProfile(): Promise<ProfileModel> {
        return axios.get(SYMBOLS.APIURL + 'profile')
            .then(response => {
                return new ProfileModel(response.data);
            });       
    }

    registrationInit(model: IRegistrationModel): Promise<object> {
        return axios.post(SYMBOLS.APIURL + 'profile/registration-init', model)
            .then(response => {
                return response.data;
            });       
    }

    registrationConfirm(activityID: string, pin: string): Promise<void> {
        //var data = {
        //    ActivityID: activityID,
        //    Pin: pin
        //};
        return axios.post(SYMBOLS.APIURL + 'profile/registration-confirm/' + activityID + '/' + pin);
    }

    passwordRecoveryInit(model: IPasswordRecoveryModel): Promise<object> {
        return axios.post(SYMBOLS.APIURL + 'profile/password-recovery-init', model)
            .then(response => {
                return response.data;
            });
    }

    passwordRecoveryConfirm(activityID: string, pin: string): Promise<void> {
        return axios.post(SYMBOLS.APIURL + 'profile/password-recovery-confirm/' + activityID + '/' + pin);
    }

    changePassword(model: IChangePasswordModel): Promise<object> {
        return axios.post(SYMBOLS.APIURL + 'profile/password-change', model);
    }
}
