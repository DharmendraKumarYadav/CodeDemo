export class FirstTimeLoginResetPassword{
    newPassword: string;
    passwordConfirm: string;
    operation: string;
}

export class FirstTimeLoginRequest{
    userid: string;
    customerid: string;
    password: string;
    msg:string; 
}

export class TxnPasswordReset{
    newPassword: string;
    passwordConfirm: string;
}