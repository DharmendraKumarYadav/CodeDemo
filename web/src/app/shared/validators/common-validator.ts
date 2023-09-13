import { FormControl, AbstractControl, Validators, ValidatorFn, ValidationErrors } from '@angular/forms';

export function PasswordComplexityValidator(control: AbstractControl): { [key: string]: any } | null {
    if (!control.value) {
        return null;
    }
    const regex=new RegExp('^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[!@$%^&*]).{8,15}$');
    const valid = regex.test(control.value);
    return valid ? null : { passwordComplexity: true };
}
export function WhiteSapceValidator(control: AbstractControl): { [key: string]: any } | null {
   if(control.value !=null){
    if((control.value as string).indexOf(' ') >= 0){
        return {cannotContainSpace: true}
    }

   }
   return null;
}

export function ValidateMobileNumber(control: AbstractControl): { [key: string]: any } | null {
    let regexpNumber = new RegExp('^[0-9]{10}$');
    let isValid = regexpNumber.test(control.value);
    if (!isValid && control.value != '') {
        return { 'mobileNumberInvalid': true };
    }
    return null;
}
export function NumericValidator(control: AbstractControl): { [key: string]: any } | null {
    let regexpNumber = new RegExp('^[0-9]*$');
    let isValid = regexpNumber.test(control.value);
    if (!isValid && control.value != '') {
        return { 'numeric': true };
    }
    return null;
}

export function ValidateEmail(control: AbstractControl): { [key: string]: any } | null {
    let regexpNumber = new RegExp('^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$');
    let isValid = regexpNumber.test(control.value);
    if (!isValid && control.value != '') {
        return { 'emailInvalid': true };
    }
    return null;
}
export function CheckBoxValidation(control: AbstractControl): { [key: string]: any } | null {
    if (control.value == '' || control.value == false) {
        return { 'invalidCheckbox': true };
    }
    return null;
}
export function RequiredIfValidator(value: boolean) {
    return (formControl => {
        if (!formControl.parent) {
            return null
        }
        if (value) {
            return Validators.required(formControl);
        }
        return null;
    })
}
export function LengthValidator(length: number): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } => {
        if (!control.value) {
            return null;
        }
        const valid = control.value.length == length ? true : false;
        return valid ? null : { length: true };
    };
}
export class WhiteSpaceValidatorClass {
    static cannotContainSpace(control: AbstractControl) : ValidationErrors | null {
        if((control.value as string).indexOf(' ') >= 0){
            return {cannotContainSpace: true}
        }
  
        return null;
    }
}