import { FormGroup } from '@angular/forms';
export function ComparePassword(controlName: string, matchingControlName: string) {
    let control=controlName;
    let matchControl=matchingControlName;
    return (formGroup: FormGroup) => {
        const controlName = formGroup.controls[control];
        const matchingControlName = formGroup.controls[matchControl];
        if (matchingControlName.errors && !matchingControlName.errors.confirmedValidator) {
            return;
        }
        if (controlName.value !== matchingControlName.value) {
            matchingControlName.setErrors({ confirmedValidator: true });
        } else {
            matchingControlName.setErrors(null);
        }
    }
}