export class ValidationMessage {
    static loginForm = {
        'userid': [
            { type: 'required', message: 'User Id or Email Id is required.' },
        ],
        'password': [
            { type: 'required', message: 'Password is required.' }
        ]
    };

    static userProfileForm=
    {
        'fullName': [
            { type: 'required', message: 'Please enter name.' },
        ],
        'email': [
            { type: 'required', message: 'Please enter email id.' },
            { type: 'email', message: 'Please enter valid email id.' },
        ],
        'phoneNumber': [
            { type: 'required', message: 'Please enter mobile number.' },
            { type: 'mobileNumberInvalid', message: 'Please enter valid mobile number.' },
            
        ],    
    };
    static changePasswordForm = {
        'currentPassword': [
            { type: 'required', message: 'Current Password is required.' },
        ],
        'newPassword': [
            { type: 'required', message: 'Password is required.' },
            { type: 'minlength', message: 'Password length should be greater than 8 character' },
            { type: 'maxlength', message: 'Password length should  be less than 15 character' },
            
        ],
        'passwordConfirm': [
            { type: 'required', message: 'Confirm password is required.' },
            { type: 'confirmedValidator', message: 'Password and confirm password does not match.' }
        ],
    };


    static createUserForm=
{
    'fullName': [
        { type: 'required', message: 'Please enter name.' },
    ],
    'email': [
        { type: 'required', message: 'Please enter email id.' },
        { type: 'email', message: 'Please enter valid email id.' },
        
    ],
    'password': [
        { type: 'required', message: 'Password is required.' },
    ],
    'userName': [
        { type: 'required', message: 'Please enter user name.' },
    ],
    'phoneNumber': [
        { type: 'required', message: 'Please enter mobile number.' },
        { type: 'mobileNumberInvalid', message: 'Please enter valid mobile number.' },
    ],
    'role': [
        { type: 'required', message: 'Please select role.' },
    ],
    'dealer': [
        { type: 'required', message: 'Please select dealer.' },
    ],
    'isEnabled': [
        { type: 'required', message: 'Please enable user.' },
    ]
    
    };

    static createDelearForm=
    {
    'name': [
        { type: 'required', message: 'Please enter Name.' },
    ],
    'emailId': [
        { type: 'required', message: 'Please enter email id.' },
        { type: 'email', message: 'Please enter valid email id.' },
    ],
    'mobileNumber': [
        { type: 'required', message: 'Please enter mobile number.' },
        { type: 'mobileNumberInvalid', message: 'Please enter valid mobile number.' },
    ],
    'phoneNumber': [
        { type: 'required', message: 'Please enter phone number.' },
        { type: 'mobileNumberInvalid', message: 'Please enter valid phone number.' },
    ],
    'addressLine2': [
        { type: 'required', message: 'Please enter address line 2.' },
    ],
    'addressLine1': [
        { type: 'required', message: 'Please enter address line 1' },
    ],
    'url': [
        { type: 'required', message: 'Please enter url.' },
    ],
    'areaId': [
        { type: 'required', message: 'Please select area.' },
    ],
'orderId':[
    { type: 'required', message: 'Please enter local order number.' },
    { type: 'numeric', message: 'Order should be numeric.' },
]
    };
    static catalogForm=
    {
        'name': [
            { type: 'required', message: 'Please enter data.' },
        ],
        'type': [
            { type: 'required', message: 'Please select type.' },
        ],
        'description': [
            { type: 'required', message: 'Please enter description.' },
        ],
        'amount': [
            { type: 'required', message: 'Amount is required.' },
            { type: 'numeric', message: 'Amount should be numeric required.' },
            
        ],
        'operator': [
            { type: 'required', message: 'Operator is required.' },
        ],
        'colourCode': [
            { type: 'required', message: 'Colour code is required.' },
        ],
        'value': [
            { type: 'required', message: 'Please enter value.' },
            { type: 'numeric', message: 'Value should be numeric.' },
        ],
        'city': [
            { type: 'required', message: 'City is required.' }
            
        ],
        'pinCode': [
            { type: 'required', message: 'Pin code is required.' },
            { type: 'numeric', message: 'Pin code should be numeric.' },
            { type: 'length', message: 'Pin code should be 6 digit'},
        ],
        
    };
    static saleBikeForm=
    {
    'showRoom': [
        { type: 'required', message: 'Please select show room.' },
    ],
    'bikeVariant': [
        { type: 'required', message: 'Please select bike variant.' }
    ],
    'bikeColour': [
        { type: 'required', message: 'Please select bike colour.' }
    ],
    'amount': [
        { type: 'required', message: 'Amount is required.' },
        { type: 'numeric', message: 'Amount should be numeric required.' },
        { type: 'min', message: 'Amount should be greater than current price.' },
        
    ],
    'availbility':[
        { type: 'required', message: 'Please select availbility.' },
    ],
    'noOfDay':[
        { type: 'required', message: 'Please enter no. of day.' },
        { type: 'numeric', message: 'Please enter numeric digit only.' }
    ],
    'chesisNumber':[
        { type: 'required', message: 'Please enter chesis number.' },
    ],
    'engineNumber':[
        { type: 'required', message: 'Please enter engine number.' },
    ]
    
    };

    static bikeCreateForm=
    {
        'name': [
            { type: 'required', message: 'Please enter bike name.' },
        ],
        'category': [
            { type: 'required', message: 'Please select category.' },
        ],
        'shortDescription': [
            { type: 'required', message: 'Please enter short description.' },
        ],
        'longDescription': [
            { type: 'required', message: 'Please enter long description.' },
        ],
        'brandId': [
            { type: 'required', message: 'Please select brand.' },
        ],
        'price':[
            { type: 'required', message: 'Please enter  price.' },
            { type: 'numeric', message: 'Please enter numeric digit only.' }
        ],
        'colourIds': [
            { type: 'required', message: 'Please select colour.' },
        ],
        'city': [
            { type: 'required', message: 'Please select city.' },
        ],
        'bodyStyle': [
            { type: 'required', message: 'Please select body style.' },
        ],
        'displacement':[
            { type: 'required', message: 'Please enter displacement.' },
            { type: 'numeric', message: 'Please enter numeric digit only.' }
        ],
        'variantName': [
            { type: 'required', message: 'Please enter bike variant name.' },
        ],
        'specification': [
            { type: 'required', message: 'Please enter specification.' },
        ],
        'specific': [
            { type: 'required', message: 'Please select specification.' },
        ],
        'attributes': [
            { type: 'required', message: 'Please select attributes.' },
        ],
        'attributeValue': [
            { type: 'required', message: 'Please enter attribute value.' },
        ]  
    };





    static bikeFeatureForm = {
        'bike': [
            { type: 'required', message: 'Please select bike' }
        ],
        'type': [
            { type: 'required', message: 'Please select type.'}
        ]
    };




//New Form

static createRoleForm=
{
    'name': [
        { type: 'required', message: 'Please enter role.' },
    ],
    'description': [
        { type: 'required', message: 'Please enter description.' },
    ],
    'permissions': [
        { type: 'required', message: 'Permission is required.' },
    ],
    'amount': [
        { type: 'required', message: 'Amount is required.' },
    ],
    'operator': [
        { type: 'required', message: 'Operator is required.' },
    ],
    'colourCode': [
        { type: 'required', message: 'Colour code is required.' },
    ]
    
}



}
