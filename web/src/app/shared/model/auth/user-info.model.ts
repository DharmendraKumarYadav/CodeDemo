import { ApplicationModel } from 'src/app/account/model/app.model';



export interface UserInfoResponse {
    userId: string;
    lastLogin: string;
    applications: ApplicationModel[];
    customerId:string;
    customerType:string;
    isTotpEnrolled:boolean;
    isPasswordExpired:boolean;
    userType:string;
    email:string;
    mobile:string;
    isPasswordChangeRequired:boolean;
    isTxnPwdReqd:boolean;
    otpPreference:string;
}









   