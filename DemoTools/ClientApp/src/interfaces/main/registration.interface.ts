export interface IRegistrationModel {
    Login: string;
    Email: string;
    NameFirst: string;
    NameLast: string;
    Password: string;
    PasswordConfirm: string;
}

export interface IRegistrationService {
    registrationInit(model: IRegistrationModel): Promise<object>;
    registrationConfirm(activityID: string, pin: string): Promise<void>;
} 