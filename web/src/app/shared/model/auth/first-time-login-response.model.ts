import { JsonObject, JsonProperty } from "json2typescript";

@JsonObject("FirstTimeloginResponse")
export class FirstTimeloginResponse {
    @JsonProperty("mechanism", String)
    mechanism: string = undefined;
    @JsonProperty("action", String)
    action: string = undefined;
    @JsonProperty("otp.user.otp-hint", String)
    OtpHint: string = undefined;;
    @JsonProperty("otp.user.sentTo", String)
    sentTo: string = undefined;;
    @JsonProperty("error_msg", String)
    error_msg: string = undefined;
}   