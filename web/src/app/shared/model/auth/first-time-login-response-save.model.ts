export class FirstTimeloginResponseSave {
    customerid: string;
    userid: string;
    currentPasseord: string;
    OtpHint: string;
    action: string;
    otpMode: string;
}   

export class FirstTimeloginModel {
    customerid: string;
    userid: string;
    code: string;
    password: string;
    newPassword: string;
    terms: string;
    passwordConfirm: string;
}  