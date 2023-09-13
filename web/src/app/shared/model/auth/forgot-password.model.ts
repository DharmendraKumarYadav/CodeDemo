export class ForgotPasswordModel{
    customerid:string;
    userid:string;
}

export class PasswordExpiredModel{
    customerid:string;
    userid:string;
    currentPasseord:string;
    OtpHint:string;
    action:string;
    otpMode:string;
}