import { ProfileModel } from "../../classes/main/profile.class";

export interface IRegistrationModel {
    Login: string;
    Email: string;
    NameFirst: string;
    NameLast: string;
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

export interface IProfileService {
    getProfile(): Promise<ProfileModel>;
    registrationInit(model: IRegistrationModel): Promise<object>;
    registrationConfirm(activityID: string, pin: string): Promise<void>;
    passwordRecoveryInit(model: IPasswordRecoveryModel): Promise<object>;
    passwordRecoveryConfirm(activityID: string, pin: string): Promise<void>;
    changePassword(model: IChangePasswordModel): Promise<object>;
} 