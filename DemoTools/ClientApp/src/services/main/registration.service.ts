import { inject, injectable } from "inversify";
import SYMBOLS from "@/configs/symbols";
import axios from "axios";
import { IRegistrationModel, IRegistrationService } from "../../interfaces/main/registration.interface";

@injectable()
export class RegistrationService implements IRegistrationService {
    public constructor(
    ) {
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
}
