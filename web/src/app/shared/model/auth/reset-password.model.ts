import { JsonObject, JsonProperty } from "json2typescript";

export class ResetPasswordModel{
    customerId:string;
    userId:string;
    Code:string;
    password:string;
    newPassword:string;
    passwordConfirm:string;
    OtpHint:string;
    operation:string;
}
export class AvailableOtpBranchModel{
    action:string;
    error_msg:string;
    availableBranches: string[];
}
export class ResetPasswordRequestModel{
    customerId:string;
    userId:string;
    Code:string;
    password:string;
    newPassword:string;
    passwordConfirm:string;
    OtpHint:string;
    operation:string;
}
// export class PasswordResetRequest{
//     customerId:string;
//     userId:string;
//     msg:string;
// }
export class PasswordResetReq{
    password:string;
    passwordConfirm:string;
    operation:string;
}
export class PasswordReset{
    newPassword:string;
    passwordConfirm:string;
    operation:string;
}
@JsonObject("PasswordResetRequest")
export class PasswordResetRequest {
    @JsonProperty("customerid", String)
    customerid: string = undefined;
    @JsonProperty("msg", String)
    msg: string = undefined;;
    @JsonProperty("userid", String)
    userid: string = undefined;;
    @JsonProperty("g-recaptcha-response", String)
    token: string = undefined;
}  
export class PasswordExpiredRequest {
    customerid: string ;
    msg: string ;
    userid: string ;
    password: string ;
} 


