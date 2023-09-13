import { JsonObject, JsonProperty } from "json2typescript";

@JsonObject("LoginModel")
export class LoginModel {
    @JsonProperty("operation", String)
    operation: string = undefined;
    @JsonProperty("customerid", String)
    customerid: string = undefined;
    @JsonProperty("password", String)
    password: string = undefined;;
    @JsonProperty("userid", String)
    userid: string = undefined;
    @JsonProperty("g-recaptcha-response", String)
    token: string = undefined;
}
@JsonObject("LoginModelEncrypt")
export class LoginModelEncrypt {
    @JsonProperty("operation", String)
    operation: string = undefined;
    @JsonProperty("customerid", String)
    customerid: string = undefined;
    @JsonProperty("password", String)
    password: string = undefined;;
    @JsonProperty("userid", String)
    userid: string = undefined;
    @JsonProperty("g-recaptcha-response", String)
    token: string = undefined;
}  
