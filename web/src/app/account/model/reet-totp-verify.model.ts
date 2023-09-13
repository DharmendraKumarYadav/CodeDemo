import { JsonObject, JsonProperty } from "json2typescript";

@JsonObject("ResetTotpVerifyRequest")
export class ResetTotpVerifyRequest {
    @JsonProperty("otp.user.otp", String)
    otp: string = undefined;
    @JsonProperty("otp.user.otp-hint", String)
    OtpHint: string = undefined;;
    @JsonProperty("operation", String)
    operation: string = undefined;
} 

