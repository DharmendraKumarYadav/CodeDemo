export class ApplicationMessage {
    static ERROR_MESSAGE = [
        {code:"ER001", message: 'We are facing some technical issues. Please try after some time.' },
        {code:"ER004", message: 'Page not found.' },
    ];
    static GENERAL_MESSAGE = [
        {code:"MSG001", message: 'You have logged - in with unauthorised IP Address' },//Connect Online
        {code:"MSG002", message: 'You have logged - in with unauthorised IP Address' },//IndusDirect
        {code:"MSG003", message: 'Access denied, please contact our Phone Banking on 1800 266 0616 or write to us at idcsupport@indusind.com' },//IndusDirect
        {code:"MTN001", message: 'Application under maintenance. Please try after some time.' },
 
    ];
    static COMMON_MESSAGE = [
        {code:"0000", message: 'We are facing some technical issues. Please try after some time.' },
       
    ];
}


