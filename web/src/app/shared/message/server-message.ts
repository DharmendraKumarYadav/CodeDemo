export class ApiResponseMessage {
    static MESSAGE = [
        { code: "00", message: 'Success', displayMessage: "" },
        { code: "01", message: 'Internal server error', displayMessage: "We are facing some technical issues. Please try after some time." },
        { code: "02", message: 'Login required', displayMessage: "Customer is not authenticated, Please enter valid Credentials." },
        
    ];
    static COMMON_MESSAGE = 
        {code:"9999",message:'', displayMessage: 'We are facing some technical issues. Please try after some time. If issue persists, please call 1800 266 0616 or email idcsupport@indusind.com for assistance' }
       
    
}


