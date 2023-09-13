import { JsonObject, JsonProperty } from "json2typescript";

@JsonObject("UnblockUserRequest")
export class UnblockUserRequest {
    @JsonProperty("customerid", String)
    customerid: string = undefined;
    @JsonProperty("userid", String)
    userid: string = undefined;
    @JsonProperty("msg", String)
    msg: string = undefined;;
    @JsonProperty("g-recaptcha-response", String)
    token: string = undefined;
}  